using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimator : MonoBehaviour
{
    public Vector2 offset = new Vector2();
    public float speed = 0.5f;

    private Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        material.mainTextureOffset += offset * speed * Time.deltaTime;
    }
}
