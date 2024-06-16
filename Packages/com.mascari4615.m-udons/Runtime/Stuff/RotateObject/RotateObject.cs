using UdonSharp;
using UnityEngine;

public class RotateObject : UdonSharpBehaviour
{
    public float speed;

    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private bool z;
    [SerializeField] private Space space;

    private void Update()
    {
        transform.Rotate(x ? Time.deltaTime * speed : 0, y ? Time.deltaTime * speed : 0, z ? Time.deltaTime * speed : 0, space);
    }
}