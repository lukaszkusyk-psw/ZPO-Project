using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    private Material material;
    public float multiplier = 1f;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        material.mainTextureOffset = new Vector2(material.mainTextureOffset.x, 
            material.mainTextureOffset.y - PlayerController.CurrentSpeed * Time.deltaTime * multiplier * PlayerController.MinigameSpeed);
    }
}