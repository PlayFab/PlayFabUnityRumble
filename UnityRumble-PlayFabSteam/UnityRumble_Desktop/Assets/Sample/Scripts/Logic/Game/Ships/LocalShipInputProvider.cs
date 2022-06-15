//--------------------------------------------------------------------------------------
// LocalShipInputProvider.cs
//
// The input provider class that handles input controls for the local player's ship.
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalShipInputProvider : BaseShipInputProvider
{
    public float NetworkUpdateTime = 0.1F;
    public float DeadZone = 0.2f;

    public override void Initialize(SessionNetwork network, ShipController shipController)
    {
        base.Initialize(network, shipController);
    }

    public override bool IsLocal()
    {
        return true;
    }

    public void OnFire(InputValue inputValue)
    {
        FireInput = inputValue.Get<Vector2>();

        if (Math.Abs(FireInput.x) <= DeadZone &&
            Math.Abs(FireInput.y) <= DeadZone)
        {
            FireInput = Vector2.zero;
        }
    }

    public void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>();

        if (Math.Abs(MoveInput.x) <= DeadZone &&
            Math.Abs(MoveInput.y) <= DeadZone)
        {
            MoveInput = Vector2.zero;
        }
    }

    private void Update()
    {
        TimeSinceLastUpdate += Time.deltaTime;

        if (TimeSinceLastUpdate >= NetworkUpdateTime)
        {
            TimeSinceLastUpdate = 0F;
            _network.SendMessageToAll(
                new UpdateShipState(
                    transform.position.x, transform.position.y,
                    _shipController.MyRigidBody.velocity.x, _shipController.MyRigidBody.velocity.y,
                    MoveInput.x, MoveInput.y,
                    FireInput.x, FireInput.y), false);
        }
    }

    private float TimeSinceLastUpdate = 0F;
}
