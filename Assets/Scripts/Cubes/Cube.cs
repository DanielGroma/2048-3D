using Game.Cubes;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Resources.Scripts.Cubes
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private TextMeshProUGUI[] _faceTexts;
        [SerializeField] private BoxCollider _collider;

        private ICubeRepository _cubeRepository;
        public CubeValue Value { get; private set; }
        public bool IsMerged { get; set; }

        [Inject]
        public void Construct(ICubeRepository repo)
        {
            _cubeRepository = repo;
        }

        private void OnEnable()
        {
            if (_cubeRepository != null)
            {
                _cubeRepository.Register(this);
            }
        }
        public void OnDestroy()
        {
            if(_cubeRepository != null)
            {
                _cubeRepository.Unregister(this);
            }
        }
        public void SetValue(CubeValue value, Color color)
        {
            Value = value;
            _renderer.material.color = color;

            string text = value.ToString();
            foreach (var faceText in _faceTexts)
            {
                faceText.text = text;
            }
        }
        public void EnableCollider()
        {
            _collider.enabled = true;
        }
        public void DisableCollider()
        {
            _collider.enabled = false;
        }
    }
}