using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
    public class ColorSlider : MBase
    {
        [SerializeField] private Slider rSlider;
        [SerializeField] private TextMeshProUGUI rText;

        [SerializeField] private Slider gSlider;
        [SerializeField] private TextMeshProUGUI gText;

        [SerializeField] private Slider bSlider;
        [SerializeField] private TextMeshProUGUI bText;

        [SerializeField] private MeshRenderer target;

        [SerializeField] private TextMeshProUGUI ownerText;

        [UdonSynced()] private Color curColor;
        [UdonSynced()] private Color originColor;

        private void Start()
        {
            if (Networking.IsMaster)
                if (target != null)
                {
                    originColor = target.material.color;

                    rSlider.value = originColor.r;
                    gSlider.value = originColor.g;
                    bSlider.value = originColor.b;

                    curColor = originColor;
                }
        }

        private void Update()
        {
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (IsOwner()) curColor = new Color(rSlider.value, gSlider.value, bSlider.value);

            ownerText.text = Networking.GetOwner(gameObject).displayName;

            rSlider.value = curColor.r;
            gSlider.value = curColor.g;
            bSlider.value = curColor.b;

            rText.text = curColor.r.ToString();
            gText.text = curColor.g.ToString();
            bText.text = curColor.b.ToString();

            if (target != null)
                target.material.color = curColor;
        }

        public void _SetOwner()
        {
            SetOwner();
        }

        public void SetToOrigin()
        {
            SetOwner();

            rSlider.value = originColor.r;
            gSlider.value = originColor.g;
            bSlider.value = originColor.b;

            curColor = originColor;
        }
    }
}