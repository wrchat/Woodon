using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAvatarButton : MBase
	{
		[SerializeField] private VRCAvatarPedestal avatarPedestal;
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private Animator animator;

		public void SetData(string id, string name)
		{
			MDebugLog($"{nameof(SetData)} : {id}, {name}");

			avatarPedestal.blueprintId = id;
			nameText.text = name;
			animator.SetTrigger("Trigger");
		}
	}
}