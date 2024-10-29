using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ShootingManager : MBase
    {
        [SerializeField] private ShootingTarget[] shootingTargets;

        public ShootingTarget GetShootingTarget(int num)
        {
            return shootingTargets[num];
        }

        public void Ahh(GameObject shootingTarget)
        {
            foreach (var target in shootingTargets)
                if (target.gameObject == shootingTarget)
                {
                    target.Ahh();
                    return;
                }
        }
    }
}