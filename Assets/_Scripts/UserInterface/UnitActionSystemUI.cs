using System;
using Actions;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform _actionButtonContainerTransform;
        [SerializeField] private ActionButtonUI _actionButtonPrefab;
        [SerializeField] private TextMeshProUGUI _actionPointsText;

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

            UpdateUnitActionButtons();
        
            UpdateSelectedVisual();
        
            UpdateActionPoints();
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionStarted -= UnitActionSystem_OnActionStarted;
            Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        }

        private void UpdateUnitActionButtons()
        {
            foreach (Transform buttonTransform in _actionButtonContainerTransform)
            {
                Destroy(buttonTransform.gameObject);
            }
        
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

            foreach (BaseAction action in selectedUnit.GetActionsArray())
            {
                ActionButtonUI button = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
            
                button.SetAction(action);
            }
        }

        private void UpdateSelectedVisual()
        {
            foreach (Transform actionButton in _actionButtonContainerTransform)
            {
                actionButton.GetComponent<ActionButtonUI>().UpdateSelectedVisual();
            }
        }

        private void UpdateActionPoints()
        {
            int actionPoints = UnitActionSystem.Instance.GetSelectedUnit().GetActionPoints();
            _actionPointsText.text = $"Action points: {actionPoints}";
        }

        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
        {
            UpdateUnitActionButtons();
        
            UpdateSelectedVisual();
        
            UpdateActionPoints();
        }
    
        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) => UpdateSelectedVisual();

        private void UnitActionSystem_OnActionStarted(object sender, EventArgs e) => UpdateActionPoints();

        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e) => UpdateActionPoints();
    }
}