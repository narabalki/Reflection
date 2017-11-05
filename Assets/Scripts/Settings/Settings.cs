using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum Switch
{
	Off,
	On}
;

public enum Language
{
	//	English, Spanish, Hindi, Chinese, French
	Chinese,
	Spanish,
	English}
;

//Not used
public enum Difficulty
{
	Beginner,
	Medium,
	Expert}
;

public enum Voice
{
	Kim,
	Daniel,
	Moira,
	Camille}
;

//Not used
public enum UserType
{
	Child,
	Teacher,
	Adult}
;

//Not used (?)
public enum TutorialState
{
	Introduction,
	Closure,
	AddNewWord,
	FirstSentence,
	SecondSentence,
	ThirdSentence,
	FourthSentence}
;

public class Settings : MonoBehaviour
{
	public static Settings instance;

	public const string LANGUAGE = "LANGUAGE";
	public const string VOICE_LANGUAGE = "VOICE_LANGUAGE";
	public const string ANIMATE_SENTENCE	= "ANIMATE_SENTENCE";

	public const string SOUND_FX = "SOUND_FX";
	public const string SPEAK_SENTENCE = "SPEAK_SENTENCE";
	public const string AUDIO_QUESTION = "AUDIO_QUESTION";
	public const string TILE_CONNECTORS = "TILE_CONNECTORS";

	public const string WORD_LEVEL = "WORD_LEVEL";
	public const string FILTER_LEVEL = "FILTER_LEVEL";
	public const string QUESTION_LEVEL = "QUESTION_LEVEL";

	public const string VOICE = "VOICE";
	public const string VOICE_SPEED = "VOICE_SPEED";

	public const string BUILD_NUMBER = "BUILD_NUMBER";
	public const string APP_VERSION = "APP_VERSION"; 

	public Language language;
	public Language voice_language;
	public Language animate_sentence;

	public Switch soundEffects;
	public Switch speakSentence; 

	public Difficulty wordLevel; //Not used
	public Difficulty filterLevel; //Not used
	public Difficulty questionLevel; //Not used

	public Voice voice;
	public int voiceSpeed = 3;

	public Material material_ON;
	public Material material_OFF;

	public bool supportMultiplePhrases = true;
	public bool supportPrompt = true;
	public bool supportAddWords = true;

	public bool isLargeDevice = false;
	public bool isiCloudEnabled = false;

	public string userName = "";

	public const string buildNumber = "13.0"; // For app version - PlayerSettings.bundleVersion

	[SerializeField] bool resetSettings = false;

	void MakeSingleton ()
	{
		DontDestroyOnLoad (this.gameObject);
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}

	void Awake ()
	{
		MakeSingleton ();

		if (resetSettings) {
			DeleteSettings ();
		}

		RetriveSettings ();
		SetCompatibilitySettings ();
	}

	void OnLevelWasLoaded (int level)
	{
		SetSmartLanguage();
	}

	void Start ()
	{
		StartLocalization();
	}

	void StartLocalization ()
	{
		Settings.instance.SetSmartLanguage();
	}

	public string GetCurrentLanguageTableName ()
	{
		switch (language) {
		case Language.English:
			return "english";
		case Language.Spanish:
			return "spanish";
//			case Language.Hindi: 	return "language2";
//			case Language.Chinese: 	return "language3";
//			case Language.French: 	return "language4";
//			case Language.Tamil: 	return "language5";
		}

		return "english";
	}

	public bool CanPlaySFX ()
	{
		return IsTurnedOn (soundEffects);
	}

	public bool CanSpeakSentence ()
	{
		return IsTurnedOn (speakSentence);
	}

	public bool IsTurnedOn (Switch switchObj)
	{
		switch (switchObj) {
		case Switch.On:
			return true;
		case Switch.Off:
			return false;
		}

		return false;
	}

	public void SetLanguage (Language newLanguage)
	{
		language = newLanguage;
		SaveSettings ();
	}

	public void SetChallengeSentenceLanguage (Language newLanguage)
	{
		language = newLanguage;
		SaveSettings ();
	}
		
	//public void SaveVoiceSpeed (float value) {
	//	PlayerPrefs.SetFloat (VOICE_SPEED, value);
	//}

	public void RetriveVoiceSpeed ()
	{
		if (!PlayerPrefs.HasKey (VOICE_SPEED)) { 
			SetDefaultVoiceSpeed ();
		} else {
			voiceSpeed = PlayerPrefs.GetInt (VOICE_SPEED.ToString ());

			if (voiceSpeed == 0) {
				SetDefaultVoiceSpeed ();
			}
		}
	}

	void SetDefaultVoiceSpeed ()
	{
		float speed = TTSPlugin.getDefaultRate (); //1
		//speed = (speed / 0.6f);
#if UNITY_EDITOR ||  UNITY_STANDALONE_OSX
		voiceSpeed = (int)Math.Round ((speed - 65f) / 33, 0);
		if (voiceSpeed < 90 || voiceSpeed > 300) {
			voiceSpeed = (int)Math.Round ((180 - 65f) / 33, 0);
		}
		PlayerPrefs.SetInt (VOICE_SPEED, voiceSpeed);
#elif UNITY_IPHONE || UNITY_TVOS
		voiceSpeed = (int)Math.Round((speed / 0.6f)*6.0f, 0);
		PlayerPrefs.SetInt (VOICE_SPEED, voiceSpeed);
#endif
	}

	public void DeleteSettings ()
	{
		if (PlayerPrefs.HasKey (LANGUAGE))
			PlayerPrefs.DeleteKey (LANGUAGE);

		if (PlayerPrefs.HasKey (VOICE_LANGUAGE))
			PlayerPrefs.DeleteKey (VOICE_LANGUAGE);
		if (PlayerPrefs.HasKey (ANIMATE_SENTENCE))
			PlayerPrefs.DeleteKey (ANIMATE_SENTENCE);

		if (PlayerPrefs.HasKey (SOUND_FX))
			PlayerPrefs.DeleteKey (SOUND_FX);
		if (PlayerPrefs.HasKey (SPEAK_SENTENCE))
			PlayerPrefs.DeleteKey (SPEAK_SENTENCE);
		if (PlayerPrefs.HasKey (AUDIO_QUESTION))
			PlayerPrefs.DeleteKey (AUDIO_QUESTION);
		if (PlayerPrefs.HasKey (TILE_CONNECTORS))
			PlayerPrefs.DeleteKey (TILE_CONNECTORS);
	    
		if (PlayerPrefs.HasKey (WORD_LEVEL))
			PlayerPrefs.DeleteKey (WORD_LEVEL);
		if (PlayerPrefs.HasKey (FILTER_LEVEL))
			PlayerPrefs.DeleteKey (FILTER_LEVEL);
		if (PlayerPrefs.HasKey (QUESTION_LEVEL))
			PlayerPrefs.DeleteKey (QUESTION_LEVEL);

		if (PlayerPrefs.HasKey (VOICE))
			PlayerPrefs.DeleteKey (VOICE);
		if (PlayerPrefs.HasKey (VOICE_SPEED))
			PlayerPrefs.DeleteKey (VOICE_SPEED);

	}

	public void SaveSettings ()
	{
		PlayerPrefs.SetString (LANGUAGE, language.ToString ());
		PlayerPrefs.SetString (VOICE_LANGUAGE, voice_language.ToString ());
		PlayerPrefs.SetString (ANIMATE_SENTENCE, animate_sentence.ToString ());

		PlayerPrefs.SetString (SOUND_FX, soundEffects.ToString ());
		PlayerPrefs.SetString (SPEAK_SENTENCE, speakSentence.ToString ());

		PlayerPrefs.SetString (WORD_LEVEL, wordLevel.ToString ());

		PlayerPrefs.SetString (FILTER_LEVEL, filterLevel.ToString ());

		PlayerPrefs.SetString (QUESTION_LEVEL, questionLevel.ToString ());

		PlayerPrefs.SetString (VOICE, voice.ToString ());

		PlayerPrefs.SetInt (VOICE_SPEED, voiceSpeed);

		PlayerPrefs.SetString (BUILD_NUMBER, buildNumber);

		SaveSettingsToMixpanel ();

		#if UNITY_EDITOR
		#elif UNITY_IOS || UNITY_TVOS
		IOSUtilities.SaveSettingsToCustomFolder ();
		#endif
	}

	public void SaveSettingsToMixpanel ()
	{
		Dictionary<string,object> settings = new Dictionary<string,object> ();
		settings.Add (LANGUAGE, language.ToString ());
//		settings.Add (WORD_LEVEL, wordLevel.ToString ());
//		settings.Add (FILTER_LEVEL, filterLevel.ToString ());
//		settings.Add (QUESTION_LEVEL, questionLevel.ToString ());
		settings.Add (VOICE, voice.ToString ());
		settings.Add (VOICE_SPEED, "" + voiceSpeed);
		Mixpanel.instance.registerSuperProperties (settings);
	}

	public void RetriveSettings ()
	{
		if (!PlayerPrefs.HasKey (LANGUAGE)) {
			language = GetDeviceLanguage ();
		} 
		language = GetLanguageFromString (PlayerPrefs.GetString (LANGUAGE, language.ToString ()));
		voice_language = GetLanguageFromString (PlayerPrefs.GetString (VOICE_LANGUAGE, voice_language.ToString ()));
		animate_sentence	= GetLanguageFromString (PlayerPrefs.GetString (ANIMATE_SENTENCE, animate_sentence.ToString ()));

		soundEffects = GetStateFromString (PlayerPrefs.GetString (SOUND_FX, soundEffects.ToString ()));
		speakSentence = GetStateFromString (PlayerPrefs.GetString (SPEAK_SENTENCE, speakSentence.ToString ()));

		wordLevel = GetDifficultyFromString (PlayerPrefs.GetString (WORD_LEVEL, wordLevel.ToString ()));
		filterLevel = GetDifficultyFromString (PlayerPrefs.GetString (FILTER_LEVEL, filterLevel.ToString ()));
		questionLevel = GetDifficultyFromString (PlayerPrefs.GetString (QUESTION_LEVEL, questionLevel.ToString ()));

		voice = GetVoiceFromString (PlayerPrefs.GetString (VOICE, voice.ToString ()));
		RetriveVoiceSpeed ();
		SaveSettingsToMixpanel ();
	}

	Language GetLanguageFromString (string str)
	{
		foreach (Language lang in Enum.GetValues(typeof(Language))) {
			if (lang.ToString ().Equals (str.ToString ())) {
				return lang;
			}
		}
		return Language.English;
	}

	UserType GetUserFromString (string str)
	{
		foreach (UserType usr in Enum.GetValues(typeof(UserType))) {
			if (usr.ToString ().Equals (str.ToString ())) {
				return usr;
			}
		}
		return UserType.Adult;
	}

	public void SetSmartLanguage ()
	{
		SmartLocalization.LanguageManager.Instance.ChangeLanguage (GetCurrentLanguageCode ());

		#if UNITY_EDITOR
		//Debug.Log("Setting Language to "+GetCurrentLanguageCode()+" for native ui");
		#elif UNITY_IOS
		if (!PlayerPrefs.HasKey (LANGUAGE)) {
			language = GetDeviceLanguage();
		}
		if (language == Language.English) {
			IOSUtilities.SetLanguage ("en");
		}
		else if (language == Language.Spanish) {
			IOSUtilities.SetLanguage ("es");
		}
		#endif
	}

	static string GetCurrentLanguageCode ()
	{
		string languageCode = "en-US";
		switch (Settings.instance.language) {
		case Language.English:
			languageCode = "en-US";
			break;
		case Language.Spanish:
			languageCode = "es-ES";
			break;
		case Language.Chinese:
			languageCode = "zh-CHS";
			break;
		}
		return languageCode;
	}

	Voice GetVoiceFromString (string str)
	{
		foreach (Voice voiceEnum in Enum.GetValues(typeof(Voice))) {
			if (voiceEnum.ToString ().Equals (str.ToString ())) {
				return voiceEnum;
			}
		}
		return Voice.Kim;
	}

	Difficulty GetDifficultyFromString (string str)
	{
		foreach (Difficulty difficultyEnum in Enum.GetValues(typeof(Difficulty))) {
			if (difficultyEnum.ToString ().Equals (str.ToString ())) {
				return difficultyEnum;
			}
		}
		return Difficulty.Expert;
	}

	Switch GetStateFromString (string str)
	{
		switch (str) {
		case ("On"):
			return Switch.On;
		case ("Off"):
			return Switch.Off;
		}
		return Switch.On;
	}

	public TutorialState GetTutStateFromString (string str)
	{
		switch (str) {
		case ("FirstSentence"):
			return TutorialState.FirstSentence;
		case ("SecondSentence"):
			return TutorialState.SecondSentence;
		}
		return TutorialState.FirstSentence;
	}

	public string GetStringFromDifficulty (Difficulty diff)
	{
		switch (diff) {
		case Difficulty.Beginner:
			return "Beginner";
		case Difficulty.Medium:
			return "Intermediate";
		case Difficulty.Expert:
			return "Expert";
		}
		return "Expert";
	}

	public void SwitchToTutorialSettings ()
	{
		wordLevel = Difficulty.Expert;
		filterLevel = Difficulty.Expert;
		questionLevel = Difficulty.Expert;
	}

	public void SwitchToChallengeSettings ()
	{
		wordLevel = Difficulty.Expert;
		filterLevel = Difficulty.Expert;
		questionLevel = Difficulty.Expert;
	}

	static Language GetDeviceLanguage ()
	{

		Language lang = Language.English;

		#if UNITY_EDITOR || UNITY_ANDROID
		lang = Language.English;
		#elif UNITY_IOS
		string langCode = IOSUtilities.GetDeviceLanguage ();
		if(langCode.StartsWith("en"))
			lang = Language.English;
		else if (langCode.StartsWith("es"))
			lang = Language.Spanish;
		else if (langCode.StartsWith("zh"))
			lang = Language.Chinese;
		#endif	
		return lang;
	}

	int _bool_to_int (bool state)
	{
		if (state) {
			return 1;
		}

		return 0;
	}

	bool _int_to_bool (int val)
	{
		if (val == 0) {
			return false;
		}

		return true;
	}

	public void SetCompatibilitySettings ()
	{
		Settings.instance.RetriveSettings ();
		Difficulty finalDifficulty = Difficulty.Beginner;
		Difficulty[] diffs = { Settings.instance.wordLevel, Settings.instance.filterLevel, Settings.instance.questionLevel };
		foreach (Difficulty diff in diffs) {
			if (diff > finalDifficulty) {
				finalDifficulty = diff;
			}
		}
		Settings.instance.wordLevel = finalDifficulty;
		Settings.instance.filterLevel = finalDifficulty;
		Settings.instance.questionLevel = finalDifficulty;
		Settings.instance.SaveSettings ();
	}
}