//--------------------------------------------------------------------------------------
// AsteroidController.cs
//
// The game behavior class for handling the asteroid game object behaviors.
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

public class AsteroidController : BaseObjectController
{
    public float NetworkUpdateTime = 0.2F;
    public float CorrectionDistanceThreshold = 0.5f;
    public float CorrectionTime = 1.0F;
    public float NominalVelocity = 1.0F;
    public float NominalAngularVelocity = 90F;
    public Rigidbody2D MyRigidBody;

    public bool IsHost { private get; set; }

    public void Initialize(SessionNetwork network, int id, bool isHost)
    {
        _network = network;
        _id = id;

        IsHost = isHost;

        _network.OnNetworkMessage_UpdateAsteroidState_Received += HandleAsteroidStateReceived;

        // angular velocity does not matter so just start off with some random
        // amount of rotational motion
        MyRigidBody.angularVelocity = Random.value * NominalAngularVelocity;

        // start us off with some small initial velocity if we are the host

        if (IsHost)
        {
            MyRigidBody.velocity = new Vector2(
                (Random.value - 0.5F) * NominalVelocity,
                (Random.value - 0.5F) * NominalVelocity);
        }
    }

    private void OnValidate()
    {
        Assert.IsNotNull(MyRigidBody);
    }

    private void Update()
    {
        if (!IsHost)
        {
            return;
        }

        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate >= NetworkUpdateTime)
        {
            _timeSinceLastUpdate = 0F;
            _network.SendMessageToAll(
                new UpdateAsteroidState(
                    _id,
                    transform.position.x, transform.position.y,
                    MyRigidBody.velocity.x, MyRigidBody.velocity.y));
        }
    }

    private void OnDestroy()
    {
        _network.OnNetworkMessage_UpdateAsteroidState_Received -= HandleAsteroidStateReceived;
    }

    private void HandleAsteroidStateReceived(ulong senderXuid, UpdateAsteroidState asteroidState)
    {
        if (asteroidState.Id == _id)
        {
            _latestAsteroidState = asteroidState;

            var positionalDifference = new Vector3(
                _latestAsteroidState.PosX - transform.position.x,
                _latestAsteroidState.PosY - transform.position.y);

            // correct the asteroid?

            if (positionalDifference.magnitude >= CorrectionDistanceThreshold)
            {
                var velocityDifference = new Vector2(
                    _latestAsteroidState.VelX - MyRigidBody.velocity.x,
                    _latestAsteroidState.VelY - MyRigidBody.velocity.y);

                var correctionPercentage = Time.fixedDeltaTime / CorrectionTime;
                transform.position = transform.position + (positionalDifference * correctionPercentage);
                MyRigidBody.velocity = MyRigidBody.velocity + (velocityDifference * correctionPercentage);
            }
        }
    }

    private float _timeSinceLastUpdate = 0F;
    protected SessionNetwork _network;
    protected int _id;
    private UpdateAsteroidState _latestAsteroidState;
}
