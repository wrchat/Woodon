using UnityEngine;

namespace WRC.Woodon
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