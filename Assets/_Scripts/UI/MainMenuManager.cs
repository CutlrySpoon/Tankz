using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static TileMapManager;

// This is a manager script that controls main menu UI and data transfer.

// By OS.

public class MainMenuManager : MonoBehaviour
{
    // References to UI elements
    [SerializeField]
    private GameObject PlayPanel;

    [SerializeField]
    private TMP_Dropdown mapTypeDropdown;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private TextMeshProUGUI volumeText;

    [SerializeField]
    private Slider scaleSlider;

    [SerializeField]
    private TextMeshProUGUI scaleText;

    [SerializeField]
    private TMP_InputField seedText;
    [Space(10)]

    [SerializeField]
    private string gameSceneName;


    // Map data to pass onto map generator.
    private float volume = 0.5f;
    private float scale = 12f;
    private int seed = 0;

    
    // Set slider settings, update UI and generate default map.
    private void Start()
    {
        PlayPanel.SetActive(false);
        volumeSlider.minValue = tm.minNoiseVolume;
        volumeSlider.maxValue = tm.maxNoiseVolume;

        scaleSlider.minValue = tm.minNoiseScale;
        scaleSlider.maxValue = tm.maxNoiseScale;

        SetMapType(0);
        SetSliderUI();

        seed = Random.Range(0, 99999);
        seedText.text = seed.ToString();

        OnGenerateButtonClick();
    }


    #region Buttons

    public void OnPlayButtonClick()
    {
        PlayPanel.SetActive(!PlayPanel.activeSelf);
    }

    public void OnCloseButtonClick()
    {
        PlayPanel.SetActive(false);
    }

    public void OnRandomiseSeedButtonClick()
    {
        seed = Random.Range(0, 99999);
        seedText.text = seed.ToString();
    }

    public void OnGenerateButtonClick()
    {
        tm.SetMap(volume, scale, seed, true);
    }

    public void OnStartGameButtonClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    #endregion

    //Sliders
    public void OnVolumeSliderChanged()
    {
        volume = volumeSlider.value;

        float norVol = Mathf.InverseLerp(volumeSlider.minValue, volumeSlider.maxValue, volume);
        volumeText.text = $"{norVol:F2}";
    }

    public void OnScaleSliderChanged()
    {
        scale = scaleSlider.value;
        float norScale = Mathf.InverseLerp(scaleSlider.minValue, scaleSlider.maxValue, scale);
        scaleText.text = $"{norScale:F2}";
    }


    // Set the map settings based on map present and display sliders based on selected preset.
    public void SetMapType (int index)
    {
        switch (index)
        {
            case 0:
                volume = 0.5f;
                scale = 12f;

                scaleSlider.gameObject.SetActive(false);
                volumeSlider.gameObject.SetActive(false);
                break;

            case 1:
                volume = 0.35f;
                scale = 14f;

                scaleSlider.gameObject.SetActive(false);
                volumeSlider.gameObject.SetActive(false);
                break;

            case 2:
                volume = 0.55f;
                scale = 8f;

                scaleSlider.gameObject.SetActive(false);
                volumeSlider.gameObject.SetActive(false);
                break;

            case 3:
                scaleSlider.gameObject.SetActive(true);
                volumeSlider.gameObject.SetActive(true);
                break;
        }

        SetSliderUI();
    }

    // Update slider UI values.
    private void SetSliderUI()
    {
        volumeSlider.value = volume;
        float norVol = Mathf.InverseLerp(volumeSlider.minValue, volumeSlider.maxValue, volume);
        volumeText.text = $"{norVol:F2}";

        scaleSlider.value = scale;
        float norScale = Mathf.InverseLerp(scaleSlider.minValue, scaleSlider.maxValue, scale);
        scaleText.text = $"{norScale:F2}";
    }

    // Update seed UI.
    public void OnSeedValueChange(string value)
    {
        if (value.Length != 0)
        {
            seed = int.Parse(value);
            Debug.Log(seed);
        }
    }
}
