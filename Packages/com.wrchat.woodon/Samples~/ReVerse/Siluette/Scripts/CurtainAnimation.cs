using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

namespace Mascari4615.Project.ReVerse
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class CurtainAnimation : MBase
    {
        [SerializeField] private SilhouetteMask silhouetteMask;
        [SerializeField] private Image uiImage;
        [SerializeField] private Animator animator;

        [UdonSynced] [FieldChangeCallback(nameof(CurtainActive))]
        private bool curtainActive = true;

        private bool CurtainActive
        {
            get => curtainActive;
            set
            {
                curtainActive = value;
                OnCurtainActiveChange();
            }
        }

        private void Start()
        {
            OnCurtainActiveChange();
        }

        private void OnCurtainActiveChange()
        {
            animator.SetBool("CURTAIN_ACTIVE", curtainActive);
            uiImage.color = curtainActive ? Color.green : Color.red;
        }

        public void ToggleCurtainActive()
        {
            SetOwner();
            CurtainActive = !CurtainActive;

            if (CurtainActive == false)
                silhouetteMask.OffAllBit();

            RequestSerialization();
        }
    }
}