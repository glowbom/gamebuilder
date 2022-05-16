using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Created on Wed Jan 22 2020
 *
 * Copyright (c) 2020 Glowbom.
 */
public class PlayAudio : MonoBehaviour
{
    public AudioSource correctAudioSource;
    public AudioSource incorrectAudioSource;

    public void correct()
    {
        correctAudioSource.Play();
    }

    public void incorrect()
    {
        incorrectAudioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
