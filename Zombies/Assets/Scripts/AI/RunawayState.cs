using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunawayState : BaseState
{
	private NavMeshAgent agent;
	private float runSpeed = 2f; // 1f;
	private new PedestrianAIManager aiManager;
	private readonly List<GameObject> targetsOnSight;

	public RunawayState(): base()
	{
		targetsOnSight = new();
	}

	public void SetAIManager(PedestrianAIManager aiManager)
	{
		this.aiManager = aiManager;
		agent = aiManager.GetComponent<NavMeshAgent>();
	}

	private void OnTargetDetected(object sender, GameObject target)
	{
		if (!targetsOnSight.Contains(target))
		{
			targetsOnSight.Add(target);
		}
	}

	private void OnTargetLost(object sender, GameObject target)
	{
		if(targetsOnSight.Contains(target))
		{
			targetsOnSight.Remove(target);
		}
		
		if (targetsOnSight.Count == 0)
		{
			aiManager.SetState(PedestrianAIManager.PedestrianState.Travel);
		}
	}

	public void SetRunSpeed(float runSpeed)
	{
		this.runSpeed = runSpeed;
		agent.speed = runSpeed;
	}

	public override void OnStateEnter()
	{
		Debug.Log("enter runaway");
		if (agent.enabled)
		{
			agent.isStopped = false;
			//agent.speed = 1f;
			agent.speed = runSpeed;

			Radar radar = aiManager.GetRadar();
			targetsOnSight.Add(radar.GetTarget());
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
		if(targetsOnSight.Count > 0)
		{
			Vector3 direction = Vector3.zero;
			float distance = 20f;
			targetsOnSight.ForEach(go => direction += go.transform.position - aiManager.transform.position);
			var newDestination = aiManager.transform.position - distance * direction.normalized;
			agent.SetDestination(newDestination);
		}
	}

	public override void StopWorking()
	{
		;
	}

	public override void OnTriggerEnter()
	{
		;
	}

	public override void OnTriggerExit()
	{
		;
	}

	public override Color GetStateColor()
	{
		return Color.yellow;
	}
}
