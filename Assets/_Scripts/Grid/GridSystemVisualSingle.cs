using UnityEngine;

namespace Grid
{
    public class GridSystemVisualSingle : MonoBehaviour
    {
        private static readonly int ColorID = Shader.PropertyToID("_BaseColor");
        
        [SerializeField] private MeshRenderer _meshRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        public void Show(Color color, float alpha)
        {
            color.a = alpha;
            color.r -= (1 - alpha) / 3;
            color.g -= (1 - alpha) / 3;
            color.b -= (1 - alpha) / 3;
            
            _materialPropertyBlock.SetColor(ColorID, color);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
            
            _meshRenderer.enabled = true;
        }

        public void Hide() => _meshRenderer.enabled = false;
    }
}
