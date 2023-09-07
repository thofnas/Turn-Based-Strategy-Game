using TMPro;
using UnityEngine;

namespace Grid
{
    public abstract class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _coordsText;
        private object _gridObject;
        
        public virtual void SetGridObject(object gridObject)
        {
            _gridObject = gridObject;
        }

        protected virtual void Update()
        {
            _coordsText.text = _gridObject.ToString();
        }
    }
}
