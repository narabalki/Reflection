using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class JsonHelper
{
	private  Dictionary<string, object> dic = null;

	public  object Deserialize(string json)
	{
		return ToObject(JToken.Parse(json));
	}

	private  object ToObject(JToken token)
	{
		switch (token.Type)
		{
		case JTokenType.Object:
			return token.Children<JProperty>()
				.ToDictionary(prop => prop.Name,
					prop => ToObject(prop.Value));

		case JTokenType.Array:
			return token.Select(ToObject).ToList();

		default:
			return ((JValue)token).Value;
		}
	}

	public  Dictionary<string, object> ReadAndDeserializeJson(string jsonPath, bool isEncrypted)
	{
		try{
			string jsonString = FileUtils.ReadContent(jsonPath, isEncrypted);
			object data = Deserialize (jsonString);
			dic = data as Dictionary<string, object>;
		}catch(Exception e) {
			dic = new Dictionary<string, object> ();
		}
		return dic.ToDictionary(entry => entry.Key, entry => entry.Value);
	}

	public  void SaveData(string jsonPath, bool encode, string key, object value) 
	{
		dic [key] = value;
		string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject (dic, Newtonsoft.Json.Formatting.Indented);
		FileUtils.WriteContent (jsonString, jsonPath, encode);
	}
}



