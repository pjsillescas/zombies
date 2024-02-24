using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private DamageableComponent player;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button GoToMainMenuButton;

    private TranslationService.Language language;

    // Start is called before the first frame update
    void Start()
    {
        DisableWidget();
        LevelManager.OnPlayerDead += OnPlayerDead;
        RestartButton.onClick.AddListener(Restart);
        GoToMainMenuButton.onClick.AddListener(GoToMainMenu);
        FinishLevel.OnFinishLevel += OnPlayerPassLevel;
        language = GameConfiguration.GetLanguage();
    }

    private void OnDestroy()
    {
        LevelManager.OnPlayerDead -= OnPlayerDead;
        FinishLevel.OnFinishLevel -= OnPlayerPassLevel;
    }

    private void OnPlayerPassLevel(object sender, EventArgs args)
    {
        EnableWidget(TranslationService.Instance.Translate("end-escaped", language) + " {0}");
    }

    private void Restart()
    {
        LevelManager.Instance.Restart();
    }

    private void GoToMainMenu()
    {
        LevelManager.Instance.GoToMainMenu();
    }

    private void OnPlayerDead(object sender, EventArgs args)
    {
        EnableWidget(TranslationService.Instance.Translate("end-dead",language) + " {0}");
    }

    private void EnableWidget(string title)
    {
        ShowCursor();
        TitleText.text = string.Format(title,TimerUI.Instance.GetFormattedCurrentTime());
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void DisableWidget()
    {
        HideCursor();
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
