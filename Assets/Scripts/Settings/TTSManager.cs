using UnityEngine;
using System.Collections;

public class TTSManager : MonoBehaviour
{
	public static TTSManager instance;

#if UNITY_ANDROID
	public TTSBridge ttsEngine;
#endif

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

#if UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
		//Debug.Log ("Initializing TTS..");
		TTSPlugin.initTTS();
#elif UNITY_ANDROID
		Debug.Log ("Initializing TTS...");
		ttsEngine = new TTSBridge();
		ttsEngine.Init();
#endif
	}
	
	public void Speak (string sentence)
	{
		switch (Settings.instance.language) {
		case Language.English:
			{
				string voicecode = "en-US";
				switch(Settings.instance.voice){
				case Voice.Kim:				voicecode = "en-US";		break;
				case Voice.Daniel:			voicecode = "en-GB";		break;
				case Voice.Moira:			voicecode = "en-IE";		break;
				case Voice.Camille:			voicecode = "en-AU";		break;
				}
				SpeakInLang (sentence, voicecode);
			}
			break;
//		case Language.Chinese: 	
//			{
//				string voicecode = "en-US";
//				if (DebugSettingsManager.settings ["chinesetts"] == true) {
//					voicecode = "zh-CN";
//				}
//				SpeakInLang (sentence, voicecode);
//			}
//			break;
		default:
			{
				string voicecode = "en-US";
				switch(Settings.instance.voice){
				case Voice.Kim:				voicecode = "en-US";		break;
				case Voice.Daniel:			voicecode = "en-GB";		break;
				case Voice.Moira:			voicecode = "en-IE";		break;
				case Voice.Camille:			voicecode = "en-AU";		break;
				}
				SpeakInLang (sentence, voicecode);
			}
			break;
		}
	}	
	
	public void SpeakInLang (string sentence, string lang)
	{
		if(Settings.instance.CanSpeakSentence ())
		{
			//Debug.Log(sentence);
			if(sentence.Equals("Semantic rule not found")) {
				//WordSelectionPageManager.instance.PlaySound_Invalid ();
				return;
			}
			
#if UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
			TTSPlugin.speak(sentence.ToLower(),lang);
#elif UNITY_ANDROID
			if (!ttsEngine.isSpeaking ()) 	{ ttsEngine.Speak(sentence); }		
#elif UNITY_WEBPLAYER
			if(sentence != "") {
				string cmd = "window.open(\'http://translate.google.com/translate_tts?tl=en&q="+sentence.Replace(" ","%20")+"\',\'FreeSpeech\')";
				cmd = cmd.Replace("\n","");
				cmd = cmd.Replace("\r","");
				Application.ExternalEval(cmd);
			}
#endif
		}
	}

	public bool isSpeaking(){
#if UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS || UNITY_EDITOR
		return TTSPlugin.isSpeaking();
#elif UNITY_ANDROID
		return ttsEngine.isSpeaking ();		
#elif UNITY_WEBPLAYER
		return false;
#endif
	}
}
