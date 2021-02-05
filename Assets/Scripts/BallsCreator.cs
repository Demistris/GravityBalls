using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsCreator : MonoBehaviour
{
    public int BallsCounter => _ballsCounter;
    public static bool ChangePhysics;

    [SerializeField] private GameObject _ballPrefab = null;
    private int _ballsCounter = 0;

    private void Start()
    {
        InvokeRepeating("CreateBall", 0f, 0.25f);
    }

    public void CreateBall()
    {
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Random.Range(100f, Camera.main.farClipPlane)));
        Instantiate(_ballPrefab, screenPosition, Quaternion.identity, gameObject.transform);
        _ballsCounter += 1;

        CheckIfStopCreatingBalls();
    }

    private void CheckIfStopCreatingBalls()
    {
        if (_ballsCounter >= 250)
        {
            CancelInvoke();
            ChangePhysics = true;
        }
    }
}
