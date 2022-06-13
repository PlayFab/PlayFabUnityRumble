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

    public class PFLobbyJoinConfiguration
    {
        public PFLobbyJoinConfiguration()
        {
            this.MemberProperties = new Dictionary<string, string>();
        }

        internal unsafe PFLobbyJoinConfiguration(Interop.PFLobbyJoinConfiguration interopStruct)
        {
            string[] memberPropertyKeys = Converters.StringPtrToArray(interopStruct.memberPropertyKeys, interopStruct.memberPropertyCount);
            string[] memberPropertyValues = Converters.StringPtrToArray(interopStruct.memberPropertyValues, interopStruct.memberPropertyCount);
            if (memberPropertyKeys.Length == memberPropertyValues.Length)
            {
                this.MemberProperties = Enumerable.Range(0, memberPropertyKeys.Length).ToDictionary(
                    i => memberPropertyKeys[i],
                    i => memberPropertyValues[i]);
            }
            else
            {
                throw new IndexOutOfRangeException("memberPropertyKeys and memberPropertyValues don't have same length");
            }
        }

        public IDictionary<string, string> MemberProperties { get; set; }

        internal unsafe Interop.PFLobbyJoinConfiguration* ToPointer(DisposableCollection disposableCollection)
        {
            Interop.PFLobbyJoinConfiguration interopPtr = new Interop.PFLobbyJoinConfiguration();

            SizeT count;
            interopPtr.memberPropertyCount = Convert.ToUInt32(this.MemberProperties.Count);
            interopPtr.memberPropertyKeys = (sbyte**)Converters.StringArrayToUTF8StringArray(this.MemberProperties.Keys.ToArray(), disposableCollection, out count);
            interopPtr.memberPropertyValues = (sbyte**)Converters.StringArrayToUTF8StringArray(this.MemberProperties.Values.ToArray(), disposableCollection, out count);

            return (Interop.PFLobbyJoinConfiguration*)Converters.StructToPtr(interopPtr, disposableCollection);
        }
    }
}
