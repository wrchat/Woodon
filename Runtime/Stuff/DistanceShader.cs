using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class DistanceShader : UdonSharpBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    private void Update()
    {
        var distance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), transform.position);
        var calc = Mathf.Clamp(distance, 0, 30f);
        meshRenderer.material.SetFloat("_MatCapBlend", 1 - calc / 30f);
    }
}