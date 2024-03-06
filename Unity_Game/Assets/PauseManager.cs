using StarterAssets.ThirdPersonController.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject optionsScreen;

    [Header("Things to Disable")]
    [SerializeField] private ThirdPersonController controller;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Resolutions")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    bool isPaused = false;

    bool cursorVisibility;
    CursorLockMode cursorState;

    Resolution[] resolutions;

    // Temporary variables to store changes
    private float tempVolume;
    private int tempQualityIndex;
    private bool tempIsFullscreen;
    private int tempResolutionIndex;


    // Start is called before the first frame update
    void Start()
    {
        // Get the resolution dropdown specified for this computer
        resolutions = Screen.resolutions;
        //resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set the pause screen to be active
        isPaused = true;
        unPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }  
            else
            {
                unPause();
            }
        }
    }

    //Pause Screen
    public void Pause()
    {
        isPaused = true;

        cursorVisibility = Cursor.visible;
        cursorState = Cursor.lockState;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // disable player movement
        controller.enabled = false;
        // pause
        pauseScreen.SetActive(true);
        optionsScreen.SetActive(false);
        Time.timeScale = 0f;
    }

    public void unPause()
    {
        isPaused = false;

        Cursor.visible = cursorVisibility;
        Cursor.lockState = cursorState;

        // unpause
        pauseScreen.SetActive(false);
        optionsScreen.SetActive(false);
        Time.timeScale = 1f;
        // enable player movement
        controller.enabled = true;
    }

    public void Options()
    {
        pauseScreen.SetActive(false);
        optionsScreen.SetActive(true);

        // Set the temporary variables to the current settings
        audioMixer.GetFloat("volume", out tempVolume);
        tempQualityIndex = QualitySettings.GetQualityLevel();
        tempIsFullscreen = Screen.fullScreen;
        tempResolutionIndex = GetResolutionIndex(Screen.currentResolution);
    }

    private int GetResolutionIndex(Resolution resolution)
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == resolution.width && resolutions[i].height == resolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    public void Quit()
    {
        Application.Quit();
    }

    //Options Screen
    public void SetVolume(float volume)
    {
        tempVolume = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        tempQualityIndex = qualityIndex;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        tempIsFullscreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        tempResolutionIndex = resolutionIndex;
    }

    public void saveSettings()
    {
        // Apply settings changes
        audioMixer.SetFloat("volume", tempVolume);
        QualitySettings.SetQualityLevel(tempQualityIndex);
        Screen.fullScreen = tempIsFullscreen;
        Resolution resolution = resolutions[tempResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Set screens visibility
        optionsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }

    public void cancelSettings()
    {
        // Set screens visibility
        optionsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }
}
