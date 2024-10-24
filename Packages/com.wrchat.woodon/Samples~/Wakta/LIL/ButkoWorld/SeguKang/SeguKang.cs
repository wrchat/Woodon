using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.LIL.ButkoWorld
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SeguKang : MBase
    {
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private string targetAnimatorTriggerName;

        [SerializeField] private TextMeshProUGUI maxScoreText;
        [SerializeField] private TextMeshProUGUI curScoreText;

        [SerializeField] private Rigidbody[] ladles;

        [SerializeField] private int coolTimeByDecisecond;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float minPower = 5f;

        [UdonSynced] [FieldChangeCallback(nameof(CurScore))]
        private int curScore;

        [UdonSynced] [FieldChangeCallback(nameof(MaxScore))]
        private int maxScore;

        [UdonSynced] [FieldChangeCallback(nameof(NextActiveTimeByMilliseconds))]
        private int nextActiveTimeByMilliseconds;

        private int MaxScore
        {
            get => maxScore;
            set
            {
                maxScore = value;
                OnMaxScoreChange();
            }
        }

        private int CurScore
        {
            get => curScore;
            set
            {
                curScore = value;
                OnCurScoreChange();
            }
        }

        private int NextActiveTimeByMilliseconds
        {
            get => nextActiveTimeByMilliseconds;
            set
            {
                nextActiveTimeByMilliseconds = value;
                OnNextActiveTimeChange();
            }
        }


        private void Start()
        {
            if (Networking.IsMaster) NextActiveTimeByMilliseconds = Networking.GetServerTimeInMilliseconds();

            OnMaxScoreChange();
            OnCurScoreChange();
            OnNextActiveTimeChange();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (DEBUG)
                Debug.Log($"{nameof(OnCollisionEnter)} : collision.gameobject.name = {collision.gameObject.name}");

            if (!IsOwner(collision.gameObject))
                return;

            if (Networking.GetServerTimeInMilliseconds() < nextActiveTimeByMilliseconds)
                return;

            foreach (Rigidbody ladle in ladles)
                if (ladle.gameObject == collision.gameObject)
                {
                    if (DEBUG)
                        Debug.Log(
                            $"{collision.relativeVelocity.sqrMagnitude}, {collision.relativeVelocity.sqrMagnitude < minPower}");

                    if (collision.relativeVelocity.sqrMagnitude < minPower)
                        return;

                    SetOwner();

					int newScore = Mathf.RoundToInt(collision.relativeVelocity.sqrMagnitude);
                    // CurScore = ladle.velocity.sqrMagnitude;
                    CurScore = newScore;
                    if (MaxScore < CurScore)
                        MaxScore = CurScore;
                    NextActiveTimeByMilliseconds =
                        Networking.GetServerTimeInMilliseconds() + coolTimeByDecisecond * 100;
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Kang));
                    RequestSerialization();
                    return;
                }
        }

        private void OnMaxScoreChange()
        {
            if (DEBUG)
                Debug.Log(nameof(OnMaxScoreChange));

            maxScoreText.text = MaxScore.ToString();
        }

        private void OnCurScoreChange()
        {
            if (DEBUG)
                Debug.Log(nameof(OnCurScoreChange));

            curScoreText.text = CurScore.ToString();
        }

        private void OnNextActiveTimeChange()
        {
            if (DEBUG)
                Debug.Log(nameof(OnNextActiveTimeChange));

            // curScoreText.text = CurScore.ToString();
        }

        public void Kang()
        {
            if (DEBUG)
                Debug.Log(nameof(Kang));

            targetAnimator.SetTrigger(targetAnimatorTriggerName);
            audioSource.Stop();
            audioSource.Play();
        }
    }
}