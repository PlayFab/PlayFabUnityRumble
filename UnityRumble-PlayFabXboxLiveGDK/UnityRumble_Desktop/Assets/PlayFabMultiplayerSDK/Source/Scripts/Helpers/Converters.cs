namespace PlayFab.Multiplayer.InteropWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class Converters
    {
        public static ClassType PtrToStruct<ClassType>(IntPtr ptr)
        {
            return (ClassType)Marshal.PtrToStructure(ptr, typeof(ClassType));
        }

        public static IntPtr ClassArrayToPtr<ClassType, InteropStructType>(
            ClassType[] inputTypes,
            Func<ClassType, DisposableCollection, InteropStructType> converter,
            DisposableCollection disposableCollection,
            out uint arrayCount)
        {
            SizeT count;
            IntPtr result = ClassArrayToPtr<ClassType, InteropStructType>(inputTypes, converter, disposableCollection, out count);
            arrayCount = count.ToUInt32();
            return result;
        }

        public static IntPtr StructToPtr<InteropStructType>(
            InteropStructType interopStruct,
            DisposableCollection disposableCollection)
        {
            bool isEnum = typeof(InteropStructType).IsEnum;
            int sizeOfStruct = Marshal.SizeOf(isEnum ? Enum.GetUnderlyingType(typeof(InteropStructType)) : typeof(InteropStructType));
            DisposableBuffer buffer = disposableCollection.Add(new DisposableBuffer(checked(sizeOfStruct)));

            IntPtr currentPtr = buffer.IntPtr;
            object structure = isEnum ? 
                Convert.ChangeType(interopStruct, Enum.GetUnderlyingType(typeof(InteropStructType))) : 
                (object)interopStruct;
            Marshal.StructureToPtr(structure, buffer.IntPtr, fDeleteOld: false);
            return currentPtr;
        }

        public static IntPtr ClassArrayToPtr<ClassType, InteropStructType>(
            ClassType[] inputTypes,
            Func<ClassType, DisposableCollection, InteropStructType> converter,
            DisposableCollection disposableCollection,
            out SizeT arrayCount)
        {
            if (inputTypes == null)
            {
                arrayCount = new SizeT(0);
                return IntPtr.Zero;
            }

            bool isEnum = typeof(InteropStructType).IsEnum;
            int sizeOfStruct = Marshal.SizeOf(isEnum ? Enum.GetUnderlyingType(typeof(InteropStructType)) : typeof(InteropStructType));
            DisposableBuffer buffer = disposableCollection.Add(new DisposableBuffer(checked(sizeOfStruct * inputTypes.Length)));

            IntPtr currentPtr = buffer.IntPtr;
            foreach (ClassType inputType in inputTypes)
            {
                object structure = isEnum ? Convert.ChangeType(converter(inputType, disposableCollection), Enum.GetUnderlyingType(typeof(InteropStructType))) : (object)converter(inputType, disposableCollection);
                Marshal.StructureToPtr(structure, currentPtr, fDeleteOld: false);
                currentPtr = currentPtr.Offset(sizeOfStruct);
            }

            arrayCount = new SizeT(inputTypes.Length);
            return buffer.IntPtr;
        }

        public static InteropStructType[] ConvertArrayToFixedLength<ClassType, InteropStructType>(ClassType[] classes, int length, Func<ClassType, InteropStructType> ctor)
        {
            InteropStructType[] result = new InteropStructType[length];
            int minLength = Math.Min(length, classes.Length);
            for (int i = 0; i < minLength; ++i)
            {
                result[i] = ctor(classes[i]);
            }

            return result;
        }

        internal static unsafe sbyte*[] StringArrayToPtr(string[] arr, DisposableCollection dc)
        {
            sbyte*[] rawPtrArray = new sbyte*[arr.Length];

            int index = 0;
            foreach (var s in arr)
            {
                rawPtrArray[index] = new UTF8StringPtr(s, dc).Pointer; // DisposableCollection manages lifetime of pointer
                index++;
            }

            return rawPtrArray;
        }

        internal static unsafe string PtrToStringUTF8(sbyte* rawPtr)
        {
            return PtrToStringUTF8((IntPtr)rawPtr);
        }

        internal static unsafe string[] StringPtrToArray(sbyte** rawPtr, uint count)
        {
            string[] ret = new string[count];
            for (int i = 0; i < count; i++)
            {
                ret[i] = PtrToStringUTF8((IntPtr)rawPtr[i]);
            }

            return ret;
        }

        internal static IntPtr Offset(this IntPtr ptr, long that)
        {
            return new IntPtr(ptr.ToInt64() + that);
        }

        internal static DisposableBuffer StringArrayToUTF8StringArray(string[] strings)
        {
            if (strings == null)
            {
                return new DisposableBuffer();
            }

            List<byte[]> byteArrays = new List<byte[]>(strings.Length);
            int size = 0;
            foreach (string str in strings)
            {
                byte[] bytes = StringToNullTerminatedUTF8ByteArray(str);
                byteArrays.Add(bytes);
                if (bytes != null)
                {
                    size = checked(size + bytes.Length);
                }
            }

            int pointerSize = Marshal.SizeOf(typeof(IntPtr));
            int allPointerSizes = checked(pointerSize * strings.Length);
            size = checked(size + allPointerSizes);
            DisposableBuffer result = new DisposableBuffer(size);

            IntPtr currentPointerPointer = result.IntPtr;
            IntPtr currentBytePointer = currentPointerPointer.Offset(allPointerSizes);

            foreach (byte[] bytes in byteArrays)
            {
                if (bytes != null)
                {
                    Marshal.WriteIntPtr(ptr: currentPointerPointer, val: currentBytePointer);
                    Marshal.Copy(bytes, 0, destination: currentBytePointer, length: bytes.Length);
                    currentBytePointer = currentBytePointer.Offset(bytes.Length);
                }
                else
                {
                    Marshal.WriteIntPtr(ptr: currentPointerPointer, val: IntPtr.Zero);
                }

                currentPointerPointer = currentPointerPointer.Offset(pointerSize);
            }

            return result;
        }

        internal static IntPtr StringArrayToUTF8StringArray(string[] strings, DisposableCollection disposableCollection, out SizeT count)
        {
            if (strings == null)
            {
                count = new SizeT(0);
                return IntPtr.Zero;
            }

            count = new SizeT(strings.Length);
            return disposableCollection.Add(StringArrayToUTF8StringArray(strings)).IntPtr;
        }

        internal static byte[] StringToNullTerminatedUTF8ByteArray(string str) 
        { 
            return StringToNullTerminatedUTF8ByteArrayInternal(str, requiredByteArrayLength: -1); 
        }

        internal static byte[] StringToNullTerminatedUTF8ByteArray(string str, int requiredByteArrayLength) 
        { 
            return StringToNullTerminatedUTF8ByteArrayInternal(str, requiredByteArrayLength); 
        }

        internal static unsafe void StringToNullTerminatedUTF8FixedPointer(string str, byte* bytePointer, int length)
        {
            byte[] bytes = StringToNullTerminatedUTF8ByteArray(str, length);
            Marshal.Copy(source: bytes, startIndex: 0, destination: (IntPtr)bytePointer, length: length);
        }

        internal static unsafe string BytePointerToString(byte* bytePointer, int length)
        {
            byte[] bytes = new byte[length];
            Marshal.Copy(source: (IntPtr)bytePointer, destination: bytes, startIndex: 0, length: length);
            return ByteArrayToString(bytes);
        }

        internal static string ByteArrayToString(byte[] arr)
        {
            string str = System.Text.Encoding.UTF8.GetString(arr);
            int nullIndex = str.IndexOf('\0');
            return nullIndex >= 0 ? str.Substring(0, nullIndex) : str;
        }

        internal static string ByteArrayToString(byte[] arr, int index, int count)
        {
            return System.Text.Encoding.UTF8.GetString(arr, index, count).TrimEnd(new char[] { '\0' });
        }

        internal static string PtrToStringUTF8(IntPtr rawPtr)
        {
            if (rawPtr == IntPtr.Zero)
            {
                return null;
            }

            List<byte> bytes = new List<byte>();
            while (true)
            {
                byte b = Marshal.ReadByte(rawPtr);
                if (b == 0)
                {
                    break;
                }

                bytes.Add(b);
                rawPtr = rawPtr.Offset(1);
            }

            return System.Text.Encoding.UTF8.GetString(bytes.ToArray());
        }

        internal static ClassType PtrToClass<ClassType, InteropStructType>(IntPtr rawPtr, Func<InteropStructType, ClassType> ctor)
            where ClassType : class
            where InteropStructType : struct
        {
            if (rawPtr == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                return ctor((InteropStructType)Marshal.PtrToStructure(rawPtr, typeof(InteropStructType)));
            }
        }

        internal static ClassType[] PtrToClassArray<ClassType, InteropStructType>(IntPtr rawPtr, SizeT count, Func<InteropStructType, ClassType> ctor)
        {
            return PtrToClassArray(rawPtr, count.ToUInt32(), ctor);
        }

        internal static ClassType[] PtrToClassArray<ClassType, InteropStructType>(IntPtr rawPtr, uint count, Func<InteropStructType, ClassType> ctor)
        {
            ClassType[] ret = new ClassType[(int)count];

            if (IntPtr.Zero != rawPtr)
            {
                IntPtr arrayPtr = rawPtr;
                int structSize = Marshal.SizeOf(typeof(InteropStructType));
                for (int i = 0; i < count; i++)
                {
                    InteropStructType u = (InteropStructType)Marshal.PtrToStructure(arrayPtr.Offset(i * structSize), typeof(InteropStructType));
                    ret[i] = ctor(u);
                }
            }

            return ret;
        }

        private static byte[] StringToNullTerminatedUTF8ByteArrayInternal(string str, int requiredByteArrayLength)
        {
            if (str == null)
            {
                return null;
            }
            else if (requiredByteArrayLength == -1)
            {
                return System.Text.Encoding.UTF8.GetBytes(str + '\0');
            }
            else
            {
                byte[] result = new byte[requiredByteArrayLength];
                System.Text.Encoding.UTF8.GetBytes(str + '\0', 0, str.Length + 1, result, 0);
                return result;
            }
        }
    }
}
