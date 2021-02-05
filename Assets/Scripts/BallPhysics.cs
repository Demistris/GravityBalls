using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float GravityRange => _gravityRange;

    private Rigidbody _rb;
    private float _gravityRange = 100f;
    private bool _isDestroyed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        AddPhysics();
    }

    private void AddPhysics()
    {
        Collider[] otherBalls = Physics.OverlapSphere(transform.position, _gravityRange);
        List<Rigidbody> rigidBodyList = new List<Rigidbody>();

        foreach (Collider balls in otherBalls)
        {
            Rigidbody rb = balls.attachedRigidbody;
            if (rb != null && rb != _rb && !rigidBodyList.Contains(rb))
            {
                rigidBodyList.Add(rb);
                Vector3 offset = transform.position - balls.transform.position;

                if (BallsCreator.ChangePhysics == false)
                {
                    rb.AddForce(offset / offset.sqrMagnitude * _rb.mass * 100);
                }
                else
                {
                    rb.AddExplosionForce(10, new Vector3(transform.position.x, transform.position.y, transform.position.z), _gravityRange);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (BallsCreator.ChangePhysics == false)
        {
            if (!collision.gameObject.GetComponent<BallPhysics>()._isDestroyed)
            {
                _isDestroyed = true;
                Destroy(gameObject);
            }
            else
            {
                _rb.mass += collision.rigidbody.mass;
                _gravityRange += collision.gameObject.GetComponent<BallPhysics>().GravityRange;
                gameObject.transform.localScale += collision.transform.localScale;
            }
        }
    }
}
