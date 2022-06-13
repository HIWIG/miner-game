using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtruckPlayer : MonoBehaviour
{
    public AudioClip[] SoundTruck;
    AudioSource AS;
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if(AS.isPlaying == false)
        {
            AS.clip = SoundTruck[0];
            AS.Play();
        }
    }
}
