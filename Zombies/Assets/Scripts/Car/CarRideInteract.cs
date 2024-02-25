using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRideInteract : MonoBehaviour, IInteractable
{
	[SerializeField] private PecCarUserControl carUserControl;

	public void Interact(GameObject caller)
	{
		if (caller.TryGetComponent(out PlayerController playerController))
		{
			carUserControl.SetRider(playerController);
			carUserControl.Possess();
		}

	}
}
