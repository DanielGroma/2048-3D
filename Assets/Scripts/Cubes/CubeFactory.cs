using Assets.Resources.Scripts.Core;
using Assets.Resources.Scripts.Cubes;
using System.Linq;
using UnityEngine;
using Zenject;

public class CubeFactory
{
    private readonly GameObject _cubePrefab;
    private readonly CubeConfig _cubeConfig;
    private readonly GameStateService _gameStateService;
    private readonly DiContainer _container;

    [Inject]
    public CubeFactory(
        [Inject(Id = "CubePrefab")] GameObject cubePrefab,
        CubeConfig cubeConfig,
        GameStateService gameStateService,
        DiContainer container)
    {
        _cubePrefab = cubePrefab;
        _cubeConfig = cubeConfig;
        _gameStateService = gameStateService;
        _container = container;
    }

    public Cube CreateCube(CubeValue cubeValue, Vector3 position, IMergeService mergeService)
    {
        var go = _container.InstantiatePrefab(_cubePrefab, position, Quaternion.identity, null);

        var cube = go.GetComponent<Cube>();
        var launcher = go.GetComponent<CubeLauncher>();
        var collision = go.GetComponent<CubeCollision>();

        launcher.Init();
        collision.Init(mergeService, _gameStateService);

        var data = _cubeConfig.cubes.FirstOrDefault(c => c.value == cubeValue.Value);
        Color color = data != null ? data.color : Color.white;
        cube.SetValue(cubeValue, color);

        return cube;
    }
}