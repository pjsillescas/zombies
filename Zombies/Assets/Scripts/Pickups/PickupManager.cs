using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		IPickup pickup = GetComponent<IPickup>();

		if (pickup != null && pickup.ApplyPickup(other.gameObject))
		{
			var audioSource = GetComponent<AudioSource>();
			if (audioSource != null)
			{
				audioSource.Play();
				Destroy(gameObject, audioSource.clip.length / 4);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}