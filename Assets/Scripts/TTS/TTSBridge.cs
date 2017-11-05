#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections;


public class TTSBridge : IDisposable 
{
	
	private AndroidJavaClass cls_SpeechActivity = new AndroidJavaClass("com.unity3d.Plugin.AndroidTTSActivity");
	
	public void Init() 
	{
		using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
		{
			using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
			{
					cls_SpeechActivity.CallStatic("Init", obj_Activity);
			}
		}
	}
	
	public bool isInitialized() {
		return cls_SpeechActivity.CallStatic<bool>("engineInitialized");
	}
	
	public void Speak(string Text) {
		cls_SpeechActivity.CallStatic("Speak", Text);
	}
	
	public bool isSpeaking() {
		return cls_SpeechActivity.CallStatic<bool>("isSpeaking");
	}
	
	public int SetPitch(float thepitch)
	{
		return cls_SpeechActivity.CallStatic<int>("setPitch", thepitch);	
	}
	public int SetSpeechRate(float therate)
	{
		return cls_SpeechActivity.CallStatic<int>("setSpeechRate", therate);	
	}	
	public int SetLanguage(string Text) {
		return cls_SpeechActivity.CallStatic<int>("setLanguage", Text );
	}
	public string GetLanguage() {
		return cls_SpeechActivity.CallStatic<string>("getLanguage" );
	}
	public string getAllLanguages() {
		return cls_SpeechActivity.CallStatic<string>("getAllLanguages" );
	}	
	public void Dispose() {
		cls_SpeechActivity.Dispose();
	}
}
#endif