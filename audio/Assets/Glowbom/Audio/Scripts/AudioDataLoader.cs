using UnityEngine;
using System.IO;

/*
 * Created on Wed Apr 1 2020
 *
 * Copyright (c) 2020 Glowbom, Inc.
 */
public class AudioDataLoader
{
    public AudioData audioData = null;

    public void load()
    {
        var textAsset = Resources.Load("Data/AudioData") as TextAsset;
        if (textAsset == null)
        {
            audioData = new AudioData();
        }
        else
        {
            audioData = JsonUtility.FromJson<AudioData>(textAsset.text);
        }
    }

    public void initialize()
    {
        if (audioData == null)
        {
            load();
        }
    }

    public void save()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter("Assets/Glowbom/Audio/Resources/Data/AudioData.txt", false))
            {
                sw.Write(JsonUtility.ToJson(audioData));
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }

}
