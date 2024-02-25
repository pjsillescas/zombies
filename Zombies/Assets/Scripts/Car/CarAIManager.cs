using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAIManager : MonoBehaviour
{
	const float DESTINATION_THRESHOLD = 0.3f;

	public event EventHandler<CarAIManager> OnDestinationReached;

	//private List<Transform> targets;
	private NavMeshAgent agent;
	private Vector3 destination;
	private List<Transform> route;
	private int currentRouteNodeIndex;
	private int endRouteNodeIndex;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	public void SetEndRouteNodeIndex(int index)
	{
		endRouteNodeIndex = index;
		SetRandomDestination();
	}
	private bool UpdateDestination()
	{
		if (agent == null)
		{
			return false;
		}

		currentRouteNodeIndex++;

		//Debug.Log(currentRouteNodeIndex + "/" + route.Count);

		bool endOfRoute = currentRouteNodeIndex == route.Count;
		if (!endOfRoute)
		{
			destination = route[currentRouteNodeIndex].position;

			agent.SetDestination(destination);
		}

		return endOfRoute;
	}

	public void SetRandomDestination()
	{
		var routeObject = RoadMap.Instance.GetRoute(endRouteNodeIndex);
		route = routeObject.Positions;
		endRouteNodeIndex = routeObject.LastPositionIndex;
		currentRouteNodeIndex = 0;
		UpdateDestination();

		agent.SetDestination(destination);
	}

	// Update is called once per frame
	void Update()
	{
		if (Vector3.SqrMagnitude(agent.transform.position - destination) < DESTINATION_THRESHOLD)
		{
			if (UpdateDestination())
			{
				OnDestinationReached?.Invoke(this, this);

				SetRandomDestination();
			}
		}
	}

	public void DestroyCar()
	{
		Debug.Log("destroying car " + transform.parent.gameObject.name);
		Destroy(transform.parent.gameObject);
	}

	public void DisableAI()
	{
		agent.enabled = false;
		enabled = false;
	}

	public void EnableAI()
	{
		agent.enabled = true;
		enabled = true;
	}

	public bool IsAIDisabled()
	{
		return enabled;
	}

	private bool HasToWaitForCollider(Collider other, out Semaphore semaphore)
	{
		semaphore = other.GetComponent<Semaphore>();
		return agent.isActiveAndEnabled && (semaphore != null || other.CompareTag("Car"));
	}

	private bool IsSemaphoreColor(Semaphore semaphore, Semaphore.SemaphoreState state)
	{
		return semaphore == null || semaphore.GetState().Equals(state);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (HasToWaitForCollider(other, out Semaphore semaphore) && IsSemaphoreColor(semaphore, Semaphore.SemaphoreState.Red))
		{
			Debug.Log("stop");
			destination = agent.destination;
			agent.ResetPath();
			agent.isStopped = true;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Pedestrian") || collision.gameObject.CompareTag("Zombie"))
		{
			if (collision.gameObject.TryGetComponent(out DamageableComponent damageableComponent))
			{
				damageableComponent.ApplyDamage(1000, collision.transform.position, DamageableComponent.DamageType.hit);
			}
		}
	}
	private void OnTriggerStay(Collider other)
	{
		CheckSemaphore(other);
	}

	private void CheckSemaphore(Collider other)
	{
		if (HasToWaitForCollider(other, out Semaphore semaphore) && agent.isStopped && IsSemaphoreColor(semaphore, Semaphore.SemaphoreState.Green))
		{
			Debug.Log("go");
			agent.isStopped = false;
			agent.destination = destination;
		}

	}

	private void OnTriggerExit(Collider other)
	{
		CheckSemaphore(other);
	}

}
