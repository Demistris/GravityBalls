using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsCreator : MonoBehaviour
{
    public int BallsCounter { get; private set; } = 0;
    public bool ChangePhysics;

    [SerializeField] private BallPhysics _ballPrefab = null;
    [SerializeField] private int _maxBalls = 250;

    private void Start()
    {
        InvokeRepeating("CreateBall", 0f, 0.25f);
    }

    public void CreateBall()
    {
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Random.Range(100f, Camera.main.farClipPlane)));
        var ballPrefab = Instantiate(_ballPrefab, screenPosition, Quaternion.identity, gameObject.transform);
        ballPrefab.Initialize(this);

        BallsCounter += 1;

        CheckIfStopCreatingBalls();
    }

    private void CheckIfStopCreatingBalls()
    {
        if (BallsCounter >= _maxBalls)
        {
            CancelInvoke();
            ChangePhysics = true;
        }
    }
}
