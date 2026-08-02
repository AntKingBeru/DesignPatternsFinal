// Displays the score; observes ScoreManager.ScoreChanged. (View)
using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        ScoreManager.Instance.ScoreChanged += OnScoreChanged;
        OnScoreChanged(ScoreManager.Instance.Score);
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance)
            ScoreManager.Instance.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int score) => scoreText.text = $"Score: {score}";
}