using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public class ShotEventArgs : EventArgs
	{
		public Weapon Shotter;
		public DamageableComponent Victim;
	}

	public static event EventHandler<ShotEventArgs> OnAnyShot;
	public enum WeaponType
	{
		Rifle,
		Shotgun,
		Sword,
		GrenadeLauncher,
		Grenade,
		Zombiefier,
	}

	[SerializeField] protected WeaponType Type;
	[SerializeField] protected string Name = "";
	[SerializeField] protected float CooldownTimeSeconds = 1f;
	[SerializeField] protected float Damage = 1f;
	[SerializeField] protected float Spread = 0f;
	[SerializeField] protected int Ammo = 20;
	[SerializeField] protected int DefaultAmmo = 20;
	[SerializeField] protected float WeaponRange = 10f;
	[SerializeField] protected AudioClip ShootClip;
	[SerializeField] protected DamageableComponent.DamageType WeaponDamageType;

	protected bool canShoot = true;
	protected ParticleSystem particles;
	protected AudioSource audioSource;
	protected ShooterController shooterController;


	protected virtual void Awake()
	{
		particles = GetComponent<ParticleSystem>();
		if (particles == null)
		{
			particles = GetComponentInChildren<ParticleSystem>();
		}

		audioSource = GetComponent<AudioSource>();
		if (audioSource != null)
		{
			audioSource.clip = ShootClip;
		}
	}

	public void ResetAmmo()
	{
		Ammo = DefaultAmmo;
	}

	public virtual void ActivateWeapon()
	{
		gameObject.SetActive(true);
		canShoot = true;
	}

	public virtual void DeactivateWeapon()
	{
		gameObject.SetActive(false);
	}

	public void SetShooterController(ShooterController shooterController)
	{
		this.shooterController = shooterController;
	}

	public virtual string GetAnimationTrigger()
	{
		return "Fire";
	}
	private Vector3 GetShotDirection()
	{
		return transform.forward;
	}

	public virtual bool Shoot()
	{
		if (canShoot && Ammo > 0)
		{
			Ammo--;

			if (particles != null)
			{
				particles.Play();
			}

			if (audioSource != null)
			{
				audioSource.Play();
			}

			canShoot = false;
			Vector3 direction = GetShotDirection();

			Vector3 origin = transform.position;
			Ray ray = new Ray(origin, direction);

			DamageableComponent receiver = null;
			if (Physics.Raycast(ray, out RaycastHit hit, WeaponRange, -5, QueryTriggerInteraction.Ignore) &&
				hit.collider.gameObject.TryGetComponent(out receiver))
			{
				receiver.ApplyDamage(Damage, hit.point, WeaponDamageType);
			}

			SendOnAnyShotEvent(receiver);

			StartCoroutine(WaitForCooldown());
			return true;
		}

		return false;
	}

	protected void SendOnAnyShotEvent(DamageableComponent receiver)
	{
		OnAnyShot?.Invoke(this, new ShotEventArgs()
		{
			Shotter = this,
			Victim = receiver,
		});

	}

	public virtual void OnAnyShotEvent(ShotEventArgs args)
	{
		OnAnyShot?.Invoke(this, args);
	}

	public IEnumerator WaitForCooldown()
	{
		yield return new WaitForSeconds(CooldownTimeSeconds);

		canShoot = true;

		yield return null;
	}

	public string GetName()
	{
		return Name;
	}

	public void SetParentTransform(Transform newParent)
	{
		transform.parent = newParent;
	}

	public int GetAmmo()
	{
		return Ammo;
	}

	public WeaponType GetWeaponType()
	{
		return Type;
	}

	public void AddAmmo(int newAmmo)
	{
		Ammo += newAmmo;
	}
}
