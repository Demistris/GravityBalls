using UnityEngine;
using TMPro;

public class BallsCounter : MonoBehaviour
{
    [SerializeField] private BallsCreator _ballsCreator = null;
    [SerializeField] private TextMeshProUGUI _ballsCounterText = null;

    private void Update()
    {
        UpdateBallsCountText();
    }

    private void UpdateBallsCountText()
    {
        _ballsCounterText.text = _ballsCreator.BallsCounter.ToString();
    }
}
