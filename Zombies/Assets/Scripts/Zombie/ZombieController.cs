using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static DamageableComponent;

public class ZombieController : MonoBehaviour, ICharacterController
{
	public static event EventHandler<ZombieController> OnAnyCreatedZombie;
	public static event EventHandler<ZombieController> OnAnyKilledZombie;

	[SerializeField] private Canvas ZombieCanvas;
	[SerializeField] private float HealthPoints = 100f;
	[SerializeField] private float WalkSpeed = 1.2f; // 0.7f;
	[SerializeField] private float RunSpeed = 2f; // 1f;
	[SerializeField] private float Damage = 10f;
	[SerializeField] private GameObject ShatteredCharacterPrefab;

	private NavMeshAgent agent;
	private Animator animator;
	private PunchBag punchBag;
	private AIManager aIManager;
	private DamageableComponent damageableComponent;

	// Start is called before the first frame update
	void Start()
	{
		damageableComponent = GetComponent<DamageableComponent>();
		damageableComponent.OnHitPointsDepleted += OnHitPointsDepleted;
		damageableComponent.OnDamage += OnDamage;
		punchBag = GetComponentInChildren<PunchBag>();

		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		aIManager = GetComponent<AIManager>();

		ConfigureZombie();
		OnAnyCreatedZombie?.Invoke(this, this);
	}

	private void ConfigureZombie()
	{
		aIManager.SetSpeeds(WalkSpeed, RunSpeed);
		punchBag.SetDamage(Damage);
		damageableComponent.SetHitPoints(HealthPoints);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void ResetZombie()
	{
		GetComponent<DamageableComponent>().ResetComponent();
		EnablePhysics();
		EnableControl();
		animator.enabled = true;
		animator.ResetTrigger("Attack");
		animator.ResetTrigger("Hit");
		animator.ResetTrigger("Death");
		animator.Play("Idle/Walk", -1, 0f);
		GetComponent<AIManager>().SetState(AIManager.State.Roam);
	}

	private void LateUpdate()
	{
		if (ZombieCanvas != null)
		{
			ZombieCanvas.transform.LookAt(transform.position + Camera.main.transform.forward);
		}
	}

	public void DisableControl()
	{
		if (agent.enabled)
		{
			agent.isStopped = true;
			agent.enabled = false;
		}
	}

	public void EnableControl()
	{
		if (!agent.enabled)
		{
			agent.enabled = true;
			agent.isStopped = false;
		}
	}

	private void DisablePhysics()
	{
		foreach (var collider in GetComponents<BoxCollider>())
		{
			collider.enabled = false;
		}
		GetComponent<Rigidbody>().detectCollisions = false;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		if (agent.enabled)
		{
			agent.velocity = Vector3.zero;
			agent.speed = 0;
			agent.isStopped = true;
		}
	}

	private void EnablePhysics()
	{
		foreach (var collider in GetComponents<BoxCollider>())
		{
			collider.enabled = true;
		}
		GetComponent<Rigidbody>().detectCollisions = true;
		GetComponent<Rigidbody>().isKinematic = true;
		agent.enabled = true;
		agent.isStopped = false;
	}

	private void OnHitPointsDepleted(object sender, DamageType damageType)
	{
		DisablePhysics();

		DisableControl();
		OnAnyKilledZombie?.Invoke(this, this);

		if (damageType == DamageType.hit && ShatteredCharacterPrefab != null)
		{
			Instantiate(ShatteredCharacterPrefab, transform.position, transform.rotation);
			Destroy(gameObject);
		}
		else
		{
			animator.SetTrigger("Death");
			StartCoroutine(DieAnyway());
		}
	}

	private IEnumerator DieAnyway()
	{
		yield return new WaitForSeconds(5f);

		Destroy(gameObject);
	}

	public void OnDeathFinished()
	{
		animator.enabled = false;

		Destroy(gameObject);
	}

	public void OnHitFinished()
	{
		EnableControl();
	}

	private void OnDamage(object sender, DamageableComponent.DamageEventArgs args)
	{
		if (args.HitDamage > 0)
		{
			DisableControl();
			animator.SetTrigger("Hit");
		}
	}

	public void ActivatePunchBag()
	{
		punchBag.ActivatePunchBag();
	}

	public void DeactivatePunchBag()
	{
		punchBag.DeactivatePunchBag();
	}
}
