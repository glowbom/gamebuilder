using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Created on Wed Feb 23 2020
 *
 * Copyright (c) 2020 Glowbom.
 */
public class AudioEngine : MonoBehaviour
{
    public AudioSource correctAudioSource;
    public AudioSource incorrectAudioSource;
    private Dictionary<long, AudioSource> audioMap;

    public AudioSource source;

    public void correct()
    {
        correctAudioSource.Play();
    }

    public void incorrect()
    {
        incorrectAudioSource.Play();
    }

    public void play()
    {
        if (source != null)
        {
            source.Play();
        }
    }

    public class AudioItem
    {
        public string spriteName;
        public float time;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioMap = new Dictionary<long, AudioSource>();
        audioMap.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
