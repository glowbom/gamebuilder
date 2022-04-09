using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEngine : MonoBehaviour
{

	public AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
    }

    IEnumerator PlayAudioSource(float t) 
    {
    	yield return new WaitForSeconds(t);
    	audioSource.Play();
    }

    void PlayBass() 
    {
    	audioSource.Play(); 
    }

    public void PlayBassFromButtonClick() 
    {
    	PlayBass(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


// Links of reference

// https://weeklyhow.com/unity-delay-function/
