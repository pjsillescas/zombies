using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
	public float Damage = 2000f;

	private void OnTriggerEnter(Collider other)
	{
		DamageableComponent damageableComponent = other.GetComponent<DamageableComponent>();

		if (damageableComponent == null)
		{
			damageableComponent = other.GetComponentInChildren<DamageableComponent>();
		}

		if (damageableComponent != null)
		{
			damageableComponent.ApplyDamage(Damage, Vector3.zero, DamageableComponent.DamageType.deadZone);
		}
	}
}
