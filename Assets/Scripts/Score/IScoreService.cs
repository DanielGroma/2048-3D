namespace Game.Scores
{
    public interface IScoreService
    {
        int CurrentScore { get; }
        void AddScore(int amount);
        void Reset();

        event System.Action<int> OnScoreChanged;
    }
}