using UnityEngine;

public static class MyPlayerPrefs
{
	private const int StorageFalse = 0;
	private const int StorageTrue = 1;

	public static string LEVEL = "LEVEL";
	public static string CHALLENGE_INDEX = "CINDEX";

	public static void SetBool (string key, bool value)
	{
		int intValue = value ? StorageTrue : StorageFalse;
		PlayerPrefs.SetInt (key, intValue);
	}

	public static void SetLevel (int lvl)
	{
		SetInt (LEVEL, lvl);
	}

	public static int GetLevel ()
	{
		return GetInt (LEVEL, 1);
	}

	public static void SetChallengeIndex (int cIdx)
	{
		SetInt (CHALLENGE_INDEX, cIdx);
	}

	public static int GetChallengeIndex ()
	{
		return GetInt (CHALLENGE_INDEX, 0);
	}

	//cal this method only when we start the level;
	public static void IncrementChallengeAttempt (int level,int cIndex)
	{
		/**
		 * L1:CI1 = "1:1:IC" // Level 1, repeat1  , Index 1 ,attempt 1 ,incomeplete
		 * L2:CI4 = "4:6:C" // Level 2, repeat4  , Index 4 ,attempt 6, comeplete
		 * */
		string key = "L" + level + ":CI" + cIndex;
		string data = GetString (key, "");
		if(!data.Equals("")){
			string[] details = data.Split (':');
			if(details[2].Equals("IC")){
				int attemptCount = int.Parse (details [1]);
				int repeatCount = int.Parse (details [0]);
				attemptCount += 1;
				SetString(key,repeatCount+":"+attemptCount+":IC");
			}else{
				int attemptCount = int.Parse (details [1]);
				int repeatCount = int.Parse (details [0]);
				repeatCount += 1;
				SetString(key,repeatCount+":1:IC");
			}
		}else{
			SetString(key,"1:1:IC");
		}
	}

	//cal this method only when we complete the level;
	public static void SetChallengeAttempt (int level,int cIndex)
	{
		string key = "L" + level + ":CI" + cIndex;
		string data = GetString (key, "");
		if(!data.Equals("")){
			string[] details = data.Split (':');
			int attemptCount = int.Parse (details [1]);
			int repeatCount = int.Parse (details [0]);
			SetString(key,repeatCount+":"+attemptCount+":C");

		}
	}

	public static string GetChallengeAttempt(int level,int cIndex){
		string key = "L" + level + ":CI" + cIndex;
		return GetString (key, "");
	}



	public static bool GetBool (string key)
	{
		int intValue = PlayerPrefs.GetInt (key);
		return intValue == StorageTrue;
	}

	public static void SetInt (string key, int value)
	{
		PlayerPrefs.SetInt (key, value);
	}

	public static int GetInt (string key, int defaultValue)
	{
		return PlayerPrefs.GetInt (key, defaultValue);
	}

	public static void SetFloat (string key, float value)
	{
		PlayerPrefs.SetFloat (key, value);
	}

	public static float GetFloat (string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat (key, defaultValue);
	}

	public static void SetString (string key, string value)
	{
		PlayerPrefs.SetString (key, value);
	}

	public static string GetString (string key, string defaultValue)
	{
		return PlayerPrefs.GetString (key, defaultValue);
	}

	public static bool HasKey (string key)
	{
		return PlayerPrefs.HasKey (key);
	}

	public static void DeleteAll ()
	{
		PlayerPrefs.DeleteAll ();
	}

	public static void DeleteKey (string key)
	{
		PlayerPrefs.DeleteKey (key);
	}

	private static void Save ()
	{
		PlayerPrefs.Save ();
	}
}