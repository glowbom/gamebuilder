using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using TMPro;

/*
 * Created on Sun Jul 21 2019
 *
 * Copyright (c) 2019 Glowbom.
 */
[System.Serializable]
public class Logic
{
    [System.Serializable]
    public class Item
    {
        public string title;
        public string description;
        public string[] buttonsTexts;
        public string[] picturesSpriteNames;
        public int[] goIndexes;
        public int[] goConditions;
        public int[] heroValues;
        public int[] buttonAnswers;

        public Vector2 mainImagePosition;
        public Vector2 mainImageSize;

        public int answersCount;
        public string answerPicture;
        public int answerPictureDelay = 4;
    }

    public string[] heroElements;
    public int[] heroValues;
    public int currentItemIndex = 0;
    public Item[] questions;
    public int deadValue;
    public int deadLevel;
    public int deadItemIndex;
    public bool pleaseRestart = false;
    public string answers = "";
    public string backgroundPicture;
    public string link;
    public string nft;

    public int getAnswersCount()
    {
        return questions[currentItemIndex].answersCount;
    }

    public int getTotalQuestionsCount()
    {
        int totalQuestions = 0;
        for (int i = 0; i < questions.Length; i++)
        {
            if (questions[i].answersCount > 0)
            {
                ++totalQuestions;
            }
        }

        return totalQuestions;
    }

    public bool isSupportAnswers()
    {
        return questions[currentItemIndex].answersCount > 0;
    }

    public bool hasMultipleAnswers()
    {
        bool hasMultiple = false;
        int answers = 0;

        if (questions[currentItemIndex] != null)
        {
            for (int i = 0; i < questions[currentItemIndex].buttonAnswers.Length; i++)
            {
                if (questions[currentItemIndex].buttonAnswers[i] > 0)
                {
                    ++answers;
                    if (answers > 1)
                    {
                        hasMultiple = true;
                        break;
                    }
                }
            }
        }

        return hasMultiple;
    }

    public int getButtonAnswer(int i)
    {
        return questions[currentItemIndex].buttonAnswers != null &&
            questions[currentItemIndex].buttonAnswers != null &&
            i > -1 && i < questions[currentItemIndex].buttonAnswers.Length ?
            questions[currentItemIndex].buttonAnswers[i] : 0;
    }

    public int getAnswerPictureDelay()
    {
        return questions[currentItemIndex].answerPictureDelay;
    }

    public string getAnswerPicture()
    {
        return questions[currentItemIndex].answerPicture;
    }

    public bool isSharingButton(int i)
    {
        if (currentItemIndex < questions.Length)
        {
            Item item = questions[currentItemIndex];

            if (item.goIndexes != null && i > -1 && i < item.goIndexes.Length)
            {
                return item.goIndexes[i] == 10001;
            }
        }

        return false;
    }

    public bool isAskFriendButton(int i)
    {

        Item item = questions[currentItemIndex];

        if (item.goIndexes != null && i > -1 && i < item.goIndexes.Length)
        {
            return item.goIndexes[i] == 10002;
        }

        return false;
    }

    public bool isGoBackButton(int i)
    {
        Item item = questions[currentItemIndex];

        if (item.goIndexes != null && i > -1 && i < item.goIndexes.Length)
        {
            return item.goIndexes[i] == 10003;
        }

        return false;
    }

    public string getTextToShare()
    {
        return questions[currentItemIndex].description;
    }

    public string getAskFriendTextToShare()
    {
        string a = "";
        int i = 0;
        foreach (string s in questions[currentItemIndex].buttonsTexts)
        {
            if (questions[currentItemIndex].goIndexes[i] == 10001 || questions[currentItemIndex].goIndexes[i] == 10002)
            {
                ++i;
                continue;
            }

            a += s + ", ";
            ++i;
        }

        if (a != "")
        {
            a = a.Substring(0, a.Length - 2);
        }

        return questions[currentItemIndex].description + " Answers: " + a;
    }


    public bool isCorrectAnswer(int i)
    {
        Item item = questions[currentItemIndex];

        if (item.buttonAnswers != null && i > -1 && i < item.buttonAnswers.Length)
        {
            return item.answersCount != 0 && item.buttonAnswers[i] > 0;
        }

        return false;
    }

    public Item nextItem(int i)
    {
        Item item = questions[currentItemIndex];

        if (i > -1 && i < item.goIndexes.Length)
        {
            currentItemIndex = item.goIndexes[i];

            if (answers == "")
            {
                answers = item.buttonsTexts[i];
            }
            else
            {
                answers += (", " + item.buttonsTexts[i]);
            }

            if (currentItemIndex < questions.Length)
            {
                Item ni = questions[currentItemIndex];

                if (ni.heroValues != null)
                {
                    for (int j = 0; j < ni.heroValues.Length; j++)
                    {
                        heroValues[j] += ni.heroValues[j];
                    }
                }

                /*if (deadValue > -1 && deadValue < heroValues.Length) {
                    if (heroValues [deadValue] <= deadLevel) {
                        currentItemIndex = deadItemIndex;
                        nextItem = questions [currentItemIndex];
                        pleaseRestart = true;
                    }
                }
                */

                return ni;
            }

        }

        return null;
    }
}

[System.Serializable]
public class Buttons
{
    [System.Serializable]
    public class Button
    {
        public string name;
        public string image;
        public string link;
        public string nft;
        public int score;
        public int totalQuestionsCount;
    }

    public Button[] buttons;

    public string title;
    public string secondTitle;
    public string aboutText;

    public string playButton;
    public string aboutButton;
    public string mainMenuButton;

    public string frontImage;
    public string gridImage;
    public string backImage;
}

[System.Serializable]
public class Default
{
    public string lastUsedName;
}

// networking

public class Quiz : MonoBehaviour
{
    // correct answer color #37B392
    // incorrect answer color #B24437

    public Sharing sharing;
    //public Monetization monetization;
    public InputField clipboard;

    public GameObject editButtonPanel;
    public InputField editTitleButtonField;

    public GameObject editView;
    public InputField editTitleField;
    public InputField editTextField;
    public Image quitView;
    public Image scrollView;
    public Text gameViewTitle;
    public TextMeshProUGUI gameViewText;
    public Text gameViewHeroStatusText;
    public Button[] buttons;
    public InputField[] inputFields;

    public Button[] gridButtons;
    public Image[] pictures;
    public Image backgroundImage;
    public Logic logic = null;

    public Text startButtonText;

    public Buttons buttonsLogic = null;

    public Image front;

    public Image gridBackground;

    public GameObject about;

    public GameObject gridButtonsPanel;

    public Text status;

    Dictionary<string, string> answers = new Dictionary<string, string>();

    public static void trackEvent(string category, string action)
    {

    }

    private static void resetDraggablePanelPosition(Transform view)
    {

    }

    public PlayAudio playAudio;
    private int answersCollected = 0;
    public int correctAnswers = 0;
    public int totalQuestionsCount = 0;

    //Moralis.MoralisClient moralis = null;

    public void procced()
    {
        if (logic != null)
        {
            answersCollected = 0;

            if (logic.currentItemIndex > -1 && logic.currentItemIndex < logic.questions.Length)
            {

                //if (monetization != null)
                //{
                //    monetization.tryShowAds();
                //}

                if (scrollView != null)
                {
                    resetDraggablePanelPosition(scrollView.gameObject.transform);
                }
                

                Logic.Item item = logic.questions[logic.currentItemIndex];
                if (gameViewTitle != null)
                {
                    gameViewTitle.text = item.title;
                }
                    

                //trackEvent("Book", item.title);

                string statusString = "";
                int index = 0;

                if (logic.heroElements != null)
                {
                    foreach (string key in logic.heroElements)
                    {
                        statusString += string.Format("{0} = {1},  ", key, logic.heroValues[index]);
                        ++index;
                    }
                }

                if (statusString.Length > 2)
                {
                    statusString = statusString.Remove(statusString.Length - 2, 2);
                }

                if (gameViewHeroStatusText != null)
                {
                    gameViewHeroStatusText.text = statusString;
                }




                if (item.description.Contains("[newline]"))
                {
                    item.description = item.description.Replace("[newline]", "\n");
                }

                if (item.description.Contains("[correctAnswers]"))
                {
                    item.description = item.description.Replace("[correctAnswers]", correctAnswers.ToString());

                    if (gridButtonsPanel != null && lastClickedGridButtonIndex >= 0
                        && correctAnswers > buttonsLogic.buttons[lastClickedGridButtonIndex].score)
                    {
                        buttonsLogic.buttons[lastClickedGridButtonIndex].score = correctAnswers;
                    }
                }

                if (item.description.Contains("[totalQuestionsCount]"))
                {
                    totalQuestionsCount = logic.getTotalQuestionsCount();
                    item.description = item.description.Replace("[totalQuestionsCount]", totalQuestionsCount.ToString());

                    if (gridButtonsPanel != null && lastClickedGridButtonIndex >= 0)
                    {
                        buttonsLogic.buttons[lastClickedGridButtonIndex].totalQuestionsCount = totalQuestionsCount;
                        saveButtonsLogic();
                    }
                }

                if (item.description.Contains("{question"))
                {
                    foreach (string key in answers.Keys)
                    {
                        //Debug.Log("key = " + key + "; value = " + answers[key]);
                        if (item.description.Contains("{" + key + "}"))
                        {
                            item.description = item.description.Replace("{" + key + "}", answers[key]);
                        }
                    }
                }

                for (int heroValueIndex = 0; heroValueIndex < logic.heroElements.Length; heroValueIndex++)
                {
                    string element = logic.heroElements[heroValueIndex];
                    if (item.description.Contains("{" + element))
                    {
                        item.description = item.description.Replace("{" + element + "}", logic.heroValues[heroValueIndex].ToString());
                    }
                }

                if (gameViewText != null)
                {
                    gameViewText.text = item.description;
                }
                

                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                    buttons[i].image.color = new Color32(255, 255, 255, 255); // 255 for non-transparent
                    //buttons[i].image.sprite = sprites["default"];
                }

                for (int i = 0; i < item.buttonsTexts.Length; i++)
                {
                    if (i < buttons.Length)
                    {
                        Button bs = buttons[i];
                        if (item.buttonsTexts[i].Contains("{question"))
                        {
                            foreach (string key in answers.Keys)
                            {
                                if (item.buttonsTexts[i].Contains("{" + key + "}"))
                                {
                                    item.buttonsTexts[i] = item.buttonsTexts[i].Replace("{" + key + "}", answers[key]);
                                }
                            }
                        }

                        for (int heroValueIndex = 0; heroValueIndex < logic.heroElements.Length; heroValueIndex++)
                        {
                            string element = logic.heroElements[heroValueIndex];
                            if (item.buttonsTexts[i].Contains("{" + element))
                            {
                                item.buttonsTexts[i] = item.buttonsTexts[i].Replace("{" + element + "}", logic.heroValues[heroValueIndex].ToString());
                            }
                        }

                        bs.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = item.buttonsTexts[i];

                        buttons[i].enabled = item.goIndexes[i] != -1;
                        buttons[i].gameObject.SetActive(item.goIndexes[i] != -1);
                    }
                }

                if (item.goConditions != null && item.goConditions.Length > 0)
                {
                    bool conditionOk = true;
                    for (int i = 0; i < item.goConditions.Length; i++)
                    {
                        if (item.goConditions[i] >= logic.heroValues[i])
                        {
                            conditionOk = false;
                            break;
                        }
                    }

                    if (buttons[buttons.Length - 1].enabled)
                    {
                        buttons[buttons.Length - 1].gameObject.SetActive(conditionOk);
                    }
                }

                if (item.picturesSpriteNames != null)
                {
                    
                    if (pictures != null && pictures.Length > 0 && pictures[0] != null)
                    {
                        pictures[0].gameObject.SetActive(false);
                    }
                   
                    if (item.picturesSpriteNames.Length != 0)
                    {
                        for (int i = 0; i < item.picturesSpriteNames.Length; i++)
                        {
                            if (i < pictures.Length)
                            {
                                if (sprites.ContainsKey(item.picturesSpriteNames[i]))
                                {
                                    pictures[i].sprite = sprites[item.picturesSpriteNames[i]];
                                    pictures[i].gameObject.SetActive(!item.picturesSpriteNames[i].Equals(string.Empty));
                                }

                            }
                        }
                    }
                }
                else
                {
                    pictures[0].gameObject.SetActive(false);
                }


                // Online Feedback Form
                if (item.title == "Form" && item.description.Contains("http"))
                {
                    for (int i = 0; i < inputFields.Length; i++)
                    {
                        inputFields[i].text = "";
                        inputFields[i].gameObject.SetActive(false);
                    }

                    Forms.ui = this;
                    gameViewText.text = "Loading...";
                    gameViewTitle.text = "";
                    Forms.load(this, item.description);
                }

            }
        }
    }

    public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    // Use this for initialization
    void Start()
    {
        //if (monetization != null)
        //{
        //    monetization.initAds();
        //}

        sprites.Clear();
        Sprite sprite = Resources.Load("Textures/default", typeof(Sprite)) as Sprite;
        sprites.Add("default", sprite);

        GameStatusMagic.instance.load();

        loadButtonsLogic();

        load();

        procced();

        refreshGridButtons();
    }

    public void refreshGridButtons()
    {
        for (int i = 0; i < gridButtons.Length; i++)
        {
            gridButtons[i].gameObject.SetActive(false);
        }

        if (buttonsLogic != null && buttonsLogic.buttons != null)
        {
            for (int i = 0; i < buttonsLogic.buttons.Length; i++)
            {
                if (i < gridButtons.Length)
                {
                    Button bs = gridButtons[i];
                    bs.transform.Find("Text").GetComponent<Text>().text = buttonsLogic.buttons[i].name;
                    gridButtons[i].gameObject.SetActive(true);


                    if (buttonsLogic.buttons[i].image != null && buttonsLogic.buttons[i].image != "")
                    {
                        if (sprites.ContainsKey(buttonsLogic.buttons[i].image))
                        {
                            bs.GetComponent<Image>().sprite = sprites[buttonsLogic.buttons[i].image];
                            bs.transform.Find("Text").GetComponent<Text>().text = "";

                            if (buttonsLogic.buttons[i].score != 0)
                            {
                                bs.transform.Find("Text").GetComponent<Text>().text = buttonsLogic.buttons[i].score.ToString()
                                    + " of " + buttonsLogic.buttons[i].totalQuestionsCount.ToString();
                            }
                        }
                    }
                    else
                    {
                        bs.GetComponent<Image>().sprite = null;
                    }
                }
            }
        }
    }

    public void backPressed()
    {
        logic.currentItemIndex = 0;
        load();
        procced();

        refreshGridButtons();
        front.gameObject.SetActive(true);
    }

    public void aboutPressed()
    {
        about.SetActive(true);
    }

    public void aboutBackPressed()
    {
        about.SetActive(false);
    }

    public void openGlowbomLink()
    {
        Application.OpenURL("https://glowbom.com/");
    }

    public void openAboutLink()
    {

    }


    public void openPressed()
    {
        GameStatusMagic.instance.questAnswers.Clear();
        load();


        front.gameObject.SetActive(false);

        if (gridButtonsPanel != null && buttonsLogic != null && buttonsLogic.buttons != null && buttonsLogic.buttons.Length > 1)
        {
            gridButtonsPanel.gameObject.SetActive(true);
            gridBackground.gameObject.SetActive(true);
        }

        //moralis = new Moralis.MoralisClient(new Moralis.Platform.ServerConnectionData() { ApplicationID = "tExaeI71lL2dacD127nXbAYUWXbz7TWfzZqp82yy", ServerURI = "https://r93uxdhi97sw.usemoralis.com:2053/server" }, new Moralis.Web3Api.Client.Web3ApiClient());
    }

    private int buttonPressedCounter = 0;

    private string lastClickedLink = null;
    private int lastClickedGridButtonIndex = -1;

    public void buttonGridPressed(GameObject button)
    {
        int i = 0;

        foreach (Button b in gridButtons)
        {
            if (b.gameObject == button)
            {
                lastClickedGridButtonIndex = i;
                lastClickedLink = buttonsLogic.buttons[i].link;

                if (buttonsLogic.buttons[i].nft != "")
                {
                    Application.OpenURL(buttonsLogic.buttons[i].nft);
                }

                load();
                procced();


                if (backgroundImage != null && logic.backgroundPicture != null && logic.backgroundPicture != "")
                {
                    if (sprites.ContainsKey(logic.backgroundPicture))
                    {
                        backgroundImage.sprite = sprites[logic.backgroundPicture];
                    }
                }


                //logic.nextItem (i);
                //procced ();
                break;
            }

            ++i;
        }

        gridButtonsPanel.gameObject.SetActive(false);
        gridBackground.gameObject.SetActive(false);
    }

    public void backButtonMenuPressed(GameObject button)
    {
        front.gameObject.SetActive(true);
    }

    public void backButtonGamePressed(GameObject button)
    {
        gridButtonsPanel.gameObject.SetActive(true);
        gridBackground.gameObject.SetActive(true);
    }

    public async void buttonPressed(int i)
    {
        ++buttonPressedCounter;
        if (buttonPressedCounter % 25 == 0)
        {
            //Advertisement.Show();
        }

        if (inputFields != null && inputFields.Length > 0 && inputFields[0].IsActive())
        {
            Forms.values = new List<string>();
            for (int j = 0; j < inputFields.Length; j++)
            {
                if (inputFields[j].IsActive())
                {
                    if (inputFields[j].text == "")
                    {
                        // don't allow to submit until it has something
                        return;
                    }

                    Forms.values.Add(inputFields[j].text);
                }
            }

            if (logic.answers != "")
            {
                Forms.values.Add(logic.answers);
            }

            Forms.submit(this);

            for (int k = 0; k < inputFields.Length; k++)
            {
                inputFields[k].gameObject.SetActive(false);
            }

        }

        if (logic.pleaseRestart)
        {
            load();
            procced();
            return;
        }

        if (logic.isSharingButton(i) && sharing != null)
        {
            sharing.shareMessage(logic.getTextToShare(), logic.link != null && logic.link != "" ? logic.link : "glowbom.com");
            return;
        }

        if (logic.isAskFriendButton(i) && sharing != null)
        {
            sharing.shareMessage(logic.getAskFriendTextToShare(), logic.link != null && logic.link != "" ? logic.link : "glowbom.com");
            return;
        }

        if (logic.isGoBackButton(i))
        {
            backPressed();
            return;
        }

        if (logic.isSupportAnswers())
        {

            if (logic.isCorrectAnswer(i))
            {
                buttons[i].image.color = new Color32(55, 179, 46, 255);

                if (playAudio != null)
                {
                    playAudio.correct();
                }


                if (logic.hasMultipleAnswers())
                {
                    answersCollected += logic.getButtonAnswer(i);
                    if (answersCollected < logic.getAnswersCount())
                    {
                        return;
                    }
                }

                ++correctAnswers;

                if (logic.getAnswerPicture() != null && logic.getAnswerPicture() != "")
                {
                    pictures[0].sprite = sprites[logic.getAnswerPicture()];
                    pictures[0].gameObject.SetActive(true);

                    if (logic.getAnswerPictureDelay() == 0)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(4));
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(logic.getAnswerPictureDelay() - 1));
                    }


                }
            }
            else
            {
                buttons[i].image.color = new Color32(178, 68, 55, 255);
                if (playAudio != null)
                {
                    playAudio.incorrect();
                }
                highlightCorrectAnswerIfNeeded();
            }



            await Task.Delay(TimeSpan.FromSeconds(1));
        }




        logic.nextItem(i);
        procced();
    }

    private bool isShowCorrectAnswer = true;

    private void highlightCorrectAnswerIfNeeded()
    {
        if (isShowCorrectAnswer)
        {
            for (int j = 0; j < buttons.Length; j++)
            {
                if (logic.isCorrectAnswer(j))
                {
                    buttons[j].image.color = new Color32(55, 179, 46, 255);
                }
            }
        }
    }



    private int shift = 0;
    private int currentOpenedButtonIndex = 0;

    public void updateOrCreateQuestIfPossible()
    {
        if (GameStatusMagic.instance.user != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitView.gameObject.SetActive(true);
        }
    }

    public void save(string path)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(lastClickedLink != null && lastClickedLink != "" ?
                path.Replace("TemplateQuest", lastClickedLink) : path, false))
            {
                sw.Write(JsonUtility.ToJson(logic));
            }

            Debug.Log("Saved: " + path);
        }
        catch (IOException)
        {
        }
    }

    public void saveAs(string newLink, string oldLink)
    {
        if (lastUsedFileName != null)
        {
            save(lastUsedFileName.Replace(oldLink, newLink));
        }
    }

    public void save()
    {
        if (lastUsedFileName != null)
        {
            save(lastUsedFileName);
            return;
        }

        try
        {
            using (StreamWriter sw = new StreamWriter("Assets/Glowbom/Quests/Resources/Data/" + QuestLoader.name + QuestLoader.language + ".txt", false))
            {
                sw.Write(JsonUtility.ToJson(logic));
            }
        }
        catch (IOException)
        {
        }
    }

    public void saveButtonsLogic(string path)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(JsonUtility.ToJson(buttonsLogic));
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void saveButtonsLogic()
    {
        if (lastUsedButtonsFileName != null)
        {
            saveButtonsLogic(lastUsedButtonsFileName);
            return;
        }

        try
        {
            using (StreamWriter sw = new StreamWriter("Assets/Glowbom/Quests/Resources/Data/Buttons" + QuestLoader.language + ".txt", false))
            {
                sw.Write(JsonUtility.ToJson(buttonsLogic));
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
    }

    private string lastUsedFileName = null;

    public void loadFromFile(string filename, bool storeFilename)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                load(reader.ReadToEnd());
                if (storeFilename)
                {
                    lastUsedFileName = filename;
                }
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }

        loadResources();
    }

    public void loadFromFile(string filename)
    {
        loadFromFile(filename, true);
    }

    public void load()
    {
        if (lastUsedFileName != null)
        {
            String link = lastUsedFileName;
            if (lastClickedLink != null && lastClickedLink != "")
            {
                link = link.Replace("TemplateQuest", lastClickedLink);
            }

            loadFromFile(link, false);
            return;
        }

        var textAsset = Resources.Load("Data/" + QuestLoader.name + QuestLoader.language) as TextAsset;

        if (lastClickedLink != null)
        {
            textAsset = Resources.Load("Data/" + lastClickedLink) as TextAsset;
        }

        load(textAsset.text);
    }

    private Sprite loadSpriteFromFile(string path)
    {

        if (string.IsNullOrEmpty(path)) return null;

        if (!path.Contains(".png") && !path.Contains(".jpg"))
        {
            path += ".png";
        }

        if (File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            return sprite;
        }
        else
        {
            path = path.Replace(".png", ".jpg");
            if (File.Exists(path))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
        }
        return null;
    }

    private void load(String data)
    {
        if (editButton != null)
        {
            editButton.gameObject.SetActive(true);
        }
        

        logic = JsonUtility.FromJson<Logic>(data);
        logic.answers = "";
        correctAnswers = 0;

        loadResources();
    }

    public void cleanupSprites()
    {
        sprites.Clear();
        Resources.UnloadUnusedAssets();
    }

    public string getTextureImagesPath(int index, string name, string extention)
    {
        string spritesLink = null;
        if (lastUsedFileName != null)
        {
            String link = lastUsedFileName;
            if (lastClickedLink != null && lastClickedLink != "")
            {
                spritesLink = link.Replace("Data/TemplateQuest.txt", "Textures/");
            }
        }

        return spritesLink == null ? "Textures/images/" + name + index + extention :
                        spritesLink + "images/" + name + index + extention;
    }

    public string getTexturesPath(string name, string extention)
    {
        string spritesLink = null;
        if (lastUsedFileName != null)
        {
            String link = lastUsedFileName;
            if (lastClickedLink != null && lastClickedLink != "")
            {
                spritesLink = link.Replace("Data/TemplateQuest.txt", "Textures/");
            }
        }

        return spritesLink == null ? "Textures/" + name + extention :
                        spritesLink + name + extention;
    }

    public void reloadResource(string key, string path)
    {
        Debug.Log("reloading " + path);
        Sprite sprite = loadSpriteFromFile(path);
        if (sprite != null)
        {
            if (sprites.ContainsKey(key))
            {
                Debug.Log("removing sprite: " + key);
                sprites.Remove(key);
            }

            sprites.Add(key, sprite);
        }
    }

    private void loadResources()
    {
        string spritesLink = null;
        if (lastUsedFileName != null)
        {
            String link = lastUsedFileName;
            if (lastClickedLink != null && lastClickedLink != "")
            {
                spritesLink = link.Replace("Data/TemplateQuest.txt", "Textures/");
            }
        }


        // load pics
        Sprite sprite = null;
        foreach (var item in logic.questions)
        {
            if (item.answerPicture != null && item.answerPicture != "")
            {

                if (!sprites.ContainsKey(item.answerPicture))
                {
                    sprite = spritesLink == null ? Resources.Load("Textures/images/" + item.answerPicture, typeof(Sprite)) as Sprite :
                        loadSpriteFromFile(spritesLink + "images/" + item.answerPicture);
                    if (sprite != null)
                    {
                        sprites.Add(item.answerPicture, sprite);
                    }
                }
            }

            if (item.picturesSpriteNames != null && item.picturesSpriteNames.Length != 0)
            {
                for (int i = 0; i < item.picturesSpriteNames.Length; i++)
                {
                    if (i < pictures.Length)
                    {
                        string key = item.picturesSpriteNames[i];
                        if (!sprites.ContainsKey(key))
                        {
                            sprite = spritesLink == null ? Resources.Load("Textures/images/" + key, typeof(Sprite)) as Sprite :
                                loadSpriteFromFile(spritesLink + "images/" + key);
                            if (sprite != null)
                            {
                                sprites.Add(key, sprite);
                            }
                        }

                    }
                }
            }
        }

        if (buttonsLogic != null)
        {
            foreach (var b in buttonsLogic.buttons)
            {
                if (b.image != null && b.image != "")
                {
                    string key = b.image;
                    if (!sprites.ContainsKey(key))
                    {
                        sprite = spritesLink == null ? Resources.Load("Textures/" + key, typeof(Sprite)) as Sprite :
                            loadSpriteFromFile(spritesLink + key);
                        if (sprite != null)
                        {
                            sprites.Add(key, sprite);
                        }
                    }
                }
            }
        }

        if (logic.backgroundPicture != null && logic.backgroundPicture != "")
        {
            string key = logic.backgroundPicture;
            if (!sprites.ContainsKey(key))
            {
                sprite = spritesLink == null ? Resources.Load("Textures/" + key, typeof(Sprite)) as Sprite :
                    loadSpriteFromFile(spritesLink + key);
                if (sprite != null)
                {
                    sprites.Add(key, sprite);
                }
            }
        }

    }

    private string lastUsedButtonsFileName = null;

    public void loadButtonsLogicFromFile(string filename)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                buttonsLogic = JsonUtility.FromJson<Buttons>(reader.ReadToEnd());
                lastUsedButtonsFileName = filename;
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void loadButtonsLogic()
    {
        if (lastUsedButtonsFileName != null)
        {
            loadButtonsLogicFromFile(lastUsedButtonsFileName);
            return;
        }

        var textAsset = Resources.Load("Data/Buttons") as TextAsset;
        buttonsLogic = JsonUtility.FromJson<Buttons>(textAsset.text);
    }


    public Text title;
    public Text secondTitle;
    public Text aboutText;

    public Text playButton;
    public Text aboutButton;
    public Text mainMenuButton;

    public void updateFrontUi()
    {
        if (title != null)
        {
            if (buttonsLogic.title == null || buttonsLogic.title == "")
            {
                buttonsLogic.title = title.text;
                saveButtonsLogic();
            }
            else
            {
                title.text = buttonsLogic.title;
            }
        }

        if (secondTitle != null)
        {
            if (buttonsLogic.secondTitle == null || buttonsLogic.secondTitle == "")
            {
                buttonsLogic.secondTitle = secondTitle.text;
                saveButtonsLogic();
            }
            else
            {
                secondTitle.text = buttonsLogic.secondTitle;
            }
        }

        if (aboutText != null)
        {
            if (buttonsLogic.aboutText == null || buttonsLogic.aboutText == "")
            {
                buttonsLogic.aboutText = aboutText.text;
                saveButtonsLogic();
            }
            else
            {
                aboutText.text = buttonsLogic.aboutText;
            }
        }

        if (playButton != null)
        {
            if (buttonsLogic.playButton == null || buttonsLogic.playButton == "")
            {
                buttonsLogic.playButton = playButton.text;
                saveButtonsLogic();
            }
            else
            {
                playButton.text = buttonsLogic.playButton;
            }
        }

        if (aboutButton != null)
        {
            if (buttonsLogic.aboutButton == null || buttonsLogic.aboutButton == "")
            {
                buttonsLogic.aboutButton = aboutButton.text;
                saveButtonsLogic();
            }
            else
            {
                aboutButton.text = buttonsLogic.aboutButton;
            }
        }

        if (mainMenuButton != null)
        {
            if (buttonsLogic.mainMenuButton == null || buttonsLogic.mainMenuButton == "")
            {
                buttonsLogic.mainMenuButton = mainMenuButton.text;
                saveButtonsLogic();
            }
            else
            {
                mainMenuButton.text = buttonsLogic.mainMenuButton;
            }
        }

        if (buttonsLogic.frontImage != null)
        {
            if (sprites.ContainsKey(buttonsLogic.frontImage))
            {
                front.sprite = sprites[buttonsLogic.frontImage];
            }
        }
        else
        {
            buttonsLogic.frontImage = front.sprite.name;
            saveButtonsLogic();
        }


        if (buttonsLogic.gridImage != null)
        {
            if (sprites.ContainsKey(buttonsLogic.gridImage))
            {
                gridBackground.sprite = sprites[buttonsLogic.gridImage];
            }
        }
        else
        {
            buttonsLogic.gridImage = gridBackground.sprite.name;
            saveButtonsLogic();
        }

        if (buttonsLogic.backImage != null)
        {
            if (sprites.ContainsKey(buttonsLogic.backImage))
            {
                backgroundImage.sprite = sprites[buttonsLogic.backImage];
            }
        }
        else
        {
            buttonsLogic.backImage = backgroundImage.sprite.name;
            saveButtonsLogic();
        }
    }

    string text;

    public void printText()
    {
        gameViewText.text = text;
    }

    public Button editButton;
//    public QuestCreator creator;

    public void showEditPanel()
    {
        homePressed();
//        creator.initMainQuest();
        editView.SetActive(true);
        if (clipboard != null)
        {
            clipboard.gameObject.SetActive(false);
            clipboard.text = "";
        }
    }

    public void homePressed()
    {
        logic.currentItemIndex = 0;
        procced();
    }

    public void importPressed()
    {
        if (clipboard != null)
        {
            clipboard.gameObject.SetActive(true);

            if (clipboard.text != "")
            {
                Logic newLogic = JsonUtility.FromJson<Logic>(clipboard.text);
                if (newLogic != null)
                {
                    logic = newLogic;
                    logic.currentItemIndex = 0;
                    procced();
                    QuestLoader loader = new QuestLoader();
                    loader.logic = logic;
                    loader.save();
                    clipboard.gameObject.SetActive(false);
                    clipboard.text = "";
                }
            }
        }

#if UNITY_EDITOR
        TextEditor te = new TextEditor();
        te.Paste();
        Logic l = JsonUtility.FromJson<Logic>(te.text);
        if (l != null)
        {
            logic = l;
            logic.currentItemIndex = 0;
            procced();
            QuestLoader loader = new QuestLoader();
            loader.logic = logic;
            loader.save();
        }
#endif
    }

    public void exportPressed()
    {
        if (clipboard != null)
        {
            clipboard.text = JsonUtility.ToJson(logic);
            clipboard.Select();
            clipboard.gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        TextEditor te = new TextEditor();
        te.text = JsonUtility.ToJson(logic);
        te.SelectAll();
        te.Copy();
#endif
    }

    public void saveChanges()
    {
        gameViewTitle.text = editTitleField.text;
        gameViewText.text = editTextField.text;
        logic.questions[logic.currentItemIndex].title = editTitleField.text;
        logic.questions[logic.currentItemIndex].description = editTextField.text;
        editView.SetActive(false);
    }

    public void saveButtonChanges()
    {
        editButtonPanel.gameObject.SetActive(false);
    }

    public void cancelChanges()
    {
        editView.SetActive(false);
    }

    public void cancelButtonChanges()
    {
        editButtonPanel.gameObject.SetActive(false);
    }
}
