using UnityEngine;
using System.IO;

public class FeedbackLoader
{
    public Feedback feedback = null;

    public void load()
    {
        var textAsset = Resources.Load("Data/Feedback") as TextAsset;
        if (textAsset == null)
        {
            feedback = new Feedback();
        }
        else
        {
            feedback = JsonUtility.FromJson<Feedback>(textAsset.text);
        }
    }

    public void initialize()
    {
        if (feedback == null)
        {
            load();
        }
    }

    public void save()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter("Assets/Glowbom/Monetization/Resources/Data/Feedback.txt", false))
            {
                sw.Write(JsonUtility.ToJson(feedback));
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
