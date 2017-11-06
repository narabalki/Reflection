using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Net;

public class Mixpanel : MonoBehaviour
{

	#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
	private static void mixpanelSetToken(string mixToken){
		//Debug.Log ("Mixpanel token  " + mixToken);
	}
	private static void mixpanelSetSuperProperties (string superKeys, string superValues){
		//Debug.Log ("Mixpanel superKeys  " + superKeys);
		//Debug.Log ("Mixpanel superValues  " + superValues);
	}
	private static void mixpanelTrack (string eventName){
		//Debug.Log ("Mixpanel eventName  " + eventName);
	}
	private static void mixpanelTrackWithProps (string eventName, string propsKeys, string propsValues){
		//Debug.Log ("Mixpanel eventName  " + eventName);
		//Debug.Log ("Mixpanel propsKeys  " + propsKeys);
		//Debug.Log ("Mixpanel propsValues  " + propsValues);
	}

	private static void mixpanelUpdateProfile (string dictinct_id, string propsKeys, string propsValues){
		//Debug.Log ("Mixpanel dictinct_id  " + dictinct_id);
		//Debug.Log ("Mixpanel propsKeys  " + propsKeys);
		//Debug.Log ("Mixpanel propsValues  " + propsValues);
	}
	#else
	[DllImport ("__Internal")] private static extern void mixpanelSetToken(string mixToken);
	[DllImport ("__Internal")] private static extern void mixpanelSetSuperProperties (string superKeys, string superValues);
	[DllImport ("__Internal")] private static extern void mixpanelTrack (string eventName);
	[DllImport ("__Internal")] private static extern void mixpanelTrackWithProps (string eventName, string propsKeys, string propsValues);
	[DllImport ("__Internal")] private static extern void mixpanelUpdateProfile (string dictinct_id, string propsKeys, string propsValues);
	#endif
	public static Mixpanel instance;

	// Set this to your Mixpanel token.
	public static string Token = null;

	// Set this to the distinct ID of the current user.
	public static string DistinctID;

	// Set to true to enable debug logging.
	public static bool EnableLogging;

	// Add any custom "super properties" to this dictionary. These are properties sent with every event.
	public static Dictionary<string, object> SuperProperties = new Dictionary<string, object>();

	private const string API_URL_FORMAT = "https://api.mixpanel.com/track/?data={0}";
	private const string API_URL_ENGAGE = "https://api.mixpanel.com/engage/?data={0}";
	#if UNITY_STANDALONE_OSX
	private const string APP_VERSION = "1.1.7";
	private static int testingInt = 0;
	#endif

	public static void SetToken(string tokenKey){
		Token = tokenKey;
#if UNITY_TVOS || UNITY_STANDALONE_OSX
#else
		mixpanelSetToken (Token);
#endif

	}

	void Awake ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}

	public void InitializeMixpanel ()
	{


		SetToken ("0544151710fa676c7a2db8fc1ba6a1d0");//this id is Debug id
//		SetToken ("885966fdad4e1668b16802850f07b799");//this id is iOS LIVE id
#if UNITY_TVOS || UNITY_STANDALONE_OSX

		//Release TOKEN
//		SetToken("7218ce758ba9de35a14cd7834cd0e555");

		 //Set some "super properties" to be sent with every event.
		Mixpanel.SuperProperties ["platform"] = Application.platform.ToString ();
		Mixpanel.SuperProperties ["quality"] = QualitySettings.names [QualitySettings.GetQualityLevel ()];
		Mixpanel.SuperProperties ["resolution"] = Screen.width + "x" + Screen.height;
		Mixpanel.SuperProperties ["Device type"] = SystemInfo.deviceModel;
		Mixpanel.SuperProperties ["App version"] = APP_VERSION;

#endif		
		//Comment these two lines for production build
		SuperProperties ["QA Build"] = false;
		
		//Uncomment this line for production build
		//Mixpanel.SuperProperties["QA Build"] = false;
#if UNITY_STANDALONE_OSX		
		updateProfile(new Dictionary<string,object>(){{"App version",APP_VERSION},{"Device Unique Id",SystemInfo.deviceUniqueIdentifier},{"Processor Type",SystemInfo.processorType}});
#else
		updateProfile();
#endif
	}
	// Call this to send an event to Mixpanel.
	// eventName: The name of the event. (Can be anything you'd like.)
	// properties: A dictionary containing any properties in addition to those in the Mixpanel.SuperProperties dictionary.
	public void SendEvent(string eventName, IDictionary<string, object> properties = null)
	{
		

		if (Token == null)
			InitializeMixpanel ();
		if(string.IsNullOrEmpty(Token))
		{
			Debug.LogError("Attempted to send an event without setting the Mixpanel.Token variable.");
			return;
		}
		
		if(string.IsNullOrEmpty(DistinctID))
		{
			if(!PlayerPrefs.HasKey("mixpanel_distinct_id"))
				PlayerPrefs.SetString("mixpanel_distinct_id", Guid.NewGuid().ToString());
			DistinctID = PlayerPrefs.GetString("mixpanel_distinct_id");
		}
		Dictionary<string, object> propsDict = new Dictionary<string, object>();
		propsDict.Add("distinct_id", DistinctID);
		propsDict.Add("token", Token);

		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		int cur_time = Convert.ToInt32((System.DateTime.UtcNow - epochStart).TotalSeconds);
		propsDict.Add("time", cur_time.ToString());

		foreach(var kvp in SuperProperties)
		{
			if(kvp.Value is float) // LitJSON doesn't support floats.
			{
				float f = (float)kvp.Value;
				double d = f;
				propsDict.Add(kvp.Key, d);
			}
			else
			{
				propsDict.Add(kvp.Key, kvp.Value);
			}
		}
		if(SuperProperties.ContainsKey("Active UNL")){
			string unl = (string)SuperProperties ["Active UNL"];
			if (unl.Length > 255) {
				int count = 0;
				int startIndex = 0;
				int endIndex = 255;
				while (startIndex < unl.Length) {
					count++;
					propsDict.Add ("Active UNL" + count, unl.Substring (startIndex));
					startIndex = startIndex + endIndex;
				}
			}
		}
		if(properties != null)
		{
			foreach(var kvp in properties)
			{
				if(kvp.Value is float) // LitJSON doesn't support floats.
				{
					float f = (float)kvp.Value;
					double d = f;
					propsDict.Add(kvp.Key, d);
				}
				else
				{
					propsDict.Add(kvp.Key, kvp.Value);
				}
			}
		}

		string keys = "", values = "";
		bool isFirst = true;
		foreach (var kvp in propsDict) {
			if(isFirst){
				keys = kvp.Key;
				values = kvp.Value.ToString();
				isFirst = false;
			}else{
				keys = keys+"*##*"+kvp.Key;
				values = values+"*##*"+kvp.Value.ToString();
			}
		}

		#if UNITY_TVOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
		Dictionary<string, object> jsonDict = new Dictionary<string, object>();
		jsonDict.Add("event", eventName);
		jsonDict.Add("properties", propsDict);

		string jsonStr = JsonMapper.ToJson(jsonDict);
		if(EnableLogging)
			Debug.Log("Sending mixpanel event: " + jsonStr);
		string jsonStr64 = EncodeTo64(jsonStr);
		string url = string.Format(API_URL_FORMAT, jsonStr64);
		StartCoroutine(SendEventCoroutine(url));
#else
		mixpanelTrackWithProps (eventName, keys, values);
		#endif
	}

	public void registerSuperProperties(Dictionary<string ,object> propsDict){
		if(propsDict == null)
			propsDict = new Dictionary<string, object>();
		
		string keys = "", values = "";
		bool isFirst = true;
		foreach (var kvp in propsDict) {
			if(isFirst){
				keys = kvp.Key;
				values = kvp.Value.ToString();
				isFirst = false;
			}else{
				keys = keys+"*##*"+kvp.Key;
				values = values+"*##*"+kvp.Value.ToString();
			}
			#if UNITY_TVOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
			SuperProperties[kvp.Key] = kvp.Value;
			#endif
		}
#if UNITY_TVOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
#else
		mixpanelSetSuperProperties (keys, values);
#endif
	}

	public void updateProfile( Dictionary<string ,object> propsDict = null)
	{
		if (Token == null)
			InitializeMixpanel ();
		if(string.IsNullOrEmpty(Token))
		{
			Debug.LogError("Attempted to send an event without setting the Mixpanel.Token variable.");
			return;
		}
		
		if(string.IsNullOrEmpty(DistinctID))
		{
			if(!PlayerPrefs.HasKey("mixpanel_distinct_id"))
				PlayerPrefs.SetString("mixpanel_distinct_id", Guid.NewGuid().ToString());
			DistinctID = PlayerPrefs.GetString("mixpanel_distinct_id");
		}

		if(propsDict == null)
			propsDict = new Dictionary<string, object>();

		string keys = "", values = "";
		bool isFirst = true;
		foreach (var kvp in propsDict) {
			if(isFirst){
				keys = kvp.Key;
				values = kvp.Value.ToString();
				isFirst = false;
			}else{
				keys = keys+"*##*"+kvp.Key;
				values = values+"*##*"+kvp.Value.ToString();
			}
		}

#if UNITY_TVOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
		Dictionary<string, object> jsonDict = new Dictionary<string, object>();
		jsonDict.Add("$token", Token);
		jsonDict.Add("$distinct_id", DistinctID);
		jsonDict.Add("$set", propsDict);
		string jsonStr = JsonMapper.ToJson(jsonDict);
		if(EnableLogging)
			Debug.Log("Sending mixpanel event: " + jsonStr);
		string jsonStr64 = EncodeTo64(jsonStr);
		string url = string.Format(API_URL_ENGAGE, jsonStr64);
		StartCoroutine(SendEventCoroutine(url));
		SuperProperties["Device Model"] = SystemInfo.deviceModel;
#else
		mixpanelUpdateProfile (DistinctID, keys, values);
#endif
	}

	private string EncodeTo64(string toEncode)
	{
		var toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
		var returnValue = Convert.ToBase64String(toEncodeAsBytes);
		return returnValue;
	}

	private IEnumerator SendEventCoroutine(string url)
	{
		Thread thread = new Thread(() => HttpGet(url));
		thread.Start ();
		yield return null;
	}

	private void HttpGet(string url) {
		HttpWebRequest req = WebRequest.Create(url)
			as HttpWebRequest;
		string result = null;
		using (HttpWebResponse resp = req.GetResponse()
			as HttpWebResponse)
		{
			StreamReader reader =
				new StreamReader(resp.GetResponseStream());
			result = reader.ReadToEnd();
		}
		Debug.Log ("Mixpanel HTTP Response: "+result);
	}
}