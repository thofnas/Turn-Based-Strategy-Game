using System;
using Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Image _selectedBorderImage;
    private BaseAction _action;

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyStateChanged += UnitActionSystem_OnBusyStateChanged;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnBusyStateChanged -= UnitActionSystem_OnBusyStateChanged;
    }
    
    public void SetAction(BaseAction action)
    {
        _action = action;
        _textMeshPro.text = action.GetActionName();
        
        _button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(action);
        });
    }

    public void UpdateSelectedVisual()
    {
        _selectedBorderImage.gameObject.SetActive(_action == UnitActionSystem.Instance.GetSelectedAction());
    }
    
    private void UnitActionSystem_OnBusyStateChanged(object sender, bool isBusy)
    {
        _button.interactable = !isBusy;
    }
}