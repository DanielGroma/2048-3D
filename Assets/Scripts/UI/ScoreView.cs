using TMPro;
using UnityEngine;
using Zenject;
using Game.Scores;
using DG.Tweening;

namespace Assets.Resources.Scripts.UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private IScoreService _scoreService;

        [Inject]
        public void Construct(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        private void OnEnable()
        {
            _scoreService.OnScoreChanged += UpdateScore;
            UpdateScore(_scoreService.CurrentScore);
        }

        private void OnDisable()
        {
            _scoreService.OnScoreChanged -= UpdateScore;
        }

        private void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
            var sequence = DOTween.Sequence();
            sequence.Append(_scoreText.rectTransform.DOScale(1.5f, 0.2f))
                .Append(_scoreText.rectTransform.DOScale(1.2f, 0.2f));
        }
    }
}