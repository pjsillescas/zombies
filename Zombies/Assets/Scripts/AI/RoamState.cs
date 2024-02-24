using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : BaseState
{
	private NavMeshAgent agent;
	private Animator animator;
	private Vector3 destination;
	private float threshold = 0.2f;
	private bool roamEnabled = false;
	private float walkSpeed = 0.7f;

	public override void SetAIManager(AIManager manager)
	{
		base.SetAIManager(manager);
		agent = aiManager.GetComponent<NavMeshAgent>();
		animator = aiManager.GetComponent<Animator>();

		roamEnabled = true;
	}

	private void OnTargetDetected(object sender, GameObject target)
	{
		aiManager.SetState(AIManager.State.Alert);
	}

	private void OnTargetLost(object sender, GameObject target)
	{
		;
	}

	public void SetWalkSpeed(float walkSpeed)
	{
		this.walkSpeed = walkSpeed;
		agent.speed = walkSpeed;
	}

	public override void OnStateEnter()
	{
		if (agent.enabled)
		{
			agent.isStopped = false;
		}

		Radar radar = aiManager.GetRadar();
		radar.OnTargetDetected += OnTargetDetected;
		radar.OnTargetLost += OnTargetLost;
		destination = aiManager.transform.position;
		agent.speed = walkSpeed;

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

	private Vector3 RandomNavmeshLocation(float radius)
	{
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += aiManager.transform.position;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1))
		{
			finalPosition = hit.position;
		}

		return finalPosition;
	}

	private Vector3 RandomNavmeshLocation(float radiusMin, float radiusMax)
	{
		float radius = Random.Range(radiusMin, radiusMax);

		return RandomNavmeshLocation(radius);
	}

	protected override void DoTick()
	{
		if (roamEnabled)
		{
			float distanceToDestination = Vector3.Distance(aiManager.transform.position, destination);
			animator.SetFloat("Speed", agent.speed);

			if (distanceToDestination <= threshold && agent.enabled)
			{
				destination = RandomNavmeshLocation(20, 50);
				if (agent.enabled)
				{
					agent.SetDestination(destination);
				}
			}
		}
	}

	public override void StopWorking()
	{
		if (agent.enabled)
		{
			agent.isStopped = true;
		}
	}

	public override void OnTriggerEnter()
	{
		;
		//aiManager.SetState(AIManager.State.Alert);
	}

	public override void OnTriggerExit()
	{
		;
	}

	public override Color GetStateColor()
	{
		return Color.blue;
	}
}

