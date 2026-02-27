using UnityEngine;
using Zenject;
using Assets.Resources.Scripts.Cubes;

public class Bootstrap : MonoBehaviour
{
    [Inject] private CubeSpawner _cubeSpawner;
    [Inject] private InputService _inputService;
    [Inject] private IMergeService _mergeService;

    void Start()
    {
        Cube cube = _cubeSpawner.SpawnRandomCube(_mergeService);
        var launcher = cube.GetComponent<CubeLauncher>();

        _inputService.SetActiveCube(launcher);
    }
}