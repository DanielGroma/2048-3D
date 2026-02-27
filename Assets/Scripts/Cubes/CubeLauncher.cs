using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class CubeLauncher : MonoBehaviour
{
    private SoundSystem _soundSystem;

    [SerializeField] private Rigidbody _rb;

    [Header("Swipe settings")]
    [SerializeField] private float dragSensitivity = 5f;
    [SerializeField] private float maxSwipeDistance = 3f;

    [Header("Launch settings")]
    [SerializeField] private float launchForce = 15f;

    private float _startZ;

    public bool IsLaunched { get; private set; }

    public event Action<CubeLauncher> OnDestroyed;
    public event Action<CubeLauncher> OnLaunched;

    [Inject]
    public void Construct(SoundSystem soundSystem)
    {
        _soundSystem = soundSystem;
    }

    public void Init()
    {
        IsLaunched = false;
        _rb.isKinematic = false;
        _startZ = transform.position.z;
    }

    public void Drag(float deltaX)
    {
        if (IsLaunched) return;

        float moveZ = -deltaX * dragSensitivity;

        float targetZ = Mathf.Clamp(
            transform.position.z + moveZ,
            _startZ - maxSwipeDistance,
            _startZ + maxSwipeDistance);

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            targetZ);
    }

    public void Launch()
    {
        if (IsLaunched) return;

        IsLaunched = true;

        _rb.AddForce(
            transform.right * launchForce,
            ForceMode.Impulse);
        _soundSystem.PlaySound("Launch");
        OnLaunched?.Invoke(this);
    }
    public void SetLaunched(bool value)
    {
        IsLaunched = value;

        if (value)
            OnLaunched?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}