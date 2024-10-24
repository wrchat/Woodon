using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ShootingBullet : MBase
    {
        private ParticleSystem particle;
        private ShootingGameManager shootingGameManager;

        private void Start()
        {
            particle = GetComponent<ParticleSystem>();
            shootingGameManager = GameObject.Find(nameof(ShootingGameManager)).GetComponent<ShootingGameManager>();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (Networking.IsOwner(transform.parent.gameObject))
                if (other.name.Contains("ST"))
                {
                    Debug.Log($"{nameof(OnParticleCollision)} : {other.name}");
                    shootingGameManager.Ahoy(other);
                }
        }

        public void BBang()
        {
            particle.Play();
        }
    }
}