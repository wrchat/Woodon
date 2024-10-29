// Audio Visualize script: 
// -Assign a prefab asset to the prefab01 field in the Inspector.
// -Set YScale value in Inspector.
// -Assign a audio source to the audSource field in the Inspector.

using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AudVisManager : MBase
    {
        [SerializeField] private int arrCount = 512;

        // [SerializeField] private float fwdAmount = 100;
        // [SerializeField] private GameObject prefab01;
        // Transform[] transArr01;
        // [SerializeField] private float minYScale;
        // [SerializeField] private float maxYScale;
        [SerializeField] private FFTWindow FFTWindow;
        [SerializeField] private AudioSource audSource;
        private float[] samples;
        public int ArrCount => arrCount;
        public float[] freqBand { get; private set; }

        private void Start()
        {
            samples = new float[arrCount];
            freqBand = new float[arrCount];

            /*
            transArr01 = new Transform[arrCount];
            for (int i = 0; i < transArr01.Length; i++)
            {
                transArr01[i] = VRCInstantiate(prefab01).transform;
                transArr01[i].position = transform.position;
                transArr01[i].parent = transform;
                this.transform.eulerAngles = new Vector3(0, -(transArr01.Length / 360f) * i, 0);
                transArr01[i].position = Vector3.forward * fwdAmount;
            }

            this.transform.eulerAngles = new Vector3(-90, 0, 0);
            */
        }

        private void Update()
        {
            // samples = new float[transArr01.Length];
            audSource.GetSpectrumData(samples, 0, FFTWindow);
            MakeFrequencyBands();
            //for (int i = 0; i < samples.Length; i++)
            //	transArr01[i].localScale = new Vector3(10, (samples[i] * maxYScale) + minYScale, 10);
        }

        private void MakeFrequencyBands()
        {
            /*
             * 22050 / 512 = 43hertz per sample
             * 
             * 20 - 60 hertz
             * 60 - 250
             * 250 - 500
             * 500 - 2000
             * 2000 - 4000
             * 4000 - 6000
             * 6000 - 20000
             * 
             * 0 - 2	= 86 hertz
             * 1 - 4	= 177 herrt -	87	-	258
             * 2 - 8	= 344 hertz -	259	-	602
             * 3 - 16	= 688 hertz -	603	-	1290
             * 4 - 32	= 1376 hertz -	1291-	2666
             * 5 - 64	= 2752 hertz -	2667-	5418
             * 6 - 128	= 5504 hertz -	5419-	10922
             * 7 - 256	= 11008 hertz -	10923-	21930
             * 
             * 510
             * 
             * 
             */

            var count = 0;

            for (var i = 0; i < 8; i++)
            {
                float average = 0;
                var sampltCount = (int)Mathf.Pow(2, i) * 2;

                if (i == 7) sampltCount += 2;

                for (var j = 0; j < sampltCount; j++)
                {
                    average += samples[count] * (count + 1);
                    count++;
                }

                average /= count;

                freqBand[i] = average * 10;
            }
        }
    }
}