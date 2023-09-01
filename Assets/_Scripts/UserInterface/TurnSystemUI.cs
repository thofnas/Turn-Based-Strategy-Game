using System;
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
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        }

        private void OnDestroy()
        {
            _endTurnButton.onClick.RemoveAllListeners();
            UnitActionSystem.Instance.OnBusyStateChanged -= UnitActionSystem_OnBusyStateChanged;
            TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
        }

        private void UpdateTurnNumberText()
        {
            _turnNumberText.text = $"Turn {TurnSystem.Instance.GetTurnNumber()}";
        }
        
        private void UnitActionSystem_OnBusyStateChanged(object sender, bool isBusy)
        {
            _endTurnButton.interactable = !isBusy && TurnSystem.Instance.IsPlayerTurn();
        }
        
        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            UpdateTurnNumberText();
            _endTurnButton.interactable = !UnitActionSystem.Instance.IsBusy() && TurnSystem.Instance.IsPlayerTurn();
        }
    }
}
