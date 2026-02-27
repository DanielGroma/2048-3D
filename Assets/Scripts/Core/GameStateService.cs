using System;

namespace Assets.Resources.Scripts.Core
{
    public class GameStateService
    {
        public event Action<GameState> OnStateChanged;

        private GameState _currentState = GameState.Playing;
        public GameState CurrentState => _currentState;

        public void SetState(GameState newState)
        {
            if (_currentState == newState) return;

            _currentState = newState;
            OnStateChanged?.Invoke(_currentState);
        }

        public bool IsState(GameState state) => _currentState == state;
    }
}
