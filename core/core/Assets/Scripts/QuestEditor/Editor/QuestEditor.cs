#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/*
 * Created on Sun Jul 21 2019
 *
 * Copyright (c) 2019 Glowbom.
 */
public class QuestEditor : EditorWindow
{
    static bool cloudSaveEanbled = false;
    static bool projectsEnabled = false;
    static bool templatesEnabled = false;

    static QuestLoader questLoader = null;

    static QuestEditor() {
        questLoader = new QuestLoader();
    }

    private int tab = 0;
    private int tabElements = 0;
    private Vector2 scrollPos;
    private Vector2 scrollPosButtons;

    [MenuItem("Window/Glowbom/Quests")]
    public static void ShowWindow() {
        GetWindow<QuestEditor> ("Quests");
    }

    private void insert(int i) {
        Logic.Item item = new Logic.Item();
        item.title = "New Title";
        item.description = "Hello!";
        item.buttonsTexts = new string[1];
        item.buttonsTexts[0] = "Go Button";
        item.goIndexes = new int[1];
        item.goIndexes[0] = 0;
        item.buttonAnswers = new int[1];
        item.buttonAnswers[0] = 0;

        List<Logic.Item> questions = new List<Logic.Item>(questLoader.logic.questions);
        questions.Insert(i + 1, item);

        questLoader.logic.questions = questions.ToArray();

        foreach (var logicItem in questLoader.logic.questions) {
            for(int j = 0; j < logicItem.goIndexes.Length; j++) {
                if (logicItem.goIndexes[j] >= i + 1) {
                    logicItem.goIndexes[j] = logicItem.goIndexes[j] + 1;
                }
            }
        }

        GUI.FocusControl(null);
        initAllTabUi();
    }

    private void remove(int i) {
        List<Logic.Item> questions = new List<Logic.Item>(questLoader.logic.questions);
        if (questions.Count > 1)
        {
            questions.RemoveAt(i);

            questLoader.logic.questions = questions.ToArray();

            foreach (var logicItem in questLoader.logic.questions)
            {
                for (int j = 0; j < logicItem.goIndexes.Length; j++)
                {
                    if (logicItem.goIndexes[j] >= i && logicItem.goIndexes[j] != 10001 && logicItem.goIndexes[j] != 10002 && logicItem.goIndexes[j] != 10003)
                    {
                        logicItem.goIndexes[j] = logicItem.goIndexes[j] - 1;
                        if (logicItem.goIndexes[j] < 0)
                        {
                            logicItem.goIndexes[j] = 0;
                        }
                    }
                }
            }

            if (questLoader.logic.currentItemIndex == i)
            {
                questLoader.logic.currentItemIndex = 0;
            }

            GUI.FocusControl(null);
            initAllTabUi();
        }  
    }

    private string createTitle(Logic.Item item, int i)
    {
        string buttonTitle = i + " : [" + item.title + "] " + item.description;

        if (item.title == "")
        {
            buttonTitle = i + " : " + item.description;
        }

        if (i == questLoader.logic.currentItemIndex)
        {
            buttonTitle = "* " + buttonTitle;
        }

        buttonTitle = buttonTitle.Replace("\n", "");

        try
        {
            buttonTitle = buttonTitle.Substring(0, 30);
            buttonTitle += "...";
        }
        catch (ArgumentOutOfRangeException)
        {
            // less then 30 characters
        }

        return buttonTitle;
    }

    public void append()
    {
        Logic.Item item = new Logic.Item();
        item.title = "New Title";
        item.description = "Hello!";
        item.buttonsTexts = new string[1];
        item.buttonsTexts[0] = "Go Button";
        item.goIndexes = new int[1];
        item.goIndexes[0] = 0;
        item.buttonAnswers = new int[1];
        item.buttonAnswers[0] = 0;

        List<Logic.Item> questions = new List<Logic.Item>(questLoader.logic.questions);
        questions.Add(item);

        questLoader.logic.questions = questions.ToArray();
        questLoader.logic.currentItemIndex = questLoader.logic.questions.Length - 1;
        GUI.FocusControl(null);
        initAllTabUi();
    }

    private void initAllTabUi() {
        EditorGUILayout.Space();

        if (questLoader.logic != null) {
            if (GUILayout.Button("Add"))
            {
                append();
            }

            EditorGUILayout.Space();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width - 20), GUILayout.Height(490));
            int i = 0;
            foreach (var item in questLoader.logic.questions)
            {
                string buttonTitle = createTitle(item, i);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(buttonTitle)) {
                    GUI.FocusControl(null);
                    questLoader.logic.currentItemIndex = Array.IndexOf(questLoader.logic.questions, item);
                    tabElements = 1;
                    OnGUI();

                    try
                    {
                        Image mainImage = GameObject.Find("MainImage").GetComponent<Image>();
                        if (mainImage.GetComponent<RectTransform>().hasChanged)
                        {
                            var nextItem = questLoader.logic.questions[questLoader.logic.currentItemIndex];
                            if (nextItem.mainImagePosition != null && nextItem.mainImagePosition.x != 0)
                            {
                                mainImage.rectTransform.localPosition = nextItem.mainImagePosition;
                                mainImage.rectTransform.sizeDelta = nextItem.mainImageSize;
                            }
                        }
                    } catch(Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                    
                }

                if (GUILayout.Button("Remove"))
                {
                    remove(i);
                }

                GUILayout.EndHorizontal();

                /*if (GUILayout.Button("Insert")) {
                    insert(i);
                }*/

               
                

                EditorGUILayout.Space();
                ++i;
            }

            EditorGUILayout.Space();

            //initInitialHeroValues();

            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.Space();
    }

    private void insertQuest(int i)
    {
        List<Buttons.Button> buttonsList = new List<Buttons.Button>(questLoader.buttonsLogic.buttons);
        Buttons.Button button = new Buttons.Button();
        button.name = "New Quest";
        button.link = "Data/NewQuest";
        buttonsList.Insert(i + 1, button);

        questLoader.buttonsLogic.buttons = buttonsList.ToArray();

        GUI.FocusControl(null);
        initItemUi();
    }

    private void removeQuest(int i)
    {
        List<Buttons.Button> buttonsList = new List<Buttons.Button>(questLoader.buttonsLogic.buttons);
        buttonsList.RemoveAt(i);
        questLoader.buttonsLogic.buttons = buttonsList.ToArray();

        GUI.FocusControl(null);
        initItemUi();
    }

    private void convertToAskFriendButton()
    {
        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        if (item.goIndexes.Length > 0)
        {
            item.goIndexes[item.goIndexes.Length - 1] = 10002;
            item.buttonAnswers[item.goIndexes.Length - 1] = 0;
            item.buttonsTexts[item.goIndexes.Length - 1] = "Ask Friend";
        }

        GUI.FocusControl(null);
        initItemUi();
    }

    private void convertToShareButton()
    {
        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        if (item.goIndexes.Length > 0)
        {
            item.goIndexes[item.goIndexes.Length - 1] = 10001;
            item.buttonAnswers[item.goIndexes.Length - 1] = 0;
            item.buttonsTexts[item.goIndexes.Length - 1] = "Share";
        }

        GUI.FocusControl(null);
        initItemUi();
    }

    private void convertToBackButton()
    {
        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        if (item.goIndexes.Length > 0)
        {
            item.goIndexes[item.goIndexes.Length - 1] = 10003;
            item.buttonAnswers[item.goIndexes.Length - 1] = 0;
            item.buttonsTexts[item.goIndexes.Length - 1] = "Great";
        }

        GUI.FocusControl(null);
        initItemUi();
    }

    private void insertButton(int i) {
        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        List<string> buttonsTextsList = new List<string>(item.buttonsTexts);
        List<int> goIndexesList = new List<int>(item.goIndexes);
        List<int> goConditionsList = item.goConditions == null ? new List<int>() : new List<int>(item.goConditions);
        List<int> buttonAnswersList = new List<int>(item.buttonAnswers);

        if (goConditionsList.Count > 0) {
            goConditionsList.Insert(i + 1, 0);
        }

        goIndexesList.Insert(i + 1, questLoader.logic.questions.Length);
        buttonAnswersList.Insert(i + 1, 0);
        buttonsTextsList.Insert(i + 1, "Go Button");

        item.buttonsTexts = buttonsTextsList.ToArray();
        item.goIndexes = goIndexesList.ToArray();
        item.buttonAnswers = buttonAnswersList.ToArray();

        if (goConditionsList.Count > 0) {
            item.goConditions = goConditionsList.ToArray();
        }

        GUI.FocusControl(null);
        initItemUi();
    }

    private void removeButton(int i) {
        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        List<string> buttonsTextsList = new List<string>(item.buttonsTexts);
        List<int> goIndexesList = new List<int>(item.goIndexes);
        List<int> goConditionsList = item.goConditions != null ? new List<int>(item.goConditions) : new List<int>();
        List<int> buttonAnswersList = new List<int>(item.buttonAnswers);

        if (item.goConditions != null && goConditionsList.Count > 0) {
            goConditionsList.RemoveAt(i);
        }

        goIndexesList.RemoveAt(i);
        buttonsTextsList.RemoveAt(i);
        buttonAnswersList.RemoveAt(i);

        item.buttonsTexts = buttonsTextsList.ToArray();
        item.goIndexes = goIndexesList.ToArray();
        item.buttonAnswers = buttonAnswersList.ToArray();

        if (item.goConditions != null && goConditionsList.Count > 0) {
            item.goConditions = goConditionsList.ToArray();
        }

        GUI.FocusControl(null);
        initItemUi();
    }

    string deafautQuestName = "Quest";

    private void initQuestsUi()
    {
        EditorGUILayout.Space();

        if (questLoader.logic != null)
        {
            GUILayout.Label("Supporting Multiple Quests", EditorStyles.label);

            EditorGUILayout.Space();

            scrollPosButtons = EditorGUILayout.BeginScrollView(scrollPosButtons, GUILayout.Width(position.width - 20), GUILayout.Height(500));

            for (int i = 0; i < questLoader.buttonsLogic.buttons.Length; i++)
            {
                GUI.backgroundColor = Color.white;

                GUILayout.Label("Quest " + (i + 1), EditorStyles.label);
                questLoader.buttonsLogic.buttons[i].name = EditorGUILayout.TextField("Name", questLoader.buttonsLogic.buttons[i].name);
                questLoader.buttonsLogic.buttons[i].image = EditorGUILayout.TextField("Image", questLoader.buttonsLogic.buttons[i].image);
                questLoader.buttonsLogic.buttons[i].link = EditorGUILayout.TextField("Link", questLoader.buttonsLogic.buttons[i].link);
                questLoader.buttonsLogic.buttons[i].nft = EditorGUILayout.TextField("NFT", questLoader.buttonsLogic.buttons[i].nft);

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Insert"))
                {
                    insertQuest(i);
                }

                if (GUILayout.Button("Open"))
                {
                    QuestLoader.name = questLoader.buttonsLogic.buttons[i].link;
                    questLoader.load();
                }


                GUI.backgroundColor = new Color32(238, 32, 77, 255);
                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = Color.white;
                if (GUILayout.Button("Remove", style))
                {
                    removeQuest(i);
                    break;
                }

                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            EditorGUILayout.Space();

            //initHeroValues();

            EditorGUILayout.EndScrollView();
        }
    }



    private void initItemUi() {
        EditorGUILayout.Space();

        if (questLoader.logic != null) {
            var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
            GUILayout.Label("Screen " + questLoader.logic.currentItemIndex, EditorStyles.label);

            item.title = EditorGUILayout.TextField("Title", item.title);
            EditorGUILayout.Space();

            var areaStyle = new GUIStyle(GUI.skin.textArea);
            areaStyle.wordWrap = true;
            var width = position.width - 35;

            //item.description = EditorGUILayout.TextArea(item.description, GUILayout.Width(150));

            areaStyle.fixedHeight = 0; // reset height, else CalcHeight gives wrong numbers
            areaStyle.fixedHeight = areaStyle.CalcHeight(new GUIContent(item.description), width);
            item.description = EditorGUILayout.TextArea(item.description, areaStyle, GUILayout.Height(areaStyle.fixedHeight));

            EditorGUILayout.Space();

            if (item.picturesSpriteNames == null || item.picturesSpriteNames.Length == 0)
            {
                item.picturesSpriteNames = new string[1];
            }

            item.picturesSpriteNames[0] = EditorGUILayout.TextField("Image", item.picturesSpriteNames[0]);
            item.answersCount = int.Parse(EditorGUILayout.TextField("Answers Count", item.answersCount.ToString()));
            item.answerPicture = EditorGUILayout.TextField("Answer Picture", item.answerPicture);
            item.answerPictureDelay = int.Parse(EditorGUILayout.TextField("Answers Picture Delay", item.answerPictureDelay.ToString()));

            EditorGUILayout.Space();

            scrollPosButtons = EditorGUILayout.BeginScrollView(scrollPosButtons, GUILayout.Width(position.width - 20), GUILayout.Height(250));

            int i = 0;
            foreach(var goIndex in item.goIndexes) {
                if (goIndex >= 0) {
                    GUI.backgroundColor = Color.white;

                    GUILayout.Label("Button " + (i + 1), EditorStyles.label);
                    item.buttonsTexts[i] = EditorGUILayout.TextField("Title", item.buttonsTexts[i]);
                    item.goIndexes[i] = int.Parse(EditorGUILayout.TextField("Go To", item.goIndexes[i].ToString()));

                    if (item.buttonAnswers == null || item.buttonAnswers.Length == 0 || item.buttonAnswers.Length != item.goIndexes.Length)
                    {
                        item.buttonAnswers = new int[item.goIndexes.Length];
                        for(int j = 0; j < item.goIndexes.Length; j++)
                        {
                            item.buttonAnswers[j] = 0;
                        }
                    }

                    item.buttonAnswers[i] = int.Parse(EditorGUILayout.TextField("Answer", item.buttonAnswers[i].ToString()));
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Insert"))
                    {
                        insertButton(i);
                    }

                    if (GUILayout.Button("Go"))
                    {
                        if (goIndex >= 0 && goIndex < questLoader.logic.questions.Length)
                        {
                            GUI.FocusControl(null);
                            questLoader.logic.currentItemIndex = goIndex;
                            initItemUi();
                        }
                    }


                    GUI.backgroundColor = new Color32(238, 32, 77, 255);
                    var style = new GUIStyle(GUI.skin.button);
                    style.normal.textColor = Color.white;
                    if (GUILayout.Button("Remove", style))
                    {
                        removeButton(i);
                        break;
                    }

                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    ++i;

                }
            }

            EditorGUILayout.Space();

            //initHeroValues();

            EditorGUILayout.EndScrollView();
        }
    }

    private void initHeroValues() {
        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        GUILayout.Label("Values", EditorStyles.label);

        EditorGUILayout.Space();

        if (questLoader.logic.heroElements == null || questLoader.logic.heroElements.Length == 0) {
            questLoader.logic.heroElements = new string[1];
            questLoader.logic.heroElements[0] = "Points";

            questLoader.logic.heroValues = new int[1];
            questLoader.logic.heroValues[0] = 0;
        }

        if (item.heroValues == null || item.heroValues.Length == 0 || item.heroValues.Length != questLoader.logic.heroValues.Length) {
            item.heroValues = new int[questLoader.logic.heroValues.Length];
        }

        for(int x = 0; x < questLoader.logic.heroValues.Length; x++) {
            GUI.backgroundColor = Color.white;

            questLoader.logic.heroElements[x] = EditorGUILayout.TextField("Name", questLoader.logic.heroElements[x]);
            item.heroValues[x] = int.Parse(EditorGUILayout.TextField(questLoader.logic.heroElements[x], item.heroValues[x].ToString()));
            GUILayout.BeginHorizontal();
        
            if (GUILayout.Button("Insert")) {
                insertHeroValue(x);
            }

            if (GUILayout.Button("Remove")) {
                removeHeroValue(x); 
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
    }

    private void initInitialHeroValues() {
        GUILayout.Label("Values", EditorStyles.label);

        EditorGUILayout.Space();

        if (questLoader.logic.heroElements == null || questLoader.logic.heroElements.Length == 0) {
            questLoader.logic.heroElements = new string[1];
            questLoader.logic.heroElements[0] = "Points";

            questLoader.logic.heroValues = new int[1];
            questLoader.logic.heroValues[0] = 0;
        }

        for(int x = 0; x < questLoader.logic.heroValues.Length; x++) {
            GUI.backgroundColor = Color.white;

            questLoader.logic.heroElements[x] = EditorGUILayout.TextField("Name", questLoader.logic.heroElements[x]);
            questLoader.logic.heroValues[x] = int.Parse(EditorGUILayout.TextField(questLoader.logic.heroElements[x], questLoader.logic.heroValues[x].ToString()));
            GUILayout.BeginHorizontal();
        
            if (GUILayout.Button("Insert")) {
                insertInitialHeroValue(x);
            }

            if (GUILayout.Button("Remove")) {
                removeInitialHeroValue(x); 
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
    }

    private void insertHeroValue(int i) {
        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        List<string> heroElementsList = new List<string>(questLoader.logic.heroElements);
        List<int> heroValuesList = new List<int>(questLoader.logic.heroValues);
        List<int> heroValuesItemList = new List<int>(item.heroValues);

        heroValuesList.Insert(i + 1, 0);
        heroValuesItemList.Insert(i + 1, 0);
        heroElementsList.Insert(i + 1, "Points " + i);

        questLoader.logic.heroElements = heroElementsList.ToArray();
        questLoader.logic.heroValues = heroValuesList.ToArray();
        item.heroValues = heroValuesItemList.ToArray();

        GUI.FocusControl(null);
        initItemUi();
    }

    private void removeHeroValue(int i) {
        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        List<string> heroElementsList = new List<string>(questLoader.logic.heroElements);
        List<int> heroValuesList = new List<int>(questLoader.logic.heroValues);
        List<int> heroValuesItemList = new List<int>(item.heroValues);

        heroElementsList.RemoveAt(i);
        heroValuesList.RemoveAt(i);
        heroValuesItemList.RemoveAt(i);

        questLoader.logic.heroElements = heroElementsList.ToArray();
        questLoader.logic.heroValues = heroValuesList.ToArray();
        item.heroValues = heroValuesItemList.ToArray();

        GUI.FocusControl(null);
        initItemUi();
    }

    private void insertInitialHeroValue(int i) {
        List<string> heroElementsList = new List<string>(questLoader.logic.heroElements);
        List<int> heroValuesList = new List<int>(questLoader.logic.heroValues);

        heroValuesList.Insert(i + 1, 0);
        heroElementsList.Insert(i + 1, "Points " + i);

        questLoader.logic.heroElements = heroElementsList.ToArray();
        questLoader.logic.heroValues = heroValuesList.ToArray();

        GUI.FocusControl(null);
        initItemUi();
    }

    private void removeInitialHeroValue(int i) {
        List<string> heroElementsList = new List<string>(questLoader.logic.heroElements);
        List<int> heroValuesList = new List<int>(questLoader.logic.heroValues);

        heroElementsList.RemoveAt(i);
        heroValuesList.RemoveAt(i);

        questLoader.logic.heroElements = heroElementsList.ToArray();
        questLoader.logic.heroValues = heroValuesList.ToArray();

        GUI.FocusControl(null);
        initItemUi();
    }

    private void initMainQuest() {
        // main quest

        GUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.Space();

        QuestLoader.name = EditorGUILayout.TextField("Name", QuestLoader.name);
        QuestLoader.language = EditorGUILayout.TextField("Language", QuestLoader.language);
        questLoader.logic.backgroundPicture = EditorGUILayout.TextField("Background Picture", questLoader.logic.backgroundPicture);
        questLoader.logic.link = EditorGUILayout.TextField("Link", questLoader.logic.link);
        questLoader.logic.nft = EditorGUILayout.TextField("NFT", questLoader.logic.nft);

        EditorGUILayout.Space();

        GUILayout.Label(QuestLoader.name + " [" + questLoader.logic.questions.Length + " screens]", EditorStyles.label);

        EditorGUILayout.Space();

        var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
        string buttonTitle = createTitle(item, questLoader.logic.currentItemIndex);
        tabElements = GUILayout.Toolbar (tabElements, new string[] { "All", buttonTitle, "Quests" });
        switch (tabElements) {
            case 0:
                initAllTabUi();
            break;
            case 1:
                initItemUi();
            break;
            case 2:
                initQuestsUi();
                break;
        }

        GUILayout.EndVertical();

        /// end of elements

        EditorGUILayout.Space();

        if (projectsEnabled) {
            initProjectsUi();
        }

        if (templatesEnabled) {
            initTemplatesUi();
        }

        EditorGUILayout.Space();
    }

    private void OnGUI()
    {
        questLoader.initialize();

        GUILayout.Label("Glowbom", EditorStyles.boldLabel);

        GUILayout.Label("Quests", EditorStyles.label);

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Home"))
        {
            tabElements = 0;
            questLoader.logic.currentItemIndex = 0;
            questLoader.save();
            questLoader.load();
        }

        if (GUILayout.Button("Import"))
        {
            TextEditor te = new TextEditor();
            te.Paste();
            questLoader.logic = JsonUtility.FromJson<Logic>(te.text);
            if (questLoader.logic != null)
            {
                questLoader.save();
            }
            else
            {
                questLoader.load();
            }
        }

        if (GUILayout.Button("Export"))
        {
            TextEditor te = new TextEditor();
            te.text = JsonUtility.ToJson(questLoader.logic);
            te.SelectAll();
            te.Copy();
        }

        if (GUILayout.Button("Load")) 
        {
            questLoader.load(false);
        }

        if (GUILayout.Button("Save"))
        {
            try
            {
                Image mainImage = GameObject.Find("MainImage").GetComponent<Image>();
                if (mainImage.GetComponent<RectTransform>().hasChanged)
                {
                    var item = questLoader.logic.questions[questLoader.logic.currentItemIndex];
                    item.mainImagePosition = mainImage.rectTransform.localPosition;
                    item.mainImageSize = mainImage.rectTransform.sizeDelta;
                }
            } catch(Exception e)
            {
                Debug.Log(e.Message);
            }

            questLoader.save();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Apply to all"))
        {
            try
            {
                Image mainImage = GameObject.Find("MainImage").GetComponent<Image>();
                if (mainImage.GetComponent<RectTransform>().hasChanged)
                {
                    foreach (var item in questLoader.logic.questions)
                    {
                        item.mainImagePosition = mainImage.rectTransform.localPosition;
                        item.mainImageSize = mainImage.rectTransform.sizeDelta;
                    }
                }
            } catch(Exception e)
            {
                Debug.Log(e.Message);
            }
            
            questLoader.save();
        }

        if (GUILayout.Button("Ask Friend"))
        {
            convertToAskFriendButton();
        }

        if (GUILayout.Button("Share Button"))
        {
            convertToShareButton();
        }

        if (GUILayout.Button("Back Button"))
        {
            convertToBackButton();
        }

        GUILayout.EndHorizontal();
        EditorGUILayout.Space();


        if (cloudSaveEanbled) {
            initCloudSaveUi();
        }

        EditorGUILayout.Space();

        if (questLoader.logic != null) {
            initMainQuest();
        }
    }

    private void initCloudSaveUi() {
        GUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.Space();

        GUILayout.Label("Cloud Storage", EditorStyles.label);

        EditorGUILayout.Space();

        GUILayout.Label("Glowbom Account", EditorStyles.label);

        EditorGUILayout.Space();

        tab = GUILayout.Toolbar (tab, new string[] {"Login", "Sign Up"});
        switch (tab) {
            case 0:
                EditorGUILayout.Space();

                EditorGUILayout.TextField("Email", "");
                EditorGUILayout.TextField("Password", "");

                EditorGUILayout.Space();

                if (GUILayout.Button("Login")) {
                    // login code here
                }

                EditorGUILayout.Space();
            break;
            case 1:
                EditorGUILayout.Space();

                EditorGUILayout.TextField("Email", "");
                EditorGUILayout.TextField("New Password", "");
                EditorGUILayout.TextField("Confirm Password", "");

                EditorGUILayout.Space();

                if (GUILayout.Button("Sign Up")) {
                    // login code here
                }

                EditorGUILayout.Space();
            break;
        }

        GUILayout.EndVertical();
    }

    private void initProjectsUi() {
        // more quests

        GUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.Space();

        GUILayout.Label("Projects", EditorStyles.label);

        EditorGUILayout.Space();

        GUILayout.EndVertical();

        EditorGUILayout.Space();
    }

    private void initTemplatesUi() {
        // templates

        GUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.Space();

        GUILayout.Label("Templates", EditorStyles.label);

        EditorGUILayout.Space();

        GUILayout.EndVertical();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

#endif
