using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AudVisLight : UdonSharpBehaviour
    {
        [SerializeField] private new Light light;
        [SerializeField] private AudVisManager audioPeer;
        [SerializeField] private int band;
        [SerializeField] private float minIntensity, intensityMultiplier = .05f;
        private bool apply = true;

        private void Start()
        {
            if (audioPeer == null)
                audioPeer = GameObject.Find(nameof(AudVisManager)).GetComponent<AudVisManager>();

            if (light == null)
                light = GetComponent<Light>();
        }

        private void Update()
        {
            if (audioPeer.freqBand == null)
                return;

            light.intensity = (apply ? audioPeer.freqBand[band] * intensityMultiplier : 0) + minIntensity;
        }

        public void ToggleScaleMultiplier()
        {
            apply = !apply;
        }
    }
}