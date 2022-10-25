/*
 * PlayFab Unity SDK
 *
 * Copyright (c) Microsoft Corporation
 *
 * MIT License
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 * to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

namespace PlayFab.Multiplayer.InteropWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PlayFab.Multiplayer.Interop;

    public class PFEntityKey
    {
        public PFEntityKey(string id, string type)
        {
            this.Id = id;
            this.Type = type;
        }

        internal unsafe PFEntityKey(Interop.PFEntityKey* interopStruct)
        {
            unsafe
            {
                // interopStruct may be null from this call: LeaveAllLocalUsers()
                this.Id = interopStruct == null ? this.Id : Converters.PtrToStringUTF8((IntPtr)interopStruct->id);
                this.Type = interopStruct == null ? this.Type : Converters.PtrToStringUTF8((IntPtr)interopStruct->type);
            }
        }

        public string Id { get; set; }

        public string Type { get; set; }

        internal unsafe Interop.PFEntityKey* ToPointer(DisposableCollection disposableCollection)
        {
            unsafe
            {
                UTF8StringPtr idPtr = new UTF8StringPtr(this.Id, disposableCollection);
                UTF8StringPtr typePtr = new UTF8StringPtr(this.Type, disposableCollection);

                Interop.PFEntityKey entityKey = new Interop.PFEntityKey();
                entityKey.id = idPtr.Pointer;
                entityKey.type = typePtr.Pointer;
                return (Interop.PFEntityKey*)Converters.StructToPtr<Interop.PFEntityKey>(entityKey, disposableCollection);
            }
        }
    }
}
