using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TypeExtensions
{
    public static void SetActiveOptimized(this GameObject gameObject, bool value)
    {
        if (gameObject.activeSelf != value)
            gameObject.SetActive(value);
    }
}
