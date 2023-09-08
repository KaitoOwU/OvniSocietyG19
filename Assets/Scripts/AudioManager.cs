using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static List<AudioClip> _audioClips;
    private static AudioSource _audioSource;
    private void Start()
    {
        _audioClips = Resources.LoadAll<AudioClip>("Audio").ToList();
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlayClip(int id)
    {
        _audioSource.clip = _audioClips[id];
        _audioSource.Play();
    }
}
