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
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using PlayFab.Multiplayer.Interop;

    public class PFMatchmakingStateChange
    {
        protected unsafe PFMatchmakingStateChange(PFMatchmakingStateChangeType stateChangeType, Interop.PFMatchmakingStateChange* stateChangeId)
        {
            this.StateChangeType = stateChangeType;
            this.StateChangeId = stateChangeId;
            this.UseObjectPool = false;
        }

        public PFMatchmakingStateChangeType StateChangeType { get; private set; }

        internal unsafe Interop.PFMatchmakingStateChange* StateChangeId { get; private set; }

        protected bool UseObjectPool { get; set; }

        internal static unsafe PFMatchmakingStateChange CreateFromPtr(Interop.PFMatchmakingStateChange* stateChangePtr)
        {
            PFMatchmakingStateChange result = null;
            PFMatchmakingStateChangeUnion stateChangeUnion = (PFMatchmakingStateChangeUnion)Marshal.PtrToStructure(new IntPtr(stateChangePtr), typeof(PFMatchmakingStateChangeUnion));
            PFMatchmakingStateChangeType wrapperChangeType = (PFMatchmakingStateChangeType)stateChangeUnion.stateChange.stateChangeType;
            switch (wrapperChangeType)
            {
                case PFMatchmakingStateChangeType.TicketStatusChanged:
                    result = new PFMatchmakingTicketStatusChangedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                case PFMatchmakingStateChangeType.TicketCompleted:
                    result = new PFMatchmakingTicketCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                default:
                    Debug.Write(string.Format("Unhandle type {0}\n", stateChangeUnion.stateChange.stateChangeType));
                    Debugger.Break();
                    result = new PFMatchmakingStateChange(wrapperChangeType, stateChangePtr);
                    break;
            }

            return result;
        }

        internal virtual void Cleanup()
        {
            if (this.UseObjectPool)
            {
                PFMultiplayer.ObjPool.Return(this);
            }
        }
    }

    public class PFMatchmakingTicketCompletedStateChange : PFMatchmakingStateChange
    {
        internal unsafe PFMatchmakingTicketCompletedStateChange(
            PFMatchmakingStateChangeUnion stateChangeUnion, Interop.PFMatchmakingStateChange* stateChangeId) : 
            base((PFMatchmakingStateChangeType)stateChangeUnion.stateChange.stateChangeType, stateChangeId)
        {
            unsafe
            {
                Interop.PFMatchmakingTicketCompletedStateChange stateChangeConverted = stateChangeUnion.ticketCompleted;

                this.Result = stateChangeConverted.result;
                this.Ticket = new PFMatchmakingTicketHandle(stateChangeConverted.ticket);
                this.AsyncContext = null;
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.AsyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
            }
        }

        public int Result { get; set; }

        public PFMatchmakingTicketHandle Ticket { get; set; }

        public object AsyncContext { get; set; }
    }

    public class PFMatchmakingTicketStatusChangedStateChange : PFMatchmakingStateChange
    {
        internal unsafe PFMatchmakingTicketStatusChangedStateChange(
            PFMatchmakingStateChangeUnion stateChangeUnion, Interop.PFMatchmakingStateChange* stateChangeId) : 
            base((PFMatchmakingStateChangeType)stateChangeUnion.stateChange.stateChangeType, stateChangeId)
        {
            unsafe
            {
                Interop.PFMatchmakingTicketStatusChangedStateChange stateChangeConverted = stateChangeUnion.ticketStatusChanged;
                this.Ticket = new PFMatchmakingTicketHandle(stateChangeConverted.ticket);
            }
        }

        public PFMatchmakingTicketHandle Ticket { get; set; }
    }
}
