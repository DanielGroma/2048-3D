using Assets.Resources.Scripts.Cubes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Cubes;
using UnityEngine;

namespace Assets.Resources.Scripts.Boosters
{
    public class AutoMergeService : IAutoMergeService
    {
        private readonly ICubeRepository _cubeRepository;
        private readonly IMergeService _mergeService;
        private readonly InputService _inputService;
        private readonly SoundSystem _soundSystem;

        private const float FixedHeight = 5f;
        private const float SwingBackDistance = 1.5f;

        public AutoMergeService(
            ICubeRepository cubeRepository,
            IMergeService mergeService,
            InputService inputService,
            SoundSystem soundSystem)
        {
            _cubeRepository = cubeRepository;
            _mergeService = mergeService;
            _inputService = inputService;
            _soundSystem = soundSystem;
        }

        public async UniTask ExecuteAsync()
        {
            var pair = _cubeRepository.GetFirstMergeablePair();
            if (pair == null) return;

            Cube cubeA = pair.Value.cubeA;
            Cube cubeB = pair.Value.cubeB;
            if (cubeA == null || cubeB == null) return;

            cubeA.DisableCollider();
            cubeB.DisableCollider();

            _inputService.ClearCurrentCubeIfMatches(cubeA.GetComponent<CubeLauncher>());
            _inputService.ClearCurrentCubeIfMatches(cubeB.GetComponent<CubeLauncher>());

            Vector3 startA = cubeA.transform.position;
            Vector3 startB = cubeB.transform.position;

            Vector3 center = new Vector3(
                (startA.x + startB.x) / 2f,
                FixedHeight,
                (startA.z + startB.z) / 2f
            );

            Vector3 upA = new Vector3(startA.x, FixedHeight, startA.z);
            Vector3 upB = new Vector3(startB.x, FixedHeight, startB.z);

            Vector3 dirToCenterA = (center - upA).normalized;
            Vector3 dirToCenterB = (center - upB).normalized;

            Vector3 swingA = upA - dirToCenterA * SwingBackDistance;
            Vector3 swingB = upB - dirToCenterB * SwingBackDistance;

            var mainSequence = DOTween.Sequence();

            mainSequence.Append(cubeA.transform.DOMove(upA, 0.6f).SetEase(Ease.OutQuad));
            mainSequence.Join(cubeB.transform.DOMove(upB, 0.6f).SetEase(Ease.OutQuad));

            mainSequence.Append(cubeA.transform.DOMove(swingA, 0.4f).SetEase(Ease.OutSine));
            mainSequence.Join(cubeB.transform.DOMove(swingB, 0.4f).SetEase(Ease.OutSine));

            mainSequence.AppendCallback(() => _soundSystem.PlaySound("AutoMerge"));
            mainSequence.Append(cubeA.transform.DOMove(center, 0.3f).SetEase(Ease.InQuad));
            mainSequence.Join(cubeB.transform.DOMove(center, 0.3f).SetEase(Ease.InQuad));

            await mainSequence.AsyncWaitForCompletion();

            if (cubeA == null || cubeB == null) return;

            var mergeResult = _mergeService.MergeCubes(cubeA, cubeB);
            Cube newCube = mergeResult.NewCube.GetComponent<Cube>();
            if (newCube == null) return;

            await DOTween.Sequence()
               .Append(newCube.transform.DOScale(1.5f, 0.2f))
                .Append(newCube.transform.DOScale(1.2f, 0.2f))
                .AsyncWaitForCompletion();
            if (mergeResult?.NewCube == null) return;

        }
    }
}