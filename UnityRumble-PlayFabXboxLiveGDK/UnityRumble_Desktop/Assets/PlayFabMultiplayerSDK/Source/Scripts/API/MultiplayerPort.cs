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

namespace PlayFab.Multiplayer
{
    using System;

    public class MultiplayerPort
    {

        internal MultiplayerPort(InteropWrapper.PFMultiplayerPort port)
        {
            multiplayerPort = port;
            protocolType = (MultiplayerProtocolType)port.Protocol;
        }

        /// <summary>
        /// The name of the port as specified in the PlayFab Game Manager server settings.
        /// </summary>
        public string Name
        {
            get
            {
                return multiplayerPort.Name;
            }
        }

        /// <summary>
        /// The number for the port.
        /// </summary>
        public uint Num
        {
            get
            {
                return multiplayerPort.Num;
            }
        }

        /// <summary>
        /// The protocol for the port.
        /// </summary>
        public MultiplayerProtocolType Protocol
        {
            get
            {
                return protocolType;
            }
        }

        internal InteropWrapper.PFMultiplayerPort multiplayerPort;
        private MultiplayerProtocolType protocolType;
    }
}
