using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	public delegate void OnAnimationFinishDelegate();

	public enum State
	{
		Roam,
		Alert,
		Attack,
	}

	private RoamState roamState;
	private AlertState alertState;
	private AttackState attackState;
	private IState currentState;
	private Radar radar;
	private Material material;

	private void Awake()
	{
		radar = GetComponentInChildren<Radar>();
		material = GetComponentInChildren<Renderer>().material;

		material.color = Color.blue;

		roamState = new();
		alertState = new();
		attackState = new();

		roamState.SetAIManager(this);
		alertState.SetAIManager(this);
		attackState.SetAIManager(this);

		SetCurrentState(roamState);
	}

	public void SetSpeeds(float walkSpeed, float runSpeed)
	{
		roamState.SetWalkSpeed(walkSpeed);
		alertState.SetRunSpeed(runSpeed);
	}

	private void SetCurrentState(IState newState)
	{
		currentState?.OnStateExit();
		currentState = newState;

		currentState?.OnStateEnter();
		//material.color = currentState.GetStateColor();

	}

	public Radar GetRadar()
	{
		return radar;
	}

	public void SetState(State state)
	{
		IState nextState = state switch
		{
			State.Alert => alertState,
			State.Attack => attackState,
			_ => roamState,
		};
		SetCurrentState(nextState);
	}

	// Start is called before the first frame update
	void Start()
	{
		;
	}

	// Update is called once per frame
	void Update()
	{
		if (isActiveAndEnabled)
		{
			currentState?.Tick();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log("ontriggerenter " + currentState.ToString());
		currentState?.OnTriggerEnter();
	}

	private void OnTriggerExit(Collider other)
	{
		currentState?.OnTriggerExit();
	}

	public OnAnimationFinishDelegate onAnimationFinish;

	public void WaitForAnimation(OnAnimationFinishDelegate onAnimationFinish)
	{
		this.onAnimationFinish = onAnimationFinish;

		StartCoroutine(WaitForAnimationCoroutine());
	}

	public void OnAnimationFinish()
	{
		this.onAnimationFinish?.Invoke();
	}

	private IEnumerator WaitForAnimationCoroutine()
	{
		yield return new WaitForSeconds(1f);

		onAnimationFinish?.Invoke();

		yield return null;
	}
}
