using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Assets.Resources.Scripts.Core;
using UnityEngine.SceneManagement;
using Game.Scores;

namespace Assets.Resources.Scripts.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Text _scoreText;

        private GameStateService _gameStateService;
        private ScoreService _scoreService;

        [Inject]
        public void Construct(GameStateService gameStateService, ScoreService scoreService)
        {
            _gameStateService = gameStateService;
            _scoreService = scoreService;
        }

        private void OnEnable()
        {
            _gameStateService.OnStateChanged += HandleStateChanged;
            _restartButton.onClick.AddListener(RestartGame);

            _gameOverPanel.SetActive(false);
        }

        private void OnDisable()
        {
            _gameStateService.OnStateChanged -= HandleStateChanged;
            _restartButton.onClick.RemoveListener(RestartGame);
        }

        private void HandleStateChanged(GameState state)
        {

            if (state == GameState.GameOver)
            {
                ShowGameOver();
            }
        }

        private void ShowGameOver()
        {
            _gameOverPanel.SetActive(true);
            _scoreText.text = _scoreService.CurrentScore.ToString();
            Time.timeScale = 0f;
        }

        private void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}