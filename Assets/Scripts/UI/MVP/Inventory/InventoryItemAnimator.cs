using DG.Tweening;
using Helpers;
using TMPro;
using UnityEngine;

namespace Inventory
{
    public class InventoryItemAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject _item;
        [SerializeField] private TextMeshProUGUI _textDescription;
        [SerializeField] private TextMeshProUGUI _textHeader;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Tween _tween;
        
        public void SetProperties(Material material, Mesh mesh)
        {
            Preconditions.CheckNotNull(mesh);
            Preconditions.CheckNotNull(mesh);
            _meshFilter.mesh = mesh;
            _meshRenderer.material = material;
            
            InitAnimation();
        }

        public void SetTexts(string description, string header)
        {
            Preconditions.CheckNotNull(description);
            Preconditions.CheckNotNull(header);

            _textDescription.text = description;
            _textHeader.text = header;
        }
        
        private void Start()
        {
            _meshRenderer = _item.GetComponent<MeshRenderer>();
            _meshFilter = _item.GetComponent<MeshFilter>();
        }

        private void InitAnimation()
        {
            _tween.Kill();
            _tween = null;
            _item.transform.rotation = Quaternion.identity;
            Rotate();
        }

        private void Rotate()
        {
            _tween = _item.transform.
                DORotate(new Vector3(0, 360f, 0), 1.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }
    }
}