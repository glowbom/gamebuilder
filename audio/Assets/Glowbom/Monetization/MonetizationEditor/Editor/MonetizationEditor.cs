#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonetizationEditor : EditorWindow
{
    static MonetizationLoader monetizationLoader = null;

    static MonetizationEditor()
    {
        monetizationLoader = new MonetizationLoader();

    }

    [MenuItem("Window/Glowbom/Monetization")]
    public static void ShowWindow()
    {
        GetWindow<MonetizationEditor>("Monetization");
    }

    private void OnGUI()
    {
        monetizationLoader.initialize();

        GUILayout.Label("Glowbom", EditorStyles.boldLabel);

        GUILayout.Label("Monetization", EditorStyles.label);

        EditorGUILayout.Space();

        if (GUILayout.Button("Save"))
        {
            monetizationLoader.save();
        }

        EditorGUILayout.Space();

        monetizationLoader.monetization.androidAppId = EditorGUILayout.TextField("Android App Id", monetizationLoader.monetization.androidAppId);
        monetizationLoader.monetization.androidBanner = EditorGUILayout.TextField("Android Banner", monetizationLoader.monetization.androidBanner);
        monetizationLoader.monetization.androidInterstitial = EditorGUILayout.TextField("Android Interstitial", monetizationLoader.monetization.androidInterstitial);
        monetizationLoader.monetization.iOSAppId = EditorGUILayout.TextField("iOS App Id", monetizationLoader.monetization.iOSAppId);
        monetizationLoader.monetization.iOSBanner = EditorGUILayout.TextField("iOS Banner", monetizationLoader.monetization.iOSBanner);
        monetizationLoader.monetization.iOSInterstitial = EditorGUILayout.TextField("iOS Interstitial", monetizationLoader.monetization.iOSInterstitial);
    }
}
#endif
