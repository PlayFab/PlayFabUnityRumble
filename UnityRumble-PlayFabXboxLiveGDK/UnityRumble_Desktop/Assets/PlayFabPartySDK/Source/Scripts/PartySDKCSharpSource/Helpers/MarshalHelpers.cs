using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK
{
    public static class MarshalHelpers
    {
        public delegate UInt32 GetContextFunc<InteropHandle>(InteropHandle handle, out IntPtr context);
        public static UInt32 GetCustomContext<InteropHandle>(
            GetContextFunc<InteropHandle> getContextFunc,
            InteropHandle handle,
            out Object customContext)
        {
            customContext = null;
            IntPtr contextPtr;
            UInt32 err = getContextFunc(handle, out contextPtr);
            if (PartyError.SUCCEEDED(err))
            {
                if (contextPtr != IntPtr.Zero)
                {
                    GCHandle contextGcHandle = GCHandle.FromIntPtr(contextPtr);
                    customContext = (Object)contextGcHandle.Target;
                }
            }

            return err;
        }

        public static UInt32 SetCustomContext<InteropHandle>(
            GetContextFunc<InteropHandle> getContextFunc,
            Func<InteropHandle, IntPtr, UInt32> setContextFunc,
            InteropHandle handle,
            Object customContext)
        {
            // Get the current context pointer and release on completion
            IntPtr contextPtrOld;
            UInt32 err = getContextFunc(
                handle,
                out contextPtrOld);
            if (PartyError.SUCCEEDED(err))
            {
                var contextPtr = IntPtr.Zero;
                if (customContext != null)
                {
                    contextPtr = GCHandle.ToIntPtr(GCHandle.Alloc(customContext));
                }

                err = setContextFunc(
                    handle,
                    contextPtr);
                if (PartyError.SUCCEEDED(err))
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

        public delegate UInt32 GetHandlesFun<InputInteropHandle>(InputInteropHandle input, out UInt32 count, out IntPtr handles);
        public static UInt32 GetArrayOfObjects<InputInteropHandle, IntermediaObject, OutputObject>(
            GetHandlesFun<InputInteropHandle> getHandlesFun,
            Func<IntermediaObject, OutputObject> ctorFun,
            InputInteropHandle inputHandle,
            out OutputObject[] outputHandles)
        {
            outputHandles = null;
            UInt32 handleCount;
            IntPtr handles;
            UInt32 err = getHandlesFun(inputHandle, out handleCount, out handles);
            if (PartyError.SUCCEEDED(err))
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
