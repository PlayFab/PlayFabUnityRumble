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

    public class PFMultiplayerServerDetails
    {
        internal PFMultiplayerServerDetails()
        {}

        internal unsafe PFMultiplayerServerDetails(Interop.PFMultiplayerServerDetails* interopStruct)
        {
            Region = Converters.PtrToStringUTF8((IntPtr)interopStruct->region);
            Fqdn = Converters.PtrToStringUTF8((IntPtr)interopStruct->fqdn);
            Ipv4Address = Converters.PtrToStringUTF8((IntPtr)interopStruct->ipv4Address);
            Ports = new PFMultiplayerPort[interopStruct->portCount];
            for (int i = 0; i < interopStruct->portCount; i++)
            {
                Ports[i] = new PFMultiplayerPort(&interopStruct->ports[i]);
            }
        }

        public string Fqdn { get; }

        public string Ipv4Address { get; }

        public PFMultiplayerPort[] Ports { get; }

        public string Region { get; }
    }
}
