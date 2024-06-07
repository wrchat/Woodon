using UdonSharp;

namespace Mascari4615
{
    // [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SetActive : UdonSharpBehaviour
    {
        public void SetActiveFalse()
        {
            gameObject.SetActive(false);
        }

        public void SetActiveTrue()
        {
            gameObject.SetActive(true);
        }

        public void ToggleActive()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}