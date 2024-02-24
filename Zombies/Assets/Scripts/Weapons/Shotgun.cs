using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    List<DamageableComponent> targets;

	protected override void Awake()
	{
        base.Awake();

        targets = new();
    }

    public override bool Shoot()
    {
        if (canShoot && Ammo > 0)
        {
            canShoot = false;
            Ammo--;

            if (particles != null)
            {
                particles.Play();
            }

            if (audioSource != null)
            {
                audioSource.Play();
            }

            foreach (var receiver in targets)
            {
                if (receiver != null)
                {
                    receiver.ApplyDamage(Damage, receiver.transform.position, DamageableComponent.DamageType.shotgun);
                    SendOnAnyShotEvent(receiver);
                }
            }

            StartCoroutine(WaitForCooldownShotgun());
            return true;
        }

        return false;
    }

    public IEnumerator WaitForCooldownShotgun()
    {
        yield return new WaitForSeconds(CooldownTimeSeconds);

        canShoot = true;

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out DamageableComponent damageableComponent))
		{
            if (!targets.Contains(damageableComponent))
            {
                targets.Add(damageableComponent);
            }
		}
	}

	private void OnTriggerExit(Collider other)
	{
        if (other.TryGetComponent(out DamageableComponent damageableComponent))
        {
            targets.Remove(damageableComponent);
        }
    }
}
