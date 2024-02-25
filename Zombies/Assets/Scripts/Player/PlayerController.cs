using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour, ICharacterController
{
	[SerializeField] private float AfterHitInvulnerabilityTime = 0.5f;
	[SerializeField] private CinemachineVirtualCamera PlayerCamera;

	private Animator animator;
	private DamageableComponent damageableComponent;

	// Start is called before the first frame update
	void Start()
	{
		damageableComponent = GetComponent<DamageableComponent>();
		damageableComponent.OnHitPointsDepleted += OnHitPointsDepleted;
		damageableComponent.OnDamage += OnDamage;
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void DisableControl()
	{
		GetComponent<CharacterController>().enabled = false;
		GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
		GetComponent<StarterAssets.StarterAssetsInputs>().enabled = false;
	}

	public void EnableControl()
	{
		GetComponent<CharacterController>().enabled = true;
		GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
		GetComponent<StarterAssets.StarterAssetsInputs>().enabled = true;
	}

	private void OnHitPointsDepleted(object sender, DamageableComponent.DamageType damageType)
	{
		DisableControl();
		animator.SetTrigger("Death");
	}

	public void OnDeathFinished()
	{
		animator.enabled = false;
		LevelManager.Instance.FinishGame();
	}

	private void OnDamage(object sender, DamageableComponent.DamageEventArgs args)
	{
		if (args.HitDamage > 0)
		{
			DisableControl();
			animator.SetTrigger("Hit");
		}
	}

	public void OnHitFinished()
	{
		EnableControl();
		StartCoroutine(SetInvulnerability());
	}

	public IEnumerator SetInvulnerability()
	{
		damageableComponent.DisableDamage();

		yield return new WaitForSeconds(AfterHitInvulnerabilityTime);

		damageableComponent.EnableDamage();

		yield return null;
	}

	public void EnablePlayerCamera()
	{
		PlayerCamera.enabled = true;
	}

	public void DisablePlayerCamera()
	{
		PlayerCamera.enabled = false;
	}

}
