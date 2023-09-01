using UnityEngine;

namespace UserInterface
{
    public class BusyUI : MonoBehaviour
    {
        private void Start()
        {
            UnitActionSystem.Instance.OnBusyStateChanged += UnitActionSystem_OnBusyStateChanged;
        
            Hide();
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnBusyStateChanged -= UnitActionSystem_OnBusyStateChanged;
        }

        private void Show() => gameObject.SetActive(true);
    
        private void Hide() => gameObject.SetActive(false);

        private void UnitActionSystem_OnBusyStateChanged(object sender, bool isBusy)
        {
            if (isBusy)
                Show();
            else
                Hide();
        }
    }
}
