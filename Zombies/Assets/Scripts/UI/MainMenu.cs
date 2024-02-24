using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button PlayPec3Button;
    public Button PlayPecFinalButton;
    public Button QuitButton;
    public Button OptionsButton;
    public Button BackButton;

    public TMP_Dropdown LanguageDropdown;
    public Toggle GoreModeToggle;

    // Start is called before the first frame update
    void Start()
    {
        PlayPec3Button.onClick.AddListener(StartPec3ButtonClick);
        PlayPecFinalButton.onClick.AddListener(StartPecFinalButtonClick);
        QuitButton.onClick.AddListener(QuitButtonClick);
        OptionsButton.onClick.AddListener(OptionsButtonClick);
        BackButton.onClick.AddListener(BackButtonClick);
        GoreModeToggle.onValueChanged.AddListener(delegate { OnGoreModeChange(GoreModeToggle); });

        LanguageDropdown.onValueChanged.AddListener(delegate { OnLanguageChange(LanguageDropdown);  });
    }


    private void StartPec3ButtonClick()
    {
        SceneManager.LoadScene("Level1");
    }

    private void StartPecFinalButtonClick()
    {
        SceneManager.LoadScene("Level2");
    }

    private void OptionsButtonClick()
	{
        CameraController.Instance.SwitchCamera();
	}

    private void BackButtonClick()
	{
        CameraController.Instance.SwitchCamera();
    }

    private void OnLanguageChange(TMP_Dropdown dropdownChanged)
	{
        string languageValue = dropdownChanged.options[dropdownChanged.value].text;

        TranslationService.Language language = (languageValue == "English") ? TranslationService.Language.en : TranslationService.Language.es;

        GameConfiguration.SetLanguage(language);

        ObjectTranslator.Instance.Translate(language);
	}
    private void OnGoreModeChange(Toggle toggleChanged)
    {
        GameConfiguration.SetGoreMode(toggleChanged.isOn);
    }

    private void QuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
