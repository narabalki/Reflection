using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour, IPointerClickHandler{

	public Toggle playSound;
	public Slider voiceSpeed;
	public Dropdown voiceName;
	public Dropdown language;
	public Dropdown level,index;
	public Text buildNum;

	public void Start() 
	{
		Settings.instance.RetriveSettings ();
		playSound.isOn = ((int)Settings.instance.soundEffects == 1);
		voiceSpeed.value = Settings.instance.voiceSpeed;
		voiceName.value = (int)Settings.instance.voice;
		language.value = (int)Settings.instance.language;
		buildNum.text = "Build number : " + Settings.buildNumber;

		playSound.onValueChanged.AddListener (delegate { OnToggleSound ();});
		voiceSpeed.onValueChanged.AddListener (delegate { OnVoiceSpeedChange ();});
		voiceName.onValueChanged.AddListener (delegate { OnVoiceChange ();});
		language.onValueChanged.AddListener (delegate { OnLanguageChange ();});
		level.onValueChanged.AddListener (delegate { OnLevelChange ();});
		index.onValueChanged.AddListener (delegate { OnIndexChange ();});

	}

	public void OnToggleSound() 
	{
		Settings.instance.soundEffects =  (Switch) (playSound.isOn ? 1 : 0);
		Settings.instance.SaveSettings ();

		Dictionary<string, object> _properties = new Dictionary<string, object>();
		_properties.Add ("SettingType", "SoundFX");
		_properties.Add ("SettingValue", playSound.isOn ?  "On" : "Off" );
		Mixpanel.instance.SendEvent (MXPStrings.SETTINGS_CHANGED, _properties);
	}

	public void OnVoiceSpeedChange() 
	{
		Settings.instance.voiceSpeed =  (int) voiceSpeed.value;
		Settings.instance.SaveSettings ();

		Dictionary<string, object> _properties = new Dictionary<string, object>();
		_properties.Add ("SettingType", "Voice Speed");
		_properties.Add ("SettingValue", Settings.instance.voiceSpeed);
		Mixpanel.instance.SendEvent (MXPStrings.SETTINGS_CHANGED, _properties);
	}

	public void OnVoiceChange() 
	{
		Settings.instance.voice = (Voice)voiceName.value;
		Settings.instance.SaveSettings ();

		Dictionary<string, object> _properties = new Dictionary<string, object>();
		_properties.Add ("SettingType", "Voice");
		_properties.Add ("SettingValue", Settings.instance.voice);
		Mixpanel.instance.SendEvent (MXPStrings.SETTINGS_CHANGED, _properties);
	}

	public void OnLanguageChange() 
	{
		Settings.instance.language = (Language)language.value;
		Settings.instance.SaveSettings ();

		Settings.instance.SetSmartLanguage();

		Dictionary<string, object> _properties = new Dictionary<string, object>();
		_properties.Add ("SettingType", "Language");
		_properties.Add ("SettingValue", language.ToString());
		Mixpanel.instance.SendEvent (MXPStrings.SETTINGS_CHANGED, _properties);
	}
	public void OnLevelChange() 
	{
		MyPlayerPrefs.SetLevel (level.value+1);

		Dictionary<string, object> _properties = new Dictionary<string, object>();
		_properties.Add ("SettingType", "Level");
		_properties.Add ("SettingValue", level.ToString());
		Mixpanel.instance.SendEvent (MXPStrings.SETTINGS_CHANGED, _properties);
	}
	public void OnIndexChange() 
	{
		MyPlayerPrefs.SetChallengeIndex (index.value+1);

		Dictionary<string, object> _properties = new Dictionary<string, object>();
		_properties.Add ("SettingType", "Index");
		_properties.Add ("SettingValue", index.ToString());
		Mixpanel.instance.SendEvent (MXPStrings.SETTINGS_CHANGED, _properties);
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		LevelManager.instance.showSettings = false;
		gameObject.SetActive (false);

		Mixpanel.instance.SendEvent (MXPStrings.SETTINGS_EXITED);
	}
	#endregion
}
