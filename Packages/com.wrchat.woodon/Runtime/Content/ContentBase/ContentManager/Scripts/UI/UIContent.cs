using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[DefaultExecutionOrder(-5000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public abstract class UIContent : WBase
	{
		[Header("_" + nameof(UIContent))]
		[SerializeField] protected ContentManager contentManager;

		protected virtual void Start()
		{
			Init();
			UpdateUI();
		}

		protected virtual void Init()
		{
			WDebugLog($"{nameof(Init)}");
			contentManager.RegisterListener(this, nameof(UpdateUI));
		}

		public abstract void UpdateUI();
	}
}