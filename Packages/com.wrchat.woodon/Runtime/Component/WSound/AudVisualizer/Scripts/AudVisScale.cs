using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AudVisScale : UdonSharpBehaviour
    {
        [SerializeField] private AudVisManager audioPeer;
        [SerializeField] private int band;
        [SerializeField] private bool randomBand;
        [SerializeField] private int maxRandomBand;
        [SerializeField] private float startScale, scaleMultiplier = .05f;
        private bool apply = true;

        private void Start()
        {
            if (audioPeer == null)
                audioPeer = GameObject.Find(nameof(AudVisManager)).GetComponent<AudVisManager>();

            if (startScale == 0)
                startScale = transform.localScale.x;

            if (randomBand)
                band = Random.Range(0, maxRandomBand + 1);
        }

        private void Update()
        {
            if (audioPeer.freqBand == null)
                return;

            transform.localScale =
                Vector3.one * ((apply ? audioPeer.freqBand[band] * scaleMultiplier : 0) + startScale);
        }

        public void ToggleScaleMultiplier()
        {
            apply = !apply;
        }
    }
}