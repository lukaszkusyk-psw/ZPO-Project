using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerController.Instance.Lose();
    }
}
