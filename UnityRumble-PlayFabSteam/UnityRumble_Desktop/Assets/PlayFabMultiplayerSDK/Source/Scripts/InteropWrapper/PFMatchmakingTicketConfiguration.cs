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

    public class PFMatchmakingTicketConfiguration
    {
        public PFMatchmakingTicketConfiguration()
        {
            this.MembersToMatchWith = new List<PFEntityKey>();
        }

        public PFMatchmakingTicketConfiguration(
            uint timeoutInSeconds,
            string queueName,
            List<PFEntityKey> membersToMatchWith)
        {
            this.TimeoutInSeconds = timeoutInSeconds;
            this.QueueName = queueName;
            this.MembersToMatchWith = membersToMatchWith;
        }

        public uint TimeoutInSeconds { get; set; }

        public string QueueName { get; set; }

        public List<PFEntityKey> MembersToMatchWith { get; set; }

        internal unsafe Interop.PFMatchmakingTicketConfiguration* ToPointer(DisposableCollection disposableCollection)
        {
            Interop.PFMatchmakingTicketConfiguration interopPtr = new Interop.PFMatchmakingTicketConfiguration();

            UTF8StringPtr queueNamePtr = new UTF8StringPtr(this.QueueName, disposableCollection);

            interopPtr.timeoutInSeconds = this.TimeoutInSeconds;
            interopPtr.queueName = queueNamePtr.Pointer;
            interopPtr.membersToMatchWithCount = (uint)this.MembersToMatchWith.Count;
            if (this.MembersToMatchWith.Count > 0)
            {
                Interop.PFEntityKey[] membersToMatchWith = new Interop.PFEntityKey[this.MembersToMatchWith.Count];
                for (int i = 0; i < this.MembersToMatchWith.Count; i++)
                {
                    UTF8StringPtr idPtr = new UTF8StringPtr(this.MembersToMatchWith[i].Id, disposableCollection);
                    UTF8StringPtr typePtr = new UTF8StringPtr(this.MembersToMatchWith[i].Type, disposableCollection);
                    membersToMatchWith[i].id = idPtr.Pointer;
                    membersToMatchWith[i].type = typePtr.Pointer;
                }

                fixed (Interop.PFEntityKey* membersToMatchWithArray = &membersToMatchWith[0])
                {
                    interopPtr.membersToMatchWith = membersToMatchWithArray;
                    return (Interop.PFMatchmakingTicketConfiguration*)Converters.StructToPtr<Interop.PFMatchmakingTicketConfiguration>(interopPtr, disposableCollection);
                }
            }
            else
            {
                interopPtr.membersToMatchWith = null;
                return (Interop.PFMatchmakingTicketConfiguration*)Converters.StructToPtr<Interop.PFMatchmakingTicketConfiguration>(interopPtr, disposableCollection);
            }
        }
    }
}
