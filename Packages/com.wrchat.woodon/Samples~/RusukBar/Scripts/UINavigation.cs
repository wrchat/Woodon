using UdonSharp;
using UnityEngine;
using VRC.Udon;
using WRC.Woodon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class UINavigation : UdonSharpBehaviour
{
	// public UINavigation Upper => upper;
	// [SerializeField] private UINavigation upper;
	// public UINavigation Lower => lower;
	// [SerializeField] private UINavigation lower;
	// public UINavigation Left => left;
	// [SerializeField] private UINavigation left;
	// public UINavigation Right => right;
	// [SerializeField] private UINavigation right;

	public UINavigation upper;
	public UINavigation lower;
	public UINavigation left;
	public UINavigation right;

	[SerializeField] private GameObject selectBorder;
	[SerializeField] private UdonSharpBehaviour targetUdonBehaviour;
	[SerializeField] private string clickEventName;
	[SerializeField] private string selectEventName;
	private string indexToStirng;

	public void Init(int index)
	{
		indexToStirng = index.ToString();
	}

	public void SetSelect(bool select, bool withIndex = false)
	{
		if (selectBorder == null)
			selectBorder = transform.GetChild(0).gameObject;
		selectBorder.SetActive(select);

		if (select)
			targetUdonBehaviour.SendCustomEvent(withIndex ? selectEventName + indexToStirng : selectEventName);
	}

	public void OnClick(bool withIndex = false)
	{
		targetUdonBehaviour.SendCustomEvent(withIndex ? clickEventName + indexToStirng : clickEventName);
	}
}