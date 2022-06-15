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

    public class PFLobbyConsts
    {
        public const uint MaxLobbyMemberCountLowerLimit = 2;
        public const uint MaxLobbyMemberCountUpperLimit = 128;
        public const uint MaxLobbyPropertyCount = 30;
        public const uint MaxMemberPropertyCount = 30;

#if LOBBY_MOCK
        public const int S_MOCK_TEST_OK = 0x00001234;
        public const UInt64 MOCK_TEST_HANDLE = 0x1234567890123456;
        public const uint MOCK_TEST_UINT_PARAM1 = 0x12345678;
        public const uint MOCK_TEST_UINT_PARAM2 = 0x87654321;
        public const UInt64 MOCK_TEST_UINT64_PARAM1 = 0x1234567812345678;
        public const string MOCK_TEST_STRING_PARAM1 = "Test123";
        public const string MOCK_TEST_STRING_PARAM2 = "234Test";
        public const string MOCK_TEST_STRING_PARAM3 = "Test567";
        public const string MOCK_TEST_STRING_PARAM4 = "567Test";
        public const string MOCK_TEST_ENTITY_ID = "MockEntityId";
        public const string MOCK_TEST_ENTITY_TYPE = "MockEntityType";
#endif
    }
}
