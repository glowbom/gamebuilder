using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Utils;

/*
 * Created on Sun Jul 21 2019
 *
 * Copyright (c) 2019 Glowbom, Inc.
 */
public class GameStatusMagic : MonoBehaviour {

	private const string GAME_STATUS_SAVE_PATH = "GameStatusMagicSave.json";
	private const string GAME_STATUS_WEB_SAVE_KEY = "GameStatusMagic";
	private static GameStatusMagic _instance;
	
	public static void clear() {
		_instance = null;
	}

	static public GameStatusMagic instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType(typeof(GameStatusMagic)) as GameStatusMagic;
				if (_instance == null)
				{
					GameObject go = new GameObject("_Game Status Magic");
					_instance = go.AddComponent<GameStatusMagic>();
				}
			}
			return _instance;
		}
	}

	[System.Serializable]
	public class User {
		public string _id;
		public string email;
	}

	[System.Serializable]
	public class Quest {
		public string _id;
		public Logic text;
		public string content;

		public User user;
	}

	class GameStatusSave
	{
		public List<string> questAnswers { get; set; }
		public List<int> lifebookAnswers { get; set; }
		public List<int> activityTrackerAnswers { get; set; }
		public List<int> moneyModelAnswers { get; set; }
		public int moves { get; set; }	

		public string nothing { get; set; }	

		public User user { get; set; }
		public Quest quest { get; set; }
	}

    private void PopulateFromSave(GameStatusSave save)
	{
		this.questAnswers = save.questAnswers;
		this.lifebookAnswers = save.lifebookAnswers;
		this.activityTrackerAnswers = save.activityTrackerAnswers;
		this.moneyModelAnswers = save.moneyModelAnswers;
		this.moves = save.moves;
		this.user = save.user;
		this.quest = save.quest;
		this.token = save.nothing;
		if (this.questAnswers == null) {
			questAnswers = new List<string>();
		}

		if (this.lifebookAnswers == null) {
			lifebookAnswers = new List<int>();
		}

		if (this.activityTrackerAnswers == null) {
			activityTrackerAnswers = new List<int>();
		}

		if (this.moneyModelAnswers == null) {
			moneyModelAnswers = new List<int>();
		}
	}

	private GameStatusSave CreateSave()
	{
		if (this.questAnswers == null) {
			questAnswers = new List<string>();
		}

		if (this.lifebookAnswers == null) {
			lifebookAnswers = new List<int>();
		}

		if (this.activityTrackerAnswers == null) {
			activityTrackerAnswers = new List<int>();
		}

		if (this.moneyModelAnswers == null) {
			moneyModelAnswers = new List<int>();
		}

		GameStatusSave save = new GameStatusSave()
		{
			questAnswers = new List<string>(this.questAnswers),
			lifebookAnswers = new List<int>(this.lifebookAnswers),
			activityTrackerAnswers = new List<int>(this.activityTrackerAnswers),
			moneyModelAnswers = new List<int>(this.moneyModelAnswers),
			moves = this.moves,
			user = this.user,
			quest = this.quest,
			nothing = token
		};

		return save;
	}

	public List<string> questAnswers { get; set; }	
	public List<int> lifebookAnswers { get; set; }	
	public List<int> activityTrackerAnswers { get; set; }	
	public List<int> moneyModelAnswers { get; set; }	
	public int moves { get; set; }

	public User user { get; set; }
	public Quest quest { get; set; }

	public void save ()
	{
		/* if (Application.isWebPlayer) {
			string data = JsonWriter.Serialize(CreateSave());
			PlayerPrefs.SetString("GameStatusMagic", data);
			return;
		}*/
		var save = CreateSave();
		FileUtils.SafeWriteToFile(JsonUtility.ToJson(save), GAME_STATUS_SAVE_PATH);
	}

	public bool load ()
	{
		restoreToken();
		questAnswers = new List<string>();
		lifebookAnswers = new List<int>();
		activityTrackerAnswers = new List<int>();
		moneyModelAnswers = new List<int>();
		/* if (Application.isWebPlayer) {
			string data = PlayerPrefs.GetString("GameStatusMagic");
			GameStatusSave gss = JsonReader.Deserialize<GameStatusSave>(data);
			if (gss != null) {
				PopulateFromSave(gss);
				
				return true;
			}
			return false;
		}*/

		var save = FileUtils.LoadAndDeserialize<GameStatusSave>(GAME_STATUS_SAVE_PATH);
		if (save != null) {
			PopulateFromSave(save);
		}

		return true;
	}

	private string token = "";

	public string getToken() {
		return token;
	}

	private const string TOKEN = "thetoken";

	public void restoreToken() {
		if (token == null) {
			token = "";
		}
	}

	public void resetToken() {
		storeToken("");
	}

	public void storeToken(string theToken) {
		token = theToken;
		save();
	}
}
