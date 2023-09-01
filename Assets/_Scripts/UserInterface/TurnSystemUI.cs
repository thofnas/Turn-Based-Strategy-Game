using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField] private Button _endTurnButton;
        [SerializeField] private TextMeshProUGUI _turnNumberText;
        
        private void Start()
        {
            UpdateTurnNumberText();
            _endTurnButton.onClick.AddListener(() => TurnSystem.Instance.NextTurn());
            UnitActionSystem.Instance.OnBusyStateChanged += UnitActionSystem_OnBusyStateChanged;
        }

        private void OnDestroy()
        {
            _endTurnButton.onClick.RemoveAllListeners();
            UnitActionSystem.Instance.OnBusyStateChanged -= UnitActionSystem_OnBusyStateChanged;
        }

        private void UpdateTurnNumberText()
        {
            _turnNumberText.text = $"Turn {TurnSystem.Instance.GetTurnNumber()}";
        }
        
        private void UnitActionSystem_OnBusyStateChanged(object sender, bool isBusy)
        {
            _endTurnButton.interactable = !isBusy;
        }
    }
}
