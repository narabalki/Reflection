using UnityEngine;
using System.Collections;
// We need this one for importing our IOS functions
using System.Runtime.InteropServices;
 
public class TTSPlugin
{
// Use this #if so that if you run this code on a different platform, you won't get errors.
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
[DllImport ("FS_OSX_TTS")] private static extern void getSpeechSynthesizer();
[DllImport ("FS_OSX_TTS")] private static extern void speakText(string text);
[DllImport ("FS_OSX_TTS")] private static extern void speakTextWithLang(string text,string lang);
[DllImport ("FS_OSX_TTS")] private static extern bool osXisSpeaking();
[DllImport ("FS_OSX_TTS")] private static extern float osXgetDefaultRate();
[DllImport ("FS_OSX_TTS")] private static extern void speakTextWithLangAtRate (string text, string lang, float rate);
#elif UNITY_IPHONE || UNITY_WEBPLAYER || UNITY_TVOS
[DllImport ("__Internal")]
private static extern void _initTTS();
 
[DllImport ("__Internal")]
private static extern void _speak(string cString, string lang,float rate);

[DllImport ("__Internal")]
private static extern bool _isSpeaking();

[DllImport ("__Internal")]
private static extern float _getDefaultRate();
#endif
 
// Now make methods that you can provide the iOS functionality
 

	public static void initTTS( )
	{
	    // We check for UNITY_IPHONE again so we don't try this if it isn't iOS platform.
		#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			getSpeechSynthesizer ();
		#elif UNITY_IPHONE || UNITY_TVOS
	    // Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.
			_initTTS();
		#endif
		//Debug.Log("init tts");
	}

	//For English lang : en-US
	//For Chinese lang :
	//For Hindi   lang :
	public static void speak(string cString, string lang)
	{
		// We check for UNITY_IPHONE again so we don't try this if it isn't iOS platform.
		#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		float rt = (Settings.instance.voiceSpeed * 33) + 65;
		if(rt < 90 || rt > 300) {
			rt = 180;
		}
//		speakTextWithLangAtRate(cString,lang,rt);
		//Debug.Log(ExtensionMethods.AppWritableDirectory());

		#elif UNITY_IPHONE || UNITY_TVOS
		// Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.

		float rt = (.6f/6.0f)*Settings.instance.voiceSpeed;
		_speak(cString, lang, rt); //PlayerPrefs.GetFloat("voicespeed"));

		#endif
		//Debug.Log ("TTS has spoken Sentence :" + cString);
	}
	
	public static bool isSpeaking( )
	{
		// We check for UNITY_IPHONE again so we don't try this if it isn't iOS platform.
	#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		return osXisSpeaking();
	#elif UNITY_IPHONE || UNITY_TVOS
		// Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.

		return _isSpeaking(); 
	#endif
		return false;
	}

	public static float getDefaultRate(){

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			return osXgetDefaultRate();
#elif UNITY_IPHONE || UNITY_TVOS
			return _getDefaultRate ();
#endif
		return 0.0f;
	}
}