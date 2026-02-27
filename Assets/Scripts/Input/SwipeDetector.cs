using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class SwipeDetector : MonoBehaviour
{
    public event Action<float> OnSwipeDelta;
    public event Action OnSwipeEnd;

    [SerializeField] private LayerMask uiLayer;

    private GraphicRaycaster _raycaster;
    private EventSystem _eventSystem;
    private Camera _camera;

    private PointerEventData _pointerEventData;
    private readonly List<RaycastResult> _results = new();

    private Vector3 _startPos;
    private bool _isSwiping;

    [Inject]
    public void Construct(
        GraphicRaycaster raycaster,
        EventSystem eventSystem,
        Camera camera)
    {
        _raycaster = raycaster;
        _eventSystem = eventSystem;
        _camera = camera;

        _pointerEventData = new PointerEventData(_eventSystem);
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI(Input.mousePosition)) return;

            _startPos = Input.mousePosition;
            _isSwiping = true;
        }

        if (Input.GetMouseButton(0) && _isSwiping)
        {
            float deltaX = Input.mousePosition.x - _startPos.x;
            OnSwipeDelta?.Invoke(deltaX);
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && _isSwiping)
        {
            OnSwipeEnd?.Invoke();
            _isSwiping = false;
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (IsPointerOverUI(touch.position)) return;

            _startPos = touch.position;
            _isSwiping = true;
        }

        if (touch.phase == TouchPhase.Moved && _isSwiping)
        {
            OnSwipeDelta?.Invoke(touch.deltaPosition.x);
        }

        if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && _isSwiping)
        {
            OnSwipeEnd?.Invoke();
            _isSwiping = false;
        }
    }

    private bool IsPointerOverUI(Vector3 screenPos)
    {
        _results.Clear();

        _pointerEventData.position = screenPos;
        _raycaster.Raycast(_pointerEventData, _results);

        if (_results.Count > 0)
            return true;

        Ray ray = _camera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, 100f, uiLayer))
            return true;

        return false;
    }
}
