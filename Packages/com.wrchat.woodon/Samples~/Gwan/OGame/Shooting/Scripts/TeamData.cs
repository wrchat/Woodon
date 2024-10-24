using TMPro;
using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class TeamData : MBase
    {
        [SerializeField] private TextMeshPro teamScoreText;
        [SerializeField] private TextMeshPro teamFandomText;
        [SerializeField] private SpriteRenderer selectedFandomImage;
        private ShootingGameManager shootingGameManager;

        [UdonSynced] [FieldChangeCallback(nameof(TeamFandomIndex))]
        private int teamFandomIndex;

        [UdonSynced] [FieldChangeCallback(nameof(TeamScore))]
        private int teamScore;

        private int TeamFandomIndex
        {
            get => teamFandomIndex;
            set
            {
                teamFandomIndex = value;
                OnTeamFandomIndexChange();
            }
        }

        private int TeamScore
        {
            get => teamScore;
            set
            {
                teamScore = value;
                OnTeamScoreChange();
            }
        }

        private void Start()
        {
            shootingGameManager = GameObject.Find(nameof(ShootingGameManager)).GetComponent<ShootingGameManager>();
            OnTeamFandomIndexChange();
            OnTeamScoreChange();
        }

        private void OnTeamFandomIndexChange()
        {
            MDebugLog($"{nameof(OnTeamFandomIndexChange)} : {teamFandomIndex}");
            selectedFandomImage.sprite = shootingGameManager.GetFandomSprite(teamFandomIndex);
            // teamFandomText.text = fandomDic[teamFandomIndex];
        }

        public void ResetScore()
        {
            SetOwner();
            TeamScore = 0;
            RequestSerialization();
        }

        private void OnTeamScoreChange()
        {
            MDebugLog($"{nameof(OnTeamScoreChange)} : {teamScore}");
            teamScoreText.text = teamScore.ToString();
        }

        public void ChangeTeamFandom()
        {
            SetOwner();
            TeamFandomIndex = (TeamFandomIndex + 1) % 7;
            RequestSerialization();
        }

        public void Ahoy(GameObject shootingTargetObject)
        {
            MDebugLog($"{nameof(Ahoy)} : {shootingTargetObject.name}");

            SetOwner();

			int stIndex = int.Parse(shootingTargetObject.name.Split('_')[1]);

            if (shootingGameManager.GetShootingTarget(stIndex).FandomIndex == 0)
                TeamScore -= 1;
            else if (shootingGameManager.GetShootingTarget(stIndex).FandomIndex == teamFandomIndex) TeamScore += 1;

            shootingGameManager.GetShootingTarget(stIndex).KickThatAss();

            RequestSerialization();
        }
    }
}