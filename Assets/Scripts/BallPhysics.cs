using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float GravityRange { get; private set; } = 100f;
    
    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _explosionForce = 15f;
    [SerializeField] private int _maxMass = 50;

    private BallsCreator _ballsCreator;
    private bool _isDestroyed;
    private int _howManyBallsMarged = 1;

    public void Initialize(BallsCreator ballsCreator)
    {
        _ballsCreator = ballsCreator;
    }

    private void FixedUpdate()
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
                    otherBallRigidBody.AddExplosionForce(_explosionForce, new Vector3(transform.position.x, transform.position.y, transform.position.z), GravityRange);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BallPhysics biggerBall = this;
        BallPhysics otherBall = collision.gameObject.GetComponent<BallPhysics>();

        if(otherBall._rigidBody.mass > biggerBall._rigidBody.mass)
        {
            BallPhysics temp = otherBall;
            otherBall = biggerBall;
            biggerBall = temp;
        }

        HandleCollistion(biggerBall, otherBall, _ballsCreator);
        biggerBall.CheckIfSplitBall();
    }

    private static void HandleCollistion(BallPhysics biggerBall, BallPhysics otherBall, BallsCreator ballsCreator)
    {
        if (biggerBall._isDestroyed || otherBall._isDestroyed || ballsCreator.ChangePhysics)
        {
            return;
        }

        biggerBall._rigidBody.mass += otherBall._rigidBody.mass;
        biggerBall.GravityRange += otherBall.GravityRange;
        biggerBall._howManyBallsMarged += otherBall._howManyBallsMarged;
        biggerBall.gameObject.transform.localScale += otherBall.transform.localScale;;

        otherBall._isDestroyed = true;
        Destroy(otherBall.gameObject);
    }

    private void CheckIfSplitBall()
    {
        if(_isDestroyed)
        {
            return;
        }

        if(_rigidBody.mass >= _maxMass)
        {
            Destroy(gameObject);
            _isDestroyed = true;

            for (int i = 0; i < _howManyBallsMarged; i++)
            {
                var splittedBallPrefab = Instantiate(_ballsCreator.SplittedBallPrefab, transform.position, transform.rotation, gameObject.transform.parent);
                splittedBallPrefab.Initialize(_ballsCreator);
            }
        }
    }
}
