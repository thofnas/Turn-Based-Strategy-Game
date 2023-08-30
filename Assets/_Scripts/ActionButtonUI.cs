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
}