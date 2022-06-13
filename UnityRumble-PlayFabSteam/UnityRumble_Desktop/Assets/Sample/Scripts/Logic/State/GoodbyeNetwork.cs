﻿//--------------------------------------------------------------------------------------
// GoodbyeNetwork.cs
//
// The reliable network message that is broadcasted when a player has left the network.
//
// MIT License
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// 
// Copyright (C) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GoodbyeNetwork : BaseNetStateObject
{
    public ulong MyXuid { get; private set; }

    public GoodbyeNetwork() : this(0) { }

    public GoodbyeNetwork(ulong xuid) :
        base(SessionMessageType.GoodbyeNetwork, StateType.Reliable)
    {
        MyXuid = xuid;
    }

    protected override void DeserializeFrom(BinaryReader reader)
    {
        MyXuid = reader.ReadUInt64();
    }

    protected override void SerializeTo(BinaryWriter writer)
    {
        writer.Write(MyXuid);
    }
}
