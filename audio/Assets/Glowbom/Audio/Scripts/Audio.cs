using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

/*
 * Created on Wed Apr 1 2020
 *
 * Copyright (c) 2020 Glowbom, Inc.
 */
public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioDataLoader audioDataLoader;
    public Dictionary<string, Sprite> sprites;
    public Dictionary<string, List<string>> imageKeys;
    public Image image;
    public Image background;
    public GameObject grid;
    public GameObject about;
    public GameObject front;
    public GameObject grid2;
    public Button playerButton;
    Monetization monetization = new Monetization();

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        audioDataLoader = new AudioDataLoader();
        audioDataLoader.audioData = new AudioData();
        audioDataLoader.audioData.sounds = new List<string>();
        audioDataLoader.audioData.keys = new List<string>();
        audioDataLoader.audioData.pictures = new List<string>();
        audioDataLoader.audioData.texts = new List<string>();
        audioDataLoader.audioData.textTimes = new List<string>();
        audioDataLoader.audioData.times = new List<string>();

#if UNITY_EDITOR
        // Find all Texture2Ds that have 'co' in their filename, that are labelled with 'architecture' and are placed in 'MyAwesomeProps' folder
        // to get all textures user t:texture2D
        string[] files = AssetDatabase.FindAssets("", new[] { "Assets/Glowbom/Audio/Resources" });

        foreach (string guid in files)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log(path);

            if (path.Contains(".mp3"))
            {
                audioDataLoader.audioData.sounds.Add(path);

                string[] times = path.Split('_');
                string[] parts = times[0].Split('/');
                audioDataLoader.audioData.keys.Add(parts[parts.Length - 1].Replace(".mp3", "").ToLower());
            } else if ((path.Contains(".jpg") || path.Contains(".png")) && !audioDataLoader.audioData.pictures.Contains(path) && path.Contains("_"))
            {
                string[] times = path.Split('_');
                audioDataLoader.audioData.times.Add(times[times.Length - 2] + "_" + times[times.Length - 1]
                                        .Replace(".jpg", "").Replace(".png", ""));
                audioDataLoader.audioData.pictures.Add(path);
            } else if (path.Contains(".txt") && path.Contains("_") && !audioDataLoader.audioData.texts.Contains(path) && path.Contains("_"))
            {
                string[] times = path.Split('_');
                audioDataLoader.audioData.textTimes.Add(times[times.Length - 2] + "_" + times[times.Length - 1]
                                        .Replace(".txt", ""));
                audioDataLoader.audioData.texts.Add(path);
            }
        }

        audioDataLoader.save();
#endif
        audioDataLoader.load();

        loadResources();

        monetization.initAds();

    }

    public void gridButtonClicked(string key)
    {
        grid.SetActive(false);
        play(key);
    }

    string currentKey;
    List<string> currentImages;
    int currentMin = 0;
    int currentSec = 0;
    int currentImageIndex = 0;

    private async void next()
    {
        //Debug.Log(currentMin + ":" + currentSec);
        if (currentImageIndex < currentImages.Count)
        {
            if (currentImageIndex < currentImages.Count - 1)
            {
                string timeKey = currentMin.ToString("00") + "_" + currentSec.ToString("00");
//                Debug.Log("timeKey: " + timeKey);
                int timeIndex = audioDataLoader.audioData.pictures.IndexOf(currentImages[currentImageIndex + 1]);

                if (timeKey.Equals(audioDataLoader.audioData.times[timeIndex]))
                {
                    ++currentImageIndex;
                }
            }

            image.sprite = sprites[currentImages[currentImageIndex]];
        }

        //background.transform.Rotate(Vector3.forward * -10);

        if (audioSource != null && audioSource.isPlaying)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            ++currentSec;
            if (currentSec >= 60)
            {
                currentSec = 0;
                ++currentMin;
            }
            if (!isNextProccessing)
            {
                StartCoroutine("next");
                isNextProccessing = true;
            }
        
        }
        else
        {
            //backToGrid();
            // TODO: implement non stop playing
            //playerNext();
        }

        isNextProccessing = false;
    }

    bool isNextProccessing = false;

    public void backToGrid()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        playerButton.image.sprite = playerSprite;

        grid.SetActive(true);

        monetization.tryShowAds();
    }

    public void getStarted()
    {
        grid.SetActive(true);
        front.SetActive(false);

        monetization.showBanner();
    }

    public void aboutPressed()
    {
        about.SetActive(true);
    }

    public void aboutBackPressed()
    {
        about.SetActive(false);
    }

    public void gridBackPressed()
    {
        front.SetActive(true);
    }

    public void gridNextPressed()
    {
        grid2.SetActive(true);
    }

    public void grid2BackPressed()
    {
        grid2.SetActive(false);
    }



    public void openYouTube()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCo1LM86YzjwzN9ubpucE0gQ");
    }

    public void playerPlayPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            playerButton.image.sprite = playerPauseSprite;
        } else
        {
            audioSource.Play();
            playerButton.image.sprite = playerSprite;
            StartCoroutine("next");
        }
    }

    public async void playerPrev()
    {
        audioSource.Pause();
        playerButton.image.sprite = playerPauseSprite;

        await Task.Delay(TimeSpan.FromSeconds(1));

        if (audioDataLoader.audioData.keys.Contains(currentKey))
        {
            int index = audioDataLoader.audioData.keys.IndexOf(currentKey);

            if (index == 0)
            {
                play(audioDataLoader.audioData.keys[audioDataLoader.audioData.keys.Count - 1]);
            } else
            {
                --index;
                play(audioDataLoader.audioData.keys[index]);
            }
        }
    }

    public async void playerNext()
    {
        audioSource.Pause();
        playerButton.image.sprite = playerPauseSprite;

        if (!monetization.tryShowAds())
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            if (audioDataLoader.audioData.keys.Contains(currentKey))
            {
                int index = audioDataLoader.audioData.keys.IndexOf(currentKey);

                if (index >= audioDataLoader.audioData.keys.Count - 1)
                {
                    play(audioDataLoader.audioData.keys[0]);
                }
                else
                {
                    ++index;
                    play(audioDataLoader.audioData.keys[index]);
                }
            }
        }
    }

    public void openInstagram()
    {
        Application.OpenURL("https://www.instagram.com/galurinia/?hl=ru");
    }

    public void openWebLink()
    {
        Application.OpenURL("https://opensea.io/assets/0x495f947276749ce646f68ac8c248420045cb7b5e/39722041514231892782231007733458041671678750369411936451569534866959160573953");
    }

    public void openOtherApps()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Ilins+House");
#elif UNITY_IPHONE
        Debug.Log("iPhone!");
        Application.OpenURL("https://www.google.com");
#else
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Ilins+House");
#endif

    }

    public void play(string key)
    {
        currentKey = key;
        currentImages = imageKeys[key];
        currentMin = 0;
        currentSec = 0;
        currentImageIndex = 0;

        AudioClip audioClip = Resources.Load<AudioClip>(key);
        audioSource.clip = audioClip;
        audioSource.Play();
        playerButton.image.sprite = playerSprite;

        StartCoroutine("next");
    }

    private Sprite playerSprite;
    private Sprite playerPauseSprite;

    private void loadResources()
    {
        sprites = new Dictionary<string, Sprite>();

        playerSprite = Resources.Load<Sprite>("Engine/player");
        playerPauseSprite = Resources.Load<Sprite>("Engine/player_pause");

        foreach (string path in audioDataLoader.audioData.pictures)
        {
            if (!sprites.ContainsKey(path))
            {
                string[] stringSeparators = { "Resources/" };
                string[] parts = path.Split(stringSeparators, StringSplitOptions.None);
                
                sprites.Add(path, Resources.Load<Sprite>(parts[parts.Length - 1].Replace(".png", "").Replace(".jpg", "")));

            }
        }

        imageKeys = new Dictionary<string, List<string>>();


        foreach (string key in audioDataLoader.audioData.keys)
        {
            if (!imageKeys.ContainsKey(key))
            {
                imageKeys.Add(key, new List<string>());
            }

            foreach (string path in audioDataLoader.audioData.pictures)
            {
                if (path.Contains(key))
                {
                    if (!imageKeys[key].Contains(path))
                    {
                        imageKeys[key].Add(path);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
