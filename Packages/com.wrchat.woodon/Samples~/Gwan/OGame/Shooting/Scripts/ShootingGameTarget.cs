using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ShootingGameTarget : MBase
    {
        private bool active;
        private BoxCollider boxCollider;

        private int lineIndex = -1;
        private Vector3 moveDirection = Vector3.zero;
        private float moveSpeed;
        private Vector3 originPos;

        private ShootingGameManager shootingGameManager;
        private SpriteRenderer spriteRenderer;
        private float t;
        private Vector3 targetPos;
        public int FandomIndex { get; private set; }

        private bool Active
        {
            get => active;
            set
            {
                active = value;

                spriteRenderer.enabled = active;
                boxCollider.enabled = active;
            }
        }

        private void Start()
        {
            shootingGameManager = GameObject.Find(nameof(ShootingGameManager)).GetComponent<ShootingGameManager>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            if (Active)
            {
				/*
                transform.localPosition += Time.deltaTime * moveDirection * moveSpeed;

                if (Mathf.Abs(transform.localPosition.x) >= 10)
                {
                    Active = false;
                }
                */

				// https://yoonstone-games.tistory.com/110

				Vector3 startPos = originPos;
				Vector3 endPos = targetPos;
				Vector3 center = (startPos + endPos) * 0.5f;

                // 포지션이 위로 그려지도록 중앙값 아래로 내리기
                center.y -= 3;

                // 시작지점, 끝지점을 내려간 중앙값을 기준으로 수정
                startPos = startPos - center;
                endPos = endPos - center;

				Vector3 point = Vector3.Slerp(startPos, endPos, t += Time.deltaTime * .3f * moveSpeed);

                // 포물선을 위로 그리기 위해 center를 빼줬으므로 다시 더해서 원상복구
                transform.localPosition = point + center;

                if (t >= 1) Active = false;
            }
        }

        public void KickThatAss()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ActiveFalse));
        }

        public void ActiveFalse()
        {
            Active = false;
        }

        public void Init(int fandomIndex, int lineIndex, Vector3 moveDirection, float moveSpeed)
        {
            FandomIndex = fandomIndex;
            this.lineIndex = lineIndex;
            this.moveDirection = moveDirection;
            this.moveSpeed = moveSpeed;
            t = 0;

            spriteRenderer.sprite = shootingGameManager.GetFandomSprite(fandomIndex);
            // transform.localPosition = Vector3.up * lineIndex + (moveDirection * -10);
            // transform.localPosition += Time.deltaTime * moveDirection;

            originPos = Vector3.up * lineIndex + moveDirection * -10;
            targetPos = Vector3.up * lineIndex + moveDirection * 10;

            Active = true;
        }
    }
}