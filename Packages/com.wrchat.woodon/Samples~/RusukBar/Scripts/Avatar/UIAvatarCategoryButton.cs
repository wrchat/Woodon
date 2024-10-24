using TMPro;
using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAvatarCategoryButton : MBase
	{
		[SerializeField] private MAvatarList menu;
		private int avatarType;

		[SerializeField] private TextMeshProUGUI nameText;

		public void SelectCategory()
		{
			menu.SelectType(avatarType);
		}

		public void SetData(int avatarType, string name)
		{
			MDebugLog($"{nameof(SetData)} : {avatarType}, {name}");
		
			this.avatarType = avatarType;
			nameText.text = name;
		}
	}
}