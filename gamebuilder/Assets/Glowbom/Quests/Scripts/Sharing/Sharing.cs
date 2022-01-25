using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

/*
 * Created on Fri Jun 24 2020
 *
 * Copyright (c) 2020 Glowbom.
 */
public class Sharing : MonoBehaviour
{
#if UNITY_IOS

    [DllImport("__Internal")]
    private static extern void _shareMessage(string message, string url);

    public void shareMessage(string message, string url = "")
    {
        Debug.Log(message + " " + url);
        _shareMessage(message + " " + url, url);
    }

#elif UNITY_ANDROID

    public void shareMessage(string message, string url = "")
    {
        StartCoroutine(shareMessageInAnroid(message, url));
    }

    public IEnumerator shareMessageInAnroid (string message, string url) {

		if (!Application.isEditor) {
			//Create intent for action send
			AndroidJavaClass intentClass = 
				new AndroidJavaClass ("android.content.Intent");
			AndroidJavaObject intentObject = 
				new AndroidJavaObject ("android.content.Intent");
			intentObject.Call<AndroidJavaObject> 
				("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));

			//put text and subject extra
			intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
			intentObject.Call<AndroidJavaObject> 
				("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), "");
			intentObject.Call<AndroidJavaObject> 
				("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), message + " " + url);

			//call createChooser method of activity class
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = 
				unity.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject chooser = 
				intentClass.CallStatic<AndroidJavaObject> 
				("createChooser", intentObject, "Share your high score");
			currentActivity.Call ("startActivity", chooser);
		}

		yield return new WaitUntil (() => true);
	}
	

#else

    public void shareMessage(string message, string url = "")
    {
        Debug.LogError("sharing is not supported on this platform.");
    }

#endif
}
