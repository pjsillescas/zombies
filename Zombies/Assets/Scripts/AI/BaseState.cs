using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState
{
	protected const float SIGHT_CONE_DEGREES = 30f;
	protected float MIN_DISTANCE = 6f;

	protected AIManager aiManager;
	protected GameObject player;

	public BaseState()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public virtual void SetAIManager(AIManager aiManager)
	{
		this.aiManager = aiManager;
	}

	protected bool IsPlayerOnSight()
	{
		float cosine = Vector3.Dot((player.transform.position - aiManager.transform.position).normalized, aiManager.transform.forward);
		float angle = Mathf.Acos(cosine) * 180f / Mathf.PI;
		return Mathf.Abs(angle) <= SIGHT_CONE_DEGREES && Vector3.Distance(player.transform.position, aiManager.transform.position) <= MIN_DISTANCE;
	}


	public abstract Color GetStateColor();
	public abstract void OnStateEnter();
	public abstract void OnStateExit();
	public abstract void OnTriggerEnter();
	public abstract void OnTriggerExit();

	protected abstract void DoTick();
	public abstract void StopWorking();

	private bool IsPlayerActive()
	{
		return true; // player.GetHitPoints() > 0;
	}

	public virtual void Tick()
	{
		if (IsPlayerActive())
		{
			DoTick();
		}
		else
		{
			StopWorking();
		}
	}
}