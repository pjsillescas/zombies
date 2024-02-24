using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
	public event EventHandler<GameObject> OnTargetDetected;
	public event EventHandler<GameObject> OnTargetLost;

	public List<string> TagsToSearch = new() { "Player" , "Pedestrian" };

	private GameObject target;
	private GameObject temptativeTarget;

	// Start is called before the first frame update
	void Start()
	{
		target = null;
		temptativeTarget = null;
	}

	private bool IsVisible(GameObject target)
	{
		/*
		Vector3 direction = (target.transform.position + new Vector3(0, 1, 0) - transform.position).normalized;
		Ray ray = new(transform.position + transform.forward * 2f, direction);
		Debug.DrawLine(target.transform.position, target.transform.position + direction * 10f,Color.red);
		if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
		{
			Debug.Log("isvisible hitting " + hit.collider.name);
			return hit.collider.gameObject.Equals(target);
		}

		return false;
		*/
		return true;
	}

	private void SetTarget(GameObject target)
	{
		this.target = target;
		temptativeTarget = null;
		OnTargetDetected?.Invoke(this, target);

	}

	private void SetTemptativeTarget(GameObject target)
	{
		temptativeTarget = target;

	}

	private bool HasTag(Collider other)
	{
		bool hasTag = false;
		foreach (var tag in TagsToSearch)
		{
			hasTag = hasTag || other.CompareTag(tag);
		}

		return hasTag;
	}

	private void TriggerCheck(Collider other)
	{
		if (HasTag(other))
		{
			if (IsVisible(other.gameObject))
			{
				Debug.Log("detected " + other.gameObject.name);
				SetTarget(other.gameObject);
			}
			else
			{
				SetTemptativeTarget(other.gameObject);
			}
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		TriggerCheck(other);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.Equals(target) || other.gameObject.Equals(temptativeTarget))
		{
			OnTargetLost?.Invoke(this, target);
			target = null;
			temptativeTarget = null;
		}
	}

	public GameObject GetTarget()
	{
		return target;
	}

	private void FixedUpdate()
	{
		if (temptativeTarget != null && IsVisible(temptativeTarget))
		{
			SetTarget(temptativeTarget);
		}
	}
}
