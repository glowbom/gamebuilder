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
    }

    public void playTrack() {
        track.Play();
    }

    public void playError() {
        error1.Play();
    }

    public void playError2() {
        error2.Play();
    }
}
