using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider sfxVolslider;

    [SerializeField] private GameObject postProcessingObject;
    [SerializeField] private Toggle postProcessingToggle;

    [SerializeField] private GameObject swipeLineObject;
    [SerializeField] private GameObject developerInfoTexts;

    [SerializeField] private GameObject FPSDisplay;
    [SerializeField] private Toggle FPSDisplayToggle;

    [SerializeField] private Toggle ShowTutorialToggle;

    [SerializeField] private Toggle noAdsToggle;

    // Start is called before the first frame update
    void Start()
    {
        float musicVol = PlayerPrefs.GetFloat("musicVol", 0);
        float sfxVol = PlayerPrefs.GetFloat("sfxVol", 0);

        masterMixer.SetFloat("musicVol", musicVol);
        masterMixer.SetFloat("sfxVol", sfxVol);

        musicVolSlider.SetValueWithoutNotify(musicVol);
        sfxVolslider.SetValueWithoutNotify(sfxVol);

        if(PlayerPrefs.GetInt("postProcessing", 1) == 0)
        {
            if (postProcessingObject != null) { postProcessingObject.SetActive(false); }
            if (postProcessingToggle != null) { postProcessingToggle.SetIsOnWithoutNotify(false); }
        }
        else
        {
            if (postProcessingObject != null) { postProcessingObject.SetActive(true); }
            if (postProcessingToggle != null) { postProcessingToggle.SetIsOnWithoutNotify(true); }
        }

        if(PlayerPrefs.GetInt("fpsDisplay", 0) == 0)
        {
            if (FPSDisplay != null) { FPSDisplay.SetActive(false); }
            if (FPSDisplayToggle != null) { FPSDisplayToggle.SetIsOnWithoutNotify(false); }
        }
        else
        {
            if (FPSDisplay != null) { FPSDisplay.SetActive(true); }
            if (FPSDisplayToggle != null) { FPSDisplayToggle.SetIsOnWithoutNotify(true); }
        }

        if(ShowTutorialToggle != null)
        {
            if(PlayerPrefs.GetInt("tutorialIsOn", 1) == 1)
            {
                ShowTutorialToggle.SetIsOnWithoutNotify(true);
            }
            else
            {
                ShowTutorialToggle.SetIsOnWithoutNotify(false);
            }
        }

        if (noAdsToggle != null)
        {
            if (PlayerPrefs.GetInt("noAds", 0) == 1)
            {
                noAdsToggle.SetIsOnWithoutNotify(true);
            }
            else
            {
                noAdsToggle.SetIsOnWithoutNotify(false);
            }
        }
    }

    public void SetMusicVol(float vol)
    {
        masterMixer.SetFloat("musicVol", vol);
        PlayerPrefs.SetFloat("musicVol", vol);
    }

    public void SetSfxVol(float vol)
    {
        masterMixer.SetFloat("sfxVol", vol);
        PlayerPrefs.SetFloat("sfxVol", vol);
    }

    public void PostProcessingToggleClicked(bool boolean)
    {
        if(boolean)
        {
            PlayerPrefs.SetInt("postProcessing", 1);
        }
        else
        {
            PlayerPrefs.SetInt("postProcessing", 0);
        }
        if(postProcessingObject != null)
        {
            postProcessingObject.SetActive(boolean);
        }
    }

    public void SwipeLineToggleClicked(bool boolean)
    {
        swipeLineObject.SetActive(boolean);
        developerInfoTexts.SetActive(boolean);
    }

    public void NoAdsToggleClicked(bool boolean)
    {
        if(boolean)
        {
            PlayerPrefs.SetInt("noAds", 1);
        }
        else
        {
            PlayerPrefs.SetInt("noAds", 0);
        }
    }

    public void FPSDisplayToggleClicked(bool boolean)
    {
        if (boolean)
        {
            PlayerPrefs.SetInt("fpsDisplay", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fpsDisplay", 0);
        }
        if (FPSDisplay != null)
        {
            FPSDisplay.SetActive(boolean);
        }
    }

    public void ShowTutorialToggleClicked(bool boolean)
    {
        if (boolean)
        {
            PlayerPrefs.SetInt("tutorialIsOn", 1);
        }
        else
        {
            PlayerPrefs.SetInt("tutorialIsOn", 0);
        }
    }
}
