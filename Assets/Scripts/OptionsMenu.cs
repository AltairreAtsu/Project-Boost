using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private Dropdown qualityDropdown;
	[SerializeField] private Dropdown resolutionDropDown;
	[SerializeField] private Toggle fullScreenToggle;

	private Resolution[] supportedResolutions;

	private void Start()
	{
		qualityDropdown.value = QualitySettings.GetQualityLevel();

		fullScreenToggle.isOn = Screen.fullScreen;
		supportedResolutions = Screen.resolutions;

		resolutionDropDown.ClearOptions();

		List<string> resolutionOptions = new List<string>();
		int currentResolutionIndex = 0;

		for (int i = 0; i < supportedResolutions.Length; i++)
		{
			resolutionOptions.Add(supportedResolutions[i].width + "x" + supportedResolutions[i].height);

			if (supportedResolutions[i].width == Screen.currentResolution.width &&
				supportedResolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropDown.AddOptions(resolutionOptions);
		resolutionDropDown.value = currentResolutionIndex;
	}

	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("volume", volume);
	}

	public void SetQualityLevel(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = supportedResolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void SetFullScreen(bool fullScreenValue)
	{
		Screen.fullScreen = fullScreenValue;
		if (!fullScreenValue)
		{
			Resolution resolution = Screen.currentResolution;
			Screen.SetResolution(resolution.width, resolution.height, fullScreenValue);
		}
		
	}
}
