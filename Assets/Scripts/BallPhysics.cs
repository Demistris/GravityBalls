using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{

    public float GravityRange { get; private set; } = 100f;

    [SerializeField] private BallPhysics _splitedBallPrefab = null;
    [SerializeField] private Rigidbody _rigidBody = null;

    private bool _isDestroyed;
    private BallsCreator _ballsCreator;

    private float _speed = 100f;
    private int _maxMass = 50;
    private int _howManyBallsMarged = 1;

    public void Initialize(BallsCreator ballsCreator)
    {
        _ballsCreator = ballsCreator;
    }

    private void Update()
    {
        AddPhysics();
    }

    private void AddPhysics()
    {
        Collider[] otherBalls = Physics.OverlapSphere(transform.position, GravityRange);

        foreach (Collider otherBall in otherBalls)
        {
            Rigidbody otherBallRigidBody = otherBall.attachedRigidbody;
            if (otherBallRigidBody != _rigidBody)
            {
                Vector3 offset = transform.position - otherBall.transform.position;

                if (_ballsCreator.ChangePhysics == false)
                {
                    otherBallRigidBody.AddForce(offset / offset.sqrMagnitude * _rigidBody.mass * _speed);
                }
                else
                {
                    otherBallRigidBody.AddExplosionForce(50f, new Vector3(transform.position.x, transform.position.y, transform.position.z), GravityRange);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (_ballsCreator.ChangePhysics == false)
        {
            if (!collision.gameObject.GetComponent<BallPhysics>()._isDestroyed)
            {
                _isDestroyed = true;
                Destroy(gameObject);
            }
            else
            {
                _rigidBody.mass += collision.rigidbody.mass;
                GravityRange += collision.gameObject.GetComponent<BallPhysics>().GravityRange;
                _howManyBallsMarged += collision.gameObject.GetComponent<BallPhysics>()._howManyBallsMarged;
                gameObject.transform.localScale += collision.transform.localScale;

                CheckIfSplitBall();
            }
        }
    }

    private void CheckIfSplitBall()
    {
        if(_rigidBody.mass >= _maxMass)
        {
            for (int i = 0; i < _howManyBallsMarged; i++)
            {
                var splitedBallPrefab = Instantiate(_splitedBallPrefab, transform.position, transform.rotation, gameObject.transform.parent);
                splitedBallPrefab.Initialize(_ballsCreator);
            }

            _rigidBody.mass = 1;
            Destroy(gameObject);
        }
    }
}
