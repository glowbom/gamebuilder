using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEngine : MonoBehaviour
{
    public AudioSource track;
    public AudioSource bass;
    public AudioSource error1;
    public AudioSource error2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playBass() {
        bass.Play();
        track.Stop();
    }

    public void playTrack() {
        track.Play();
        bass.Stop();
    }

    public void playError() {
        error1.Play();
        track.volume = 0.05f;
        bass.volume = 0.05f;
        Invoke("resumeVolume", 1f);
    }

    public void playError2() {
        error2.Play();
        track.volume = 0.05f;
        bass.volume = 0.05f;
        Invoke("resumeVolume", 1f);
    }

    public void resumeVolume() {
        track.volume = 1f;
        bass.volume = 1f;
    } 
}
