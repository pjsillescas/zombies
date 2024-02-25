using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
	public event EventHandler OnTimerTick;

	[SerializeField] private TextMeshProUGUI Text;
	[SerializeField] private int TimerTickSeconds = 60;
	public static TimerUI Instance { get; private set; }

	private float currentTime = 0;
	private float currentTimeToTick;
	private bool isTicking;

	public string GetFormattedCurrentTime()
	{
		int currentTimeInt = Mathf.FloorToInt(currentTime);
		int minutes = currentTimeInt / 60;
		int seconds = currentTimeInt % 60;

		return string.Format("{00:00}:{1:00}", minutes, seconds);
	}

	private void FormatCurrentTime()
	{
		Text.text = GetFormattedCurrentTime();
	}

	private void Start()
	{
		if (Instance != null)
		{
			Debug.LogError("There is another TimerUI.");
			return;
		}

		Instance = this;
		Text.text = "";
		isTicking = true;
		currentTimeToTick = TimerTickSeconds;
	}

	public void Stop()
	{
		isTicking = false;
	}

	public void Resume()
	{
		isTicking = true;
	}

	private void Update()
	{
		if (isTicking)
		{
			currentTime += Time.deltaTime;
			FormatCurrentTime();

			currentTimeToTick -= Time.deltaTime;
			if (currentTimeToTick <= 0)
			{
				OnTimerTick?.Invoke(this, EventArgs.Empty);
				currentTimeToTick = TimerTickSeconds;
			}
		}
	}
}
