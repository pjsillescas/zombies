using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
	void Tick();
	void StopWorking();

	void OnStateEnter();
	void OnStateExit();

	void OnTriggerEnter();
	void OnTriggerExit();

	Color GetStateColor();
}
