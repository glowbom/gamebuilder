#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class FeedbackEditor : EditorWindow
{
    static FeedbackLoader feedbackLoader = null;

    static FeedbackEditor()
    {
        feedbackLoader = new FeedbackLoader();
    }

    [MenuItem("Window/Glowbom/Feedback")]
    public static void ShowWindow()
    {
        GetWindow<FeedbackEditor>("Feedback");
    }

    private void OnGUI()
    {
        feedbackLoader.initialize();

        if (Forms.url != null && Forms.url != "")
        {
            feedbackLoader.feedback.names = new System.Collections.Generic.List<string>(Forms.names);
            feedbackLoader.feedback.entries = new System.Collections.Generic.List<string>(Forms.entries);
            feedbackLoader.feedback.values = new System.Collections.Generic.List<string>();
            foreach(string item in feedbackLoader.feedback.names)
            {
                feedbackLoader.feedback.values.Add("");
            }

            feedbackLoader.feedback.url = Forms.url;

            feedbackLoader.save();
        }


        GUILayout.Label("Glowbom", EditorStyles.boldLabel);

        GUILayout.Label("Feedback", EditorStyles.label);

        EditorGUILayout.Space();

        feedbackLoader.feedback.link = EditorGUILayout.TextField("Link", feedbackLoader.feedback.link);

        if (GUILayout.Button("Load Fields"))
        {
            Forms.forceLoad(GameStatusMagic.instance, feedbackLoader.feedback.link);
            feedbackLoader.save();
        }

        EditorGUILayout.Space();

        if (feedbackLoader.feedback.names != null)
        {
            for (int i = 0; i < feedbackLoader.feedback.names.Count; i++)
            {
                feedbackLoader.feedback.values[i] = EditorGUILayout.TextField(feedbackLoader.feedback.names[i], feedbackLoader.feedback.values[i]);
            }
        }

        //feedbackLoader.feedback.url = EditorGUILayout.TextField("URL", feedbackLoader.feedback.url);
    }
}
#endif
