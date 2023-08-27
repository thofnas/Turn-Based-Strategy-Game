using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _coordsText;
        private GridObject _gridObject;
        
        public void SetGridObject(GridObject gridObject)
        {
            _gridObject = gridObject;
        }

        private void Update()
        {
            _coordsText.text = _gridObject.ToString();
        }
    }
}
