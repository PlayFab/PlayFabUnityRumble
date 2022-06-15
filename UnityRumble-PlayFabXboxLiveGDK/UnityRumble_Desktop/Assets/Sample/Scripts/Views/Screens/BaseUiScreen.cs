//--------------------------------------------------------------------------------------
// BaseUiScreen.cs
//
// The base class and functionality for all of the UI screen classes.
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
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseUiScreen : MonoBehaviour
{
    // unity stuff
    public Canvas ViewCanvas;
    public GraphicRaycaster ViewRaycaster;
    public EventSystem InputEventSystem;
    public PopupView PopupView;

    // logic stuff
    public XboxLiveLogic XboxLive;
    public SessionNetwork Networking;

    // UI stuff
    public GameObject StartingSelectedButton;
    public AsynchronousOpUI AsyncOpUI;
    public TMPro.TMP_Text StatusBarText;

    public bool IsShown()
    {
        return ViewCanvas.enabled;
    }

    public bool IsEnabled()
    {
        return ViewRaycaster.enabled;
    }

    public void Show()
    {
        if (ViewCanvas != null)
        {
            ViewCanvas.enabled = true;
        }
        Enable();
        OnShown();
    }

    public void Hide()
    {
        OnHidden();
        Disable();
        if (ViewCanvas != null)
        {
            ViewCanvas.enabled = false;
        }
    }

    public void Enable()
    {
        if (ViewRaycaster != null)
        {
            ViewRaycaster.enabled = true;
        }
        OnEnabled();
    }

    public void Disable()
    {
        OnDisabled();
        if (ViewRaycaster != null)
        {
            ViewRaycaster.enabled = false;
        }
    }

    protected virtual void OnShown()
    {
    }

    protected virtual void OnHidden()
    {
    }

    protected virtual void OnEnabled()
    {
        InputEventSystem.SetSelectedGameObject(StartingSelectedButton, null);
    }

    protected virtual void OnDisabled()
    {
        InputEventSystem.SetSelectedGameObject(null, null);
    }

    protected virtual void Awake()
    {
    }

    private void Start()
    {
        Networking = SessionNetwork.Instance;
    }

    protected virtual void OnValidate()
    {
        ViewCanvas = gameObject.GetComponent<Canvas>();
        Assert.IsNotNull(ViewCanvas);

        ViewRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        Assert.IsNotNull(ViewRaycaster);
        Assert.IsNotNull(ViewRaycaster);
        Assert.IsNotNull(PopupView);
        Assert.IsNotNull(InputEventSystem, "Be sure to assign a EventSystem to all screens");
        Assert.IsNotNull(StatusBarText, "Be sure to assign a StatusBarText to all screens");
    }
}
