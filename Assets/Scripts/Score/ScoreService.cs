using System;

namespace Game.Scores
{
    public class ScoreService : IScoreService
    {
        public int CurrentScore { get; private set; }

        public event Action<int> OnScoreChanged;

        public void AddScore(int amount)
        {
            if (amount <= 0)
                return;

            CurrentScore += amount;
            OnScoreChanged?.Invoke(CurrentScore);
        }

        public void Reset()
        {
            CurrentScore = 0;
            OnScoreChanged?.Invoke(CurrentScore);
        }
    }
}