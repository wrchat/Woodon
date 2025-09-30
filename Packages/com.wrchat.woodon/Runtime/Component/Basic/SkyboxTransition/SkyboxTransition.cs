using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SkyboxTransition : UdonSharpBehaviour
{
	[SerializeField] private Material[] skyBoxes;

	[UdonSynced, FieldChangeCallback(nameof(State))] private int state = 0;
	public int State
	{
		get => state;
		set
		{
			state = value;
			OnStateChange();
		}
	}

	private void Start()
	{
		OnStateChange();
	}

	private void OnStateChange()
	{
		RenderSettings.skybox = skyBoxes[state];
	}

	public override void Interact()
	{
		NextSkybox();
	}
	
	[ContextMenu(nameof(NextSkybox))]
	public void NextSkybox()
	{
		if (Networking.IsOwner(gameObject) == false)
			Networking.SetOwner(Networking.LocalPlayer, gameObject);

		State = (State + 1) % skyBoxes.Length;

		RequestSerialization();
	}
}