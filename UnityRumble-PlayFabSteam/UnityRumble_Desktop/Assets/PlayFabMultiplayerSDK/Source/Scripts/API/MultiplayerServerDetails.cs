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
    using System.Collections.Generic;

    public class MultiplayerServerDetails
    {

        internal MultiplayerServerDetails(InteropWrapper.PFMultiplayerServerDetails serverDetails)
        {
            multiplayerServerDetails = serverDetails;
            ports = new List<MultiplayerPort>();
            foreach (var multiplayerPort in serverDetails.Ports)
            {
                MultiplayerPort port = new MultiplayerPort(multiplayerPort);
                ports.Add(port);
            }
        }

        /// <summary>
        /// The fully qualified domain name of the virtual machine that is hosting this multiplayer server.
        /// </summary>
        public string Fqdn
        {
            get
            {
                return multiplayerServerDetails.Fqdn;
            }
        }

        /// <summary>
        /// The IPv4 address of the virtual machine that is hosting this multiplayer server.
        /// </summary>
        public string Ipv4Address
        {
            get
            {
                return multiplayerServerDetails.Ipv4Address;
            }
        }

        /// <summary>
        /// The ports the multiplayer server uses.
        /// </summary>
        public IList<MultiplayerPort> Ports
        {
            get
            {
                return ports;
            }
        }

        /// <summary>
        /// The server's region.
        /// </summary>
        public string Region
        {
            get
            {
                return multiplayerServerDetails.Region;
            }
        }

        internal InteropWrapper.PFMultiplayerServerDetails multiplayerServerDetails;
        private List<MultiplayerPort> ports;
    }
}
