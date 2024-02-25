using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TravelState : BaseState
{
	const int MAX_NUMTRAVELS = 2;

	List<Transform> destinations;
	private NavMeshAgent agent;
	private Animator animator;
	private Vector3 destination;
	private float threshold = 1f;
	private bool travelEnabled = false;
	private float walkSpeed = 1.2f;
	private new PedestrianAIManager aiManager;
	private int numTravels;

	public void SetAIManager(PedestrianAIManager aiManager)
	{
		this.aiManager = aiManager;
		destinations = PedestrianSpawnManager.Instance.GetPedestrianDestinations();
		agent = aiManager.GetComponent<NavMeshAgent>();
		animator = aiManager.GetComponent<Animator>();

		numTravels = 0;
		travelEnabled = true;
	}

	private void OnTargetDetected(object sender, GameObject target)
	{
		aiManager.SetState(PedestrianAIManager.PedestrianState.Runaway);
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
		//Debug.Log(aiManager.name + " enter travel");
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

	const float DESTINATION_THRESHOLD_SQ = 5;

	private Transform GetRandomDestination()
	{
		var destObject = destinations[Random.Range(0, destinations.Count - 1)];

		while (Vector3.SqrMagnitude(aiManager.transform.position - destObject.position) < DESTINATION_THRESHOLD_SQ)
		{
			destObject = destinations[Random.Range(0, destinations.Count - 1)];
		}

		return destObject;
	}

	private void SetRandomDestination()
	{
		/*
		var destObject = destinations[Random.Range(0, destinations.Count - 1)];
		destination = destObject.position;
		*/

		var destObject = GetRandomDestination();
		destination = destObject.position;
		Debug.Log(aiManager.name + " new destination " + destObject.name);
		agent.SetDestination(destination);
	}

	protected override void DoTick()
	{
		if (travelEnabled)
		{
			float distanceToDestination = Vector3.Distance(aiManager.transform.position, destination);
			animator.SetFloat("Speed", agent.speed);

			if (agent.enabled)
			{

				if (distanceToDestination <= threshold)
				{
					numTravels++;

					if (numTravels >= MAX_NUMTRAVELS)
					{
						aiManager.RunOnDestinationReach();
					}
					else
					{
						SetRandomDestination();
					}
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
