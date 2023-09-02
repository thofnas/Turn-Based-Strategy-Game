using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class UnitWorldUI : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private TextMeshProUGUI _actionPointsText;
        [SerializeField] private Image _healthFillImage;

        private void Start()
        {
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
            _healthSystem.OnUnitDamaged += HealthSystem_OnUnitDamaged;
            UpdateActionPointsText();
            UpdateHealthBar();
        }

        private void OnDestroy() => Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;

        private void UpdateActionPointsText() => _actionPointsText.text = _unit.GetActionPoints().ToString();

        private void UpdateHealthBar() => _healthFillImage.fillAmount = _healthSystem.GetHealthNormalized();

        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e) => UpdateActionPointsText();

        private void HealthSystem_OnUnitDamaged(object sender, EventArgs e) => UpdateHealthBar();
    }
}
