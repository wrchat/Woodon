using UnityEngine;

namespace WRC.Woodon
{
    public class PosFixerReset : WBase
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