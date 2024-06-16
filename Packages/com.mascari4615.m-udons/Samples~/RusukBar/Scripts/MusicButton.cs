using TMPro;
using UdonSharp;
using UnityEngine.UI;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class MusicButton : MBase
    {
        private Image albumCover;
        private TextMeshProUGUI artist;
        private TextMeshProUGUI title;

        private void Start()
        {
            if (title == null)
                GetComponents();
        }

        public void Init(MusicData musicData)
        {
            if (title == null)
                GetComponents();

            title.text = musicData.gameObject.name;
            artist.text = musicData.artist;
            albumCover.sprite = musicData.albumCover;
        }

        private void GetComponents()
        {
            title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            artist = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            albumCover = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        }
    }
}