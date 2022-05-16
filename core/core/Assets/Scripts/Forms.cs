using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/*
 * Created on Sun Aug 19 2019
 *
 * Copyright (c) 2019 Glowbom, Inc.
 */
public class Forms
{
    public static GridStatusScript ui;

	public static List<string> names;
    public static List<string> entries;
    public static List<string> values;
    public static string url;
    static FeedbackLoader feedbackLoader = null;

	public static void load(MonoBehaviour monoBehaviour, string formUrl)
	{
        //if (Application.platform == RuntimePlatform.WebGLPlayer)
        //{
        /*names = new List<string>();
        names.Add("XXX");
        names.Add("YYY");
        names.Add("ZZZ");
        names.Add("AAA");

        entries = new List<string>();
        entries.Add("entry1");
        entries.Add("entry2");
        entries.Add("entry3");
        entries.Add("entry4");

        url = "response_url";*/

        if (feedbackLoader == null)
        {
            feedbackLoader = new FeedbackLoader();
            feedbackLoader.initialize();

            if (feedbackLoader.feedback.url != null && feedbackLoader.feedback.url != "")
            {
                names = new List<string>(feedbackLoader.feedback.names);
                entries = new List<string>(feedbackLoader.feedback.entries);
                url = feedbackLoader.feedback.url;
            }
        }

        if (url != null && url != "")
        {
            if (ui != null && ui.inputFields != null)
            {
                ui.gameViewText.text = "";
                for (int j = 0; j < names.Count - 1; j++)
                {
                    if (j < ui.inputFields.Length)
                    {
                        ui.inputFields[j].gameObject.SetActive(true);
                        ui.inputFields[j].placeholder.GetComponent<Text>().text = names[j];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        } else
        {
            forceLoad(monoBehaviour, formUrl);
        }
    }

    public static void forceLoad(MonoBehaviour monoBehaviour, string formUrl)
    {
        if (formUrl != "")
        {
            monoBehaviour.StartCoroutine(get(formUrl));
        }
    }

    public static void submit(MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(post());
    }

    static IEnumerator post()
    {
        WWWForm form = new WWWForm();

        for(int i = 0; i < values.Count; i++)
        {
            form.AddField(entries[i], values[i]);
        }

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
    }

    static IEnumerator get(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                string pageContent = webRequest.downloadHandler.text;
                if (pageContent.Contains("form action"))
                {
                    int formActionPosition = pageContent.IndexOf("form action");
                    if (formActionPosition != -1)
                    {
                        bool foundStart = false;
                        string formActionValue = "";
                        for(int i = formActionPosition; i < pageContent.Length; i++)
                        {
                            if (foundStart)
                            {
                                if (pageContent[i] == '"')
                                {
                                    break;
                                } else
                                {
                                    formActionValue += pageContent[i];
                                }
                            } else
                            {
                                if (pageContent[i] == '"')
                                {
                                    foundStart = true;
                                }
                            }
                        }

                        Debug.Log("link: " + formActionValue);
                        url = formActionValue;
                    }
                }

                if (pageContent.Contains("aria-label"))
                {
                    names = new List<string>();
                    int index = 0;
                    while (index != -1)
                    {
                        int areaLabelPosition = pageContent.IndexOf("aria-label", index);
                        if (areaLabelPosition != -1)
                        {
                            index = areaLabelPosition;
                            bool foundStart = false;
                            string value = "";
                            for (int i = areaLabelPosition; i < pageContent.Length; i++)
                            {
                                ++index;
                                if (foundStart)
                                {
                                    if (pageContent[i] == '"')
                                    {
                                        if (pageContent.Substring(i + 2, 13) == "aria-disabled")
                                        {
                                            value = "";
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        value += pageContent[i];
                                    }
                                }
                                else
                                {
                                    if (pageContent[i] == '"')
                                    {
                                        foundStart = true;
                                    }
                                }
                            }

                            if (value != "")
                            {
                                Debug.Log("name: " + value);
                                names.Add(value);
                            }
                        }
                        else
                        {
                            index = -1;
                        }
                    } 
                }

                if (ui != null && ui.inputFields != null)
                {
                    ui.gameViewText.text = "";
                    for (int j = 0; j < names.Count - 1; j++)
                    {
                        if (j < ui.inputFields.Length)
                        {
                            ui.inputFields[j].gameObject.SetActive(true);
                            ui.inputFields[j].placeholder.GetComponent<Text>().text = names[j];
                        } else
                        {
                            break;
                        }
                    }
                }

                if (pageContent.Contains("entry."))
                {
                    entries = new List<string>();
                    int index = 0;
                    while (index != -1)
                    {
                        int position = pageContent.IndexOf("entry.", index);
                        if (position != -1)
                        {
                            index = position;
                            string value = "";
                            for (int i = position; i < pageContent.Length; i++)
                            {
                                ++index;
                                if (pageContent[i] == '"')
                                {
                                    break;
                                }
                                else
                                {
                                    value += pageContent[i];
                                }
                            }

                            Debug.Log("entry: " + value);
                            entries.Add(value);
                        }
                        else
                        {
                            index = -1;
                        }
                    }
                }

#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
        }
    }


}
