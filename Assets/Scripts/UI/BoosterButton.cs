using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Resources.Scripts.UI
{
    public class BoosterButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private IAutoMergeService _autoMergeService;

        [Inject]
        public void Construct(IAutoMergeService autoMergeService)
        {
            _autoMergeService = autoMergeService;
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private async void OnClick()
        {
            _button.interactable = false;

            try
            {
                await _autoMergeService.ExecuteAsync();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }

            _button.interactable = true;
        }
    }
}