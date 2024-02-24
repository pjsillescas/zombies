using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
	public static event EventHandler OnFinishLevel;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("finished " + other.tag);
		if (other.tag == "Player")
		{
			OnFinishLevel?.Invoke(this, EventArgs.Empty);
		}
	}
}
