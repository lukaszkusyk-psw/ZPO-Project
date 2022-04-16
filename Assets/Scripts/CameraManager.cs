using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
    private void Awake()
    {
        PostProcessVolume postProcess = GetComponent<PostProcessVolume>();

        if (postProcess == null)
            return;

        //postProcess.enabled = SettingsManager.settings.graphics == 0 ? postProcess.enabled = false : postProcess.enabled = true;
    }
}
