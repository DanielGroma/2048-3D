using Assets.Resources.Scripts.Core;
using Assets.Resources.Scripts.Cubes;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    private Cube _cube;
    private IMergeService _mergeService;
    private GameStateService _gameStateService;

    private bool _isLaunched = false;
    private bool _isInsideDeathZone = false;
    private float _deathZoneTimer = 0f;

    [SerializeField] private float _deathDelay = 1f;
    [SerializeField] private float _minCollisionImpulse;

    public void Init(IMergeService mergeService, GameStateService gameStateService)
    {
        _mergeService = mergeService;
        _gameStateService = gameStateService;
    }

    private void Awake()
    {
        _cube = GetComponent<Cube>();
    }

    private void Update()
    {
        if (!_isLaunched) return;

        if (_isInsideDeathZone)
        {
            _deathZoneTimer += Time.deltaTime;
            if (_deathZoneTimer >= _deathDelay)
            {
                if (_gameStateService.IsState(GameState.GameOver)) return;
                _gameStateService.SetState(GameState.GameOver);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Cube other = collision.gameObject.GetComponent<Cube>();
        if (other == null || other == _cube) return;

        if (!HasSufficientImpulse(collision))
        {
            return;
        }
        _mergeService.MergeCubes(_cube, other);
    }

    private bool HasSufficientImpulse(Collision collision)
    {
        float impulseMagnitude = collision.impulse.magnitude;
        if (impulseMagnitude < _minCollisionImpulse)
        {
            return false;
        }

        Vector3 directionToOther = (collision.transform.position - transform.position).normalized;
        Vector3 impulseDirection = collision.impulse.normalized;
        float dot = Vector3.Dot(directionToOther, impulseDirection);

        return dot > 0.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DeathZone")) return;

        if (!_isLaunched)
        {
            _isLaunched = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("DeathZone")) return;

        if (_isLaunched)
        {
            _isInsideDeathZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("DeathZone")) return;

        _isInsideDeathZone = false;
        _deathZoneTimer = 0f;

    }
}
