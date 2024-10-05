using UnityEngine;

namespace WakVRC
{
    public class PosFixerReset : MBase
    {
        [SerializeField] private PosFixer[] posFixers;

        public override void Interact()
        {
            foreach (var posFixer in posFixers)
            {
                SetOwner(posFixer.gameObject);
                posFixer.transform.localPosition = posFixer.OriginPos;
            }
        }
    }
}