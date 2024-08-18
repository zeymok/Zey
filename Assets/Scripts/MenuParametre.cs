using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic; 

public class MenuParametre : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; 
    public Slider musiqueSlider;
    public Slider effetsSonoresSlider;
    public Button validerButton;
    public Button retourButton;
    public AudioMixer audioMixer;

    private List<Resolution> customResolutions;
    private int currentResolutionIndex;

    void Start()
    {
        
        customResolutions = new List<Resolution>
        {
            new Resolution { width = 1920, height = 1080 }, // HD
            new Resolution { width = 2560, height = 1440 }, // QHD
            new Resolution { width = 3840, height = 2160 }  // 4K
        };

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>(); 
        currentResolutionIndex = 0;

        for (int i = 0; i < customResolutions.Count; i++)
        {
            string option = customResolutions[i].width + " x " + customResolutions[i].height;
            options.Add(option);

            if (customResolutions[i].width == Screen.currentResolution.width &&
                customResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        musiqueSlider.onValueChanged.AddListener(ChangeVolumeMusique);
        effetsSonoresSlider.onValueChanged.AddListener(ChangeVolumeEffets);
        validerButton.onClick.AddListener(Valider);
        retourButton.onClick.AddListener(Retour);

        float musiqueVolume;
        float effetsVolume;
        audioMixer.GetFloat("MusiqueVolume", out musiqueVolume);
        audioMixer.GetFloat("EffetsVolume", out effetsVolume);
        musiqueSlider.value = musiqueVolume;
        effetsSonoresSlider.value = effetsVolume;
    }

    void ChangeResolution(int index)
    {
        Resolution resolution = customResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void ChangeVolumeMusique(float volume)
    {
        audioMixer.SetFloat("MusiqueVolume", volume);
    }

    void ChangeVolumeEffets(float volume)
    {
        audioMixer.SetFloat("EffetsVolume", volume);
    }

    void Valider()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetFloat("MusiqueVolume", musiqueSlider.value);
        PlayerPrefs.SetFloat("EffetsVolume", effetsSonoresSlider.value);
        PlayerPrefs.Save();
        Retour();
    }

    void Retour()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
