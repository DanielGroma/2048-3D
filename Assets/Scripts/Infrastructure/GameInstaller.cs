using Assets.Resources.Scripts.Boosters;
using Assets.Resources.Scripts.Core;
using Game.Cubes;
using Game.Scores;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CubeConfig cubeConfigSO;
    [SerializeField] private AutoMergeConfig autoMergeConfig;
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private Camera _camera;
    [SerializeField] private EventSystem _eventSystem;

    public override void InstallBindings()
    {
        Container.Bind<GameObject>().WithId("CubePrefab").FromInstance(cubePrefab);
        Container.Bind<Transform>().WithId("SpawnPoint").FromInstance(spawnPoint);

        Container.BindInterfacesAndSelfTo<MergeService>().AsSingle();
        Container.BindInterfacesAndSelfTo<AutoMergeService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle();
        Container.BindInterfacesAndSelfTo<CubeRepository>().AsSingle();

        Container.Bind<EventSystem>().FromInstance(_eventSystem).AsSingle();
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        Container.Bind<GraphicRaycaster>().FromInstance(_raycaster).AsSingle();
        Container.Bind<AutoMergeConfig>().FromInstance(autoMergeConfig).AsSingle();
        Container.Bind<CubeConfig>().FromInstance(cubeConfigSO).AsSingle();

        Container.Bind<SwipeDetector>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SoundSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputService>().FromComponentInHierarchy().AsSingle();

        Container.Bind<GameStateService>().AsSingle();
        Container.Bind<CubeSpawner>().AsSingle();
        Container.Bind<CubeFactory>().AsSingle();
    }
}