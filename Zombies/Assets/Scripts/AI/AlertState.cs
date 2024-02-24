using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlertState : BaseState
{
	private GameObject target;
	private NavMeshAgent agent;
	private float runSpeed = 1f;

	public override void SetAIManager(AIManager aiManager)
	{
		base.SetAIManager(aiManager);
		agent = aiManager.GetComponent<NavMeshAgent>();
	}

	private void OnTargetDetected(object sender, GameObject target)
	{
		this.target = target;
	}

	private void OnTargetLost(object sender, GameObject target)
	{
		aiManager.SetState(AIManager.State.Roam);
	}

	public void SetRunSpeed(float runSpeed)
	{
		this.runSpeed = runSpeed;
		agent.speed = runSpeed;
	}

	public override void OnStateEnter()
	{
		if (agent.enabled)
		{
			agent.isStopped = false;
			//agent.speed = 1f;
			agent.speed = runSpeed;

			Radar radar = aiManager.GetRadar();
			target = radar.GetTarget();
			radar.OnTargetDetected += OnTargetDetected;
			radar.OnTargetLost += OnTargetLost;
		}
	}

	public override void OnStateExit()
	{
		if (agent.enabled)
		{
			agent.isStopped = true;
		}

		Radar radar = aiManager.GetRadar();
		radar.OnTargetDetected -= OnTargetDetected;
		radar.OnTargetLost -= OnTargetLost;
	}

	protected override void DoTick()
	{
		if (target != null && agent != null && agent.enabled)
		{
			agent.SetDestination(target.transform.position);
		}
	}

	public override void StopWorking()
	{
		;
	}

	public override void OnTriggerEnter()
	{
		if (agent.enabled)
		{
			agent.isStopped = true;
		}

		aiManager.SetState(AIManager.State.Attack);
	}

	public override void OnTriggerExit()
	{
		;
	}

	public override Color GetStateColor()
	{
		return Color.green;
	}

}
