using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public static event EventHandler OnPlayerDead;

	private int numRemainingZombies;
	private int numKilledZombies;

	public static LevelManager Instance;

	private void Start()
	{
		if (Instance != null)
		{
			Debug.LogError("There is another level manager.");
			return;
		}
		
		Time.timeScale = 1f;

		Instance = this;

		numRemainingZombies = 0;
		numKilledZombies = 0;

		ZombieController.OnAnyCreatedZombie += OnZombieCreated;
		ZombieController.OnAnyKilledZombie += OnZombieKilled;
		FinishLevel.OnFinishLevel += OnFinishLevel;
	}

	private void OnZombieCreated(object sender, ZombieController zombie)
	{
		numRemainingZombies++;
		if (StatWidget.Instance != null)
		{
			StatWidget.Instance.UpdateNumRemainingZombies(numRemainingZombies);
		}
	}

	private void OnZombieKilled(object sender, ZombieController zombie)
	{
		numRemainingZombies--;
		numKilledZombies++;
		StatWidget.Instance.UpdateNumRemainingZombies(numRemainingZombies);
		StatWidget.Instance.UpdateNumKilledZombies(numKilledZombies);
	}

	private void OnFinishLevel(object sender, EventArgs args)
	{
		Time.timeScale = 0;
	}

	public void FinishGame()
	{
		OnFinishLevel(null,EventArgs.Empty);
		OnPlayerDead?.Invoke(this, EventArgs.Empty);
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public int GetNumZombiesInGame() => numRemainingZombies;
}
