using Assets.Resources.Scripts.Core;
using Assets.Resources.Scripts.Cubes;
using Game.Cubes;
using UnityEngine;
using Zenject;

public class InputService : MonoBehaviour
{
    private CubeSpawner _cubeSpawner;
    private ICubeRepository _cubeRepository;
    private IMergeService _mergeService;
    private CubeLauncher _currentCube;
    private GameStateService _gameStateService;
    private SwipeDetector _swipeDetector;

    private bool _canLaunch = true;
    private float _launchCooldown = 0.5f;

    [Inject]
    public void Construct(
        CubeSpawner cubeSpawner,
        ICubeRepository cubeRepository,
        GameStateService gameStateService,
        IMergeService mergeService,
        SwipeDetector swipeDetector)
    {
        _cubeSpawner = cubeSpawner;
        _cubeRepository = cubeRepository;
        _gameStateService = gameStateService;
        _mergeService = mergeService;
        _swipeDetector = swipeDetector;
    }

    private void OnEnable()
    {
        if (_swipeDetector != null)
            _swipeDetector.OnSwipeEnd += HandleLaunch;
    }

    private void OnDisable()
    {
        if (_swipeDetector != null)
            _swipeDetector.OnSwipeEnd -= HandleLaunch;
    }

    private void HandleLaunch()
    {
        if (!_canLaunch) return;
        if (_currentCube == null) return;

        _canLaunch = false;

        _currentCube.Launch();

        if (_currentCube.IsLaunched)
        {
            _cubeRepository.Register(_currentCube.GetComponent<Cube>());
        }

        Invoke(nameof(SpawnNext), _launchCooldown);
    }

    private void SpawnNext()
    {
        if (_gameStateService.IsState(GameState.GameOver))
            return;

        Cube cube = _cubeSpawner.SpawnRandomCube(_mergeService);

        if (cube == null)
        {
            _canLaunch = true;
            return;
        }

        CubeLauncher launcher = cube.GetComponent<CubeLauncher>();
        if (launcher == null)
        {
            _canLaunch = true;
            return;
        }

        SetActiveCube(launcher);
        _canLaunch = true;
    }

    public void SetActiveCube(CubeLauncher cube)
    {
        if (_currentCube != null)
        {
            _swipeDetector.OnSwipeDelta -= _currentCube.Drag;
            _currentCube.OnDestroyed -= ClearCurrentCubeIfMatches;
        }

        _currentCube = cube;

        if (_currentCube == null)
            return;

        _currentCube.Init();

        _swipeDetector.OnSwipeDelta += _currentCube.Drag;
        _currentCube.OnDestroyed += ClearCurrentCubeIfMatches;
    }

    public void ClearCurrentCubeIfMatches(CubeLauncher cube)
    {
        if (_currentCube == cube)
        {
            if (_swipeDetector != null)
                _swipeDetector.OnSwipeDelta -= _currentCube.Drag;

            _currentCube.OnDestroyed -= ClearCurrentCubeIfMatches;
            _currentCube = null;
        }
    }
}