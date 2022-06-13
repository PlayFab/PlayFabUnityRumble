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
    public class PFLobbySearchConfiguration
    {
        public PFLobbySearchFriendsFilter FriendsFilter { get; set; }

        public string FilterString { get; set; }

        public string SortString { get; set; }

        public uint? ClientSearchResultCount { get; set; }

        internal unsafe Interop.PFLobbySearchConfiguration* ToPointer(DisposableCollection disposableCollection)
        {
            Interop.PFLobbySearchConfiguration interopPtr = new Interop.PFLobbySearchConfiguration();

            if (this.FriendsFilter != null)
            {
                interopPtr.friendsFilter = this.FriendsFilter.ToPointer(disposableCollection);
            }
            else
            {
                interopPtr.friendsFilter = null;
            }

            if (!string.IsNullOrEmpty(this.FilterString))
            {
                UTF8StringPtr filterStringPtr = new UTF8StringPtr(this.FilterString, disposableCollection);
                interopPtr.filterString = filterStringPtr.Pointer;
            }
            else
            {
                interopPtr.filterString = null;
            }

            if (!string.IsNullOrEmpty(this.SortString))
            {
                UTF8StringPtr sortStringPtr = new UTF8StringPtr(this.SortString, disposableCollection);
                interopPtr.sortString = sortStringPtr.Pointer;
            }
            else
            {
                interopPtr.sortString = null;
            }

            if (this.ClientSearchResultCount.HasValue)
            {
                interopPtr.clientSearchResultCount = (uint*)Converters.StructToPtr<uint>(this.ClientSearchResultCount.Value, disposableCollection);
            }
            else
            {
                interopPtr.clientSearchResultCount = null;
            }            

            return (Interop.PFLobbySearchConfiguration*)Converters.StructToPtr<Interop.PFLobbySearchConfiguration>(interopPtr, disposableCollection);
        }
    }
}
