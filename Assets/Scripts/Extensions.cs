using System;
using UnityEngine;
using SmartLocalization;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods
{
	public static T[] SubArray<T>(this T[] data, int index, int length)
	{
		T[] result = new T[length];
		Array.Copy(data, index, result, 0, length);
		return result;
	}

	public static string temporaryCachePath = "";
	public static string persistentDataPath = "";

	public static void InitDataPaths()
	{
		temporaryCachePath = Application.temporaryCachePath;
		persistentDataPath = Application.persistentDataPath;
	}

	public static string AppWritableDirectory(){
		#if UNITY_TVOS
		return temporaryCachePath;
		#else
		return persistentDataPath;
		#endif
	}

	public static string AppCustomizationDirectory()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		return temporaryCachePath;
		#elif UNITY_IOS || UNITY_TVOS
//		if (!Settings.instance.isiCloudEnabled && !IOSUtilities.GetiCloudStatus()) {
		#if UNITY_TVOS
		return temporaryCachePath;
		#else
		return temporaryCachePath;
		#endif
//		} else {
		#if UNITY_TVOS
		return temporaryCachePath;
		#else
		//return IOSUtilities.GetiCloudPath (temporaryCachePath);
		#endif
//		}
		#endif
	}
			

	public static Bounds GetMaxBounds(this GameObject g) {
		Bounds b = new Bounds(g.transform.position, Vector3.zero);
		foreach (Renderer r in g.GetComponentsInChildren<Renderer>()) {
			b.Encapsulate(r.bounds);
		}
		return b;
	}

	public static void TimeThis(Action k,string start ="Starting",string end="Completed"){
		System.Diagnostics.Stopwatch  fsTimer = new System.Diagnostics.Stopwatch ();
		Debug.Log (start);
		fsTimer.Start ();
		k ();
		fsTimer.Stop ();
		Debug.Log (end+" took "+fsTimer.ElapsedMilliseconds+" ms");
	}

	public static Sprite GetLocalizedTexture (this string key, Sprite originalSprite)
	{
		Texture2D texture = LanguageManager.Instance.GetTexture (key) as Texture2D;
		if (texture != null) {
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0,0));
		} else {
			return originalSprite;
		}
	}

	public static string GetLocalizedText (this string key)
	{
		return LanguageManager.Instance.GetTextValue(key);
	}

	private static System.Random rng = new System.Random();  

	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}


}


