﻿using UnityEngine;

namespace WRC.Woodon
{
	public abstract class WBoolFollower : WBase
	{
		[Header("_" + nameof(WBoolFollower))]
		[SerializeField] protected WBool wBool;

		public virtual void SetWBool(WBool wBool)
		{
			WDebugLog($"{nameof(SetWBool)} - {wBool}");
			this.wBool = wBool;
		}
	}
}