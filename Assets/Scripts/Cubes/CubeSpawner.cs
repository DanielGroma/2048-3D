using Assets.Resources.Scripts.Cubes;
using UnityEngine;
using Zenject;

public class CubeSpawner
{
    private readonly CubeFactory _factory;
    private readonly Transform _spawnPoint;

    [Inject]
    public CubeSpawner(
        CubeFactory factory,
        [Inject(Id = "SpawnPoint")] Transform spawnPoint)
    {
        _factory = factory;
        _spawnPoint = spawnPoint;
    }

    public Cube SpawnRandomCube(IMergeService mergeService)
    {
        int rawValue = Random.value < 0.75f ? 2 : 4;

        CubeValue cubeValue = new CubeValue(rawValue);

        return _factory.CreateCube(
            cubeValue,
            _spawnPoint.position,
            mergeService);
    }
}