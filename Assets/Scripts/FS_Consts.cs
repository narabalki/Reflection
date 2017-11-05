using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

	public class FS_Consts
{
	public const int TFX_NAME_LIMIT_LARGE	= 15;

	public const string DATABASE_VERSION 	= "Database_Version";
	public const string APP_VERSION 		= "App_Version";

	public const string PATH_NEW_IMAGES 	= "NewImages"; //ExtensionMethods.AppWritableDirectory() + "/" + FS_Consts.PATH_NEW_IMAGES + "/"
	
	public const string CAT_PEOPLE_ID				= "949109";
	public const string CAT_ACTION_ID				= "987996";
	public const string CAT_DESCRIBE_ID				= "931820";
	public const string CAT_TIME_EVENTS_ID			= "905210";
	public const string CAT_MOST_USED_INPEOPLE_ID	= "908114";

	//====================  Word Types ====================//
	public const string WORD_VERB 		= "V";
	public const string WORD_NOUN 		= "N";
	public const string WORD_ADJECTIVE 	= "J";
	public const string WORD_ADVERB 	= "A";
	public const string WORD_QUESTION 	= "Q";
	public const string WORD_NOT_APPLICABLE = "NONE";
	
	//=====================================================//
	public const string TYPE_TEMPLATE 	= "T";
	public const string TYPE_CATEGORY 	= "D";

	public const string ANIM_PARENT_NAME 	= "TempAnimParent_";

}

public static class EnumEx
{
	public static T GetValueFromDescription<T>(string description)
	{
		var type = typeof(T);
		if (!type.IsEnum)
			return default(T);
		foreach(var field in type.GetFields())
		{
			var attribute =  Attribute.GetCustomAttribute(field,
				typeof(DescriptionAttribute)) as DescriptionAttribute;
			if(attribute != null)
			{
				if(attribute.Description == description)
					return (T)field.GetValue(null);
			}
			else
			{
				if(field.Name == description)
					return (T)field.GetValue(null);
			}
		}
		return default(T);
	}

	public static string GetEnumDescription<T>(T value)
	{
		FieldInfo fi = value.GetType().GetField(value.ToString());

		DescriptionAttribute[] attributes =
			(DescriptionAttribute[])fi.GetCustomAttributes(
				typeof(DescriptionAttribute),
				false);

		if (attributes != null &&
			attributes.Length > 0)
			return attributes[0].Description;
		else
			return value.ToString();
	}
}
