using System.IO;
using UnityEngine;

/*
 * Created on Mon Dec 16 2019
 *
 * Copyright (c) 2019 Glowbom.
 */
public class MonetizationLoader
{
    public Monetization monetization = null;

    public void load()
    {
        var textAsset = Resources.Load("Data/Monetization") as TextAsset;
        if (textAsset == null)
        {
            monetization = new Monetization();
        }
        else
        {
            monetization = JsonUtility.FromJson<Monetization>(textAsset.text);
        }
    }

    public void initialize()
    {
        if (monetization == null)
        {
            load();
        }
    }

    public void save()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter("Assets/Glowbom/Monetization/Resources/Data/Monetization.txt", false))
            {
                sw.Write(JsonUtility.ToJson(monetization));
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
