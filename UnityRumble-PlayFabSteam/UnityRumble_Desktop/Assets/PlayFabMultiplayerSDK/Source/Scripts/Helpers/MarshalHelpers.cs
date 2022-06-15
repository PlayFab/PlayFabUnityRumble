namespace PlayFab.Multiplayer.InteropWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class MarshalHelpers
    {
        public delegate int GetContextFunc<InteropHandle>(InteropHandle handle, out IntPtr context);

        public delegate int GetHandlesFun<InputInteropHandle>(InputInteropHandle input, out uint count, out IntPtr handles);

        public static int GetCustomContext<InteropHandle>(
            GetContextFunc<InteropHandle> getContextFunc,
            InteropHandle handle,
            out object customContext)
        {
            customContext = null;
            IntPtr contextPtr;
            int err = getContextFunc(handle, out contextPtr);
            if (LobbyError.SUCCEEDED(err))
            {
                if (contextPtr != IntPtr.Zero)
                {
                    GCHandle contextGcHandle = GCHandle.FromIntPtr(contextPtr);
                    customContext = (object)contextGcHandle.Target;
                }
            }

            return err;
        }

        public static int SetCustomContext<InteropHandle>(
            GetContextFunc<InteropHandle> getContextFunc,
            Func<InteropHandle, IntPtr, int> setContextFunc,
            InteropHandle handle,
            object customContext)
        {
            // Get the current context pointer and release on completion
            IntPtr contextPtrOld;
            int err = getContextFunc(
                handle,
                out contextPtrOld);
            if (LobbyError.SUCCEEDED(err))
            {
                var contextPtr = IntPtr.Zero;
                if (customContext != null)
                {
                    contextPtr = GCHandle.ToIntPtr(GCHandle.Alloc(customContext));
                }

                err = setContextFunc(
                    handle,
                    contextPtr);
                if (LobbyError.SUCCEEDED(err))
                {
                    if (contextPtrOld != IntPtr.Zero)
                    {
                        GCHandle contextGcHandle = GCHandle.FromIntPtr(contextPtrOld);
                        contextGcHandle.Free();
                    }
                }
                else
                {
                    if (contextPtr != IntPtr.Zero)
                    {
                        GCHandle contextGcHandle = GCHandle.FromIntPtr(contextPtr);
                        contextGcHandle.Free();
                    }
                }
            }

            return err;
        }

        public static unsafe int GetArrayOfObjects<InputInteropHandle, IntermediaObject, OutputObject>(
            GetHandlesFun<InputInteropHandle> getHandlesFun,
            Func<IntermediaObject, OutputObject> ctorFun,
            InputInteropHandle inputHandle,
            out OutputObject[] outputHandles)
        {
            outputHandles = null;
            uint handleCount;
            IntPtr handles;
            int err = getHandlesFun(inputHandle, out handleCount, out handles);
            if (LobbyError.SUCCEEDED(err))
            {
                outputHandles = Converters.PtrToClassArray<OutputObject, IntermediaObject>(
                    handles,
                    handleCount,
                    ctorFun);
            }

            return err;
        }
    }
}
