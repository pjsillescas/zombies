using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
	bool canSlash;
	DamageableComponent target;

	public override string GetAnimationTrigger()
	{
		return "Slash";
	}

	protected override void Awake()
	{
		base.Awake();

		canSlash = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(canSlash && other.TryGetComponent(out DamageableComponent damageableComponent))
		{
			damageableComponent.ApplyDamage(Damage, other.transform.position, DamageableComponent.DamageType.sword);
		}
	}

	public override bool Shoot()
	{
		if (canShoot)
		{
			shooterController.ShowSecondaryWeapon();
			return true;
		}

		return false;
	}

	public void ActivateSwordSlash()
	{
		canSlash = true;
	}

    public void DeactivateSwordSlash()
	{
		canSlash = false;
	}

    public void FinishSwordSlash()
	{
		shooterController.RestorePrimaryWeapon();
	}
}
