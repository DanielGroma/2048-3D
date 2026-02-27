using Assets.Resources.Scripts.Cubes;
using DG.Tweening;
using Game.Cubes;
using Game.Scores;
using UnityEngine;

public class MergeService : IMergeService
{
    private readonly CubeFactory _cubeFactory;
    private readonly MergeValidator _validator;
    private readonly IScoreService _scoreService;
    private readonly ICubeRepository _cubeRepository;
    private readonly SoundSystem _soundSystem;

    public MergeService(CubeFactory cubeFactory, IScoreService scoreService, ICubeRepository cubeRepository, SoundSystem soundSystem)
    {
        _cubeFactory = cubeFactory;
        _scoreService = scoreService;
        _validator = new MergeValidator();
        _cubeRepository = cubeRepository;
        _soundSystem = soundSystem;
    }

    public bool CanMerge(Cube a, Cube b)
    {
        return _validator.CanMerge(a, b);
    }

    public MergeResult MergeCubes(Cube a, Cube b)
    {
        if (a.IsMerged || b.IsMerged)
            return null;

        if (!_validator.CanMerge(a, b))
            return null;

        a.IsMerged = true;
        b.IsMerged = true;

        CubeValue newValue = a.Value.NextValue();
        Vector3 spawnPos = (a.transform.position + b.transform.position) / 2f;

        Cube newCube = _cubeFactory.CreateCube(newValue, spawnPos, this);
        DOTween.Sequence()
               .Append(newCube.transform.DOScale(1.5f, 0.2f))
                .Append(newCube.transform.DOScale(1.2f, 0.2f));
        _soundSystem.PlaySound("Merge");
        _scoreService.AddScore(newValue.Value / 4);

        var launcher = newCube.GetComponent<CubeLauncher>();
        if (launcher != null)
        {
            launcher.Init();
            launcher.SetLaunched(true);
        }

        _cubeRepository.Register(newCube);

        Object.Destroy(a.gameObject);
        Object.Destroy(b.gameObject);

        return new MergeResult(a, b, newCube);
    }
}