using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private UserInput userInput;
	private IInteractable interactable;

	private void Awake()
	{
		userInput = new UserInput();
		userInput.Enable();
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out IInteractable interactable))
		{
				this.interactable = interactable;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		interactable = null;
	}

	private void Update()
	{
		if (userInput.Player.Interaction.triggered && interactable != null)
		{
			interactable.Interact(gameObject);
		}
	}
}
