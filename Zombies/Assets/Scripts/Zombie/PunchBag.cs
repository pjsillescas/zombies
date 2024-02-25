using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBag : MonoBehaviour
{
	[SerializeField] private float Damage = 5f;

	private bool isPunching;

	public void SetDamage(float damage)
	{
		Damage = damage;
	}

	private void Awake()
	{
		DeactivatePunchBag();
	}

	public void DeactivatePunchBag()
	{
		isPunching = false;
	}

	public void ActivatePunchBag()
	{
		isPunching = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isPunching && other.TryGetComponent<DamageableComponent>(out var damageableComponent))
		{
			damageableComponent.ApplyDamage(Damage, Vector3.zero, DamageableComponent.DamageType.zombie);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		;
	}
}
