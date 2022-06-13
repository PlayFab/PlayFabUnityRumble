//--------------------------------------------------------------------------------------
// AchievementManager.cs
//
// The Manager class for Achievements.
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

public class AchievementManager : ManagerBaseClass<AchievementManager>
{
    public string[] AchievementNames;

    public readonly string STAT1 = "stat_1";

    public readonly string STAT2 = "stat_2";

    public readonly string STAT3 = "stat_3";

    public readonly string ACHFIRSTDEATH = "ACH_FIRST_DEATH";

    public virtual void Initialize()
    {
    }

    public virtual void RequestAllAchievement()
    {
    }

    public virtual void SetAchievement(string achievementName)
    {
    }

    public virtual void SetStat(string statName, int addStatNumber)
    {
    }

    public virtual bool GetAchievement(string achievementName)
    {
        return false;
    }

    public virtual int GetStat(string statName)
    {
        return -1;
    }
}
