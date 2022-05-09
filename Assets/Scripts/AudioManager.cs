using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    [SerializeField] private float defaultPitch = 1;
    [SerializeField] private float slowmoPitch = 0.8f;

    public void UpdateMusic(bool normalSpeed)
    {
        audioSource.pitch = normalSpeed ? defaultPitch : slowmoPitch;
    }

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
}
