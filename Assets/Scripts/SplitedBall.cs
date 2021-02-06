using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitedBall : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private SphereCollider _collider = null;
    [SerializeField] private float _speed = 0f;

    private void Start()
    {
        _rigidBody.AddRelativeForce(Random.onUnitSphere * _speed, ForceMode.Impulse);
        StartCoroutine(ColliderActivator());
    }

    IEnumerator ColliderActivator()
    {
        yield return new WaitForSeconds(0.5f);

        _collider.enabled = true;
    }
}
