using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public class IOSUtilities : MonoBehaviour
{
	#if UNITY_TVOS
	[DllImport("__Internal")]
	private static extern string mGetClipBoardString();
	
	[DllImport("__Internal")]
	private static extern void mSetClipBoardString(string appleAppId);
	
	[DllImport("__Internal")]
	private static extern void mSetLanguageString(string appleAppId);

	[DllImport("__Internal")]
	private static extern void _ShareSimpleText (string message);

	[DllImport("__Internal")]
	private static extern void _ShareTextWithImage (string message,string imagePath);

	[DllImport("__Internal")]
	private static extern string mGetDeviceLanguage();

	[DllImport("__Internal")]
	private static extern bool mGetiCloudStatus();

	[DllImport("__Internal")]
	private static extern string mGetiCloudPath(string persistent_path);

	[DllImport("__Internal")]
	private static extern void mStartiCloud();

	[DllImport("__Internal")]
	private static extern void mSaveSettingsToCustomFolder();

	[DllImport("__Internal")]
	private static extern void mNotifyUnity (notificationCallback n);

	public delegate void notificationCallback();

	[MonoPInvokeCallback(typeof(notificationCallback))]
	public static void callbackImplementation() {
		Debug.Log("Syncing with iCloud");
	}

	public static void SetClipBoardString(string key){
		mSetClipBoardString(key);
	}
	
	public static string GetClipBoardString () {
		return mGetClipBoardString ();
	}
	
	public static void shareMessage( string message,string imagePath) {
//		_ShareSimpleText(message);
		Debug.Log ("Sharing message and image");
		_ShareTextWithImage(message,imagePath);
	}

	public static void shareMessage( string message) {
		Debug.Log ("Sharing message");
		_ShareSimpleText(message);
	}
	
	public static void SetLanguage(string language){
		mSetLanguageString (language);
	}

	public static string GetDeviceLanguage () {
		return mGetDeviceLanguage ();
	}

	public static bool GetiCloudStatus (){
		return mGetiCloudStatus ();
	}

	public static string GetiCloudPath (string persistent_path){
		return mGetiCloudPath (persistent_path);
	}

	public static void RegisterForNotification(){
		mNotifyUnity (callbackImplementation);
	}

	public static void StartiCloud(){
		mStartiCloud ();
	}

	public static void SaveSettingsToCustomFolder (){
		mSaveSettingsToCustomFolder ();
	}

	#endif

}

