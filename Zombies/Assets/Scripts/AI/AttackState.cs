using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
	private bool isAttacking = false;

	private void OnTargetDetected(object sender, GameObject target)
	{
		;
	}

	private void OnTargetLost(object sender, GameObject target)
	{
		;
	}

	public override void OnStateEnter()
	{
		Radar radar = aiManager.GetRadar();
		radar.OnTargetDetected += OnTargetDetected;
		radar.OnTargetLost += OnTargetLost;
	}

	public override void OnStateExit()
	{
		Radar radar = aiManager.GetRadar();
		radar.OnTargetDetected -= OnTargetDetected;
		radar.OnTargetLost -= OnTargetLost;
	}

	protected override void DoTick()
	{
		if (!isAttacking)
		{
			isAttacking = true;
			/*
			if (Random.value <= 0.5f)
			{
				aiManager.GetComponent<Animator>().SetTrigger("Attack");
			}
			else
			{
				aiManager.GetComponent<Animator>().SetTrigger("NeckBite");
			}
			*/
			aiManager.GetComponent<Animator>().SetTrigger("Attack");
			aiManager.WaitForAnimation(OnAttackAnimationFinish);
		}
	}

	public void OnAttackAnimationFinish()
	{
		isAttacking = false;
	}

	public override void StopWorking()
	{
		;
	}

	public override void OnTriggerEnter()
	{
		//Debug.Log("attack");
	}

	public override void OnTriggerExit()
	{
		//Debug.Log("to alert");
		aiManager.SetState(AIManager.State.Alert);
	}

	public override Color GetStateColor()
	{
		return Color.red;
	}
}
