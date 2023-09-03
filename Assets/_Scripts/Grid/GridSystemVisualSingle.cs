using UnityEngine;

namespace Grid
{
    public class GridSystemVisualSingle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        
        public void Show(Color color, float alpha)
        {
            color.a = alpha;
            _meshRenderer.material.color = color;
            _meshRenderer.enabled = true;
        }

        public void Hide() => _meshRenderer.enabled = false;
    }
}
