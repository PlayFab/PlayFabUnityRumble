//--------------------------------------------------------------------------------------
// ProfileManager.cs
//
// Base class for all Profile Managers.
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
using UnityEngine.UI;

public class ProfileManager
{
    private static ProfileManager instance;

    public static ProfileManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ProfileManager();
            }
            return instance;
        }
    }

    public virtual ulong GetUserId()
    {
        return 1;
    }

    public virtual string GetProfileName()
    {
        return string.Empty;
    }

    public virtual void ShowUserName(Text userNameText, string userId)
    { 
    }

    public virtual void ShowAvatarImage(RawImage rawImage, string userId)
    { 
    }
}


