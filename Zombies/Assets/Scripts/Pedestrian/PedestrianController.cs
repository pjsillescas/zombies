using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianController : MonoBehaviour
{
    [SerializeField] private GameObject ZombiePrefab;
    [SerializeField] private DamageableComponent.DamageType zombieDamageType = DamageableComponent.DamageType.zombie;
    [SerializeField] private Transform ParentTransform;
    [SerializeField] private GameObject ShatteredCharacterPrefab;

    private DamageableComponent damageableComponent;
    private NavMeshAgent agent;
    private Animator animator;
    private DamageableComponent.DamageType lastDamageType = DamageableComponent.DamageType.none;

    // Start is called before the first frame update
    void Start()
    {
        damageableComponent = GetComponent<DamageableComponent>();
        damageableComponent.OnHitPointsDepleted += OnHitPointsDepleted;
        damageableComponent.OnDamage += OnDamage;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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

    public void DisableControl()
    {
        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
    }

    public void OnDeathFinished()
    {
        animator.enabled = false;

        if (lastDamageType == zombieDamageType)
        {
            var zombie = Instantiate(ZombiePrefab, transform.position, transform.rotation);

            zombie.transform.SetParent(ParentTransform);
        }

        Destroy(gameObject);
    }

    private void OnHitPointsDepleted(object sender, DamageableComponent.DamageType damageType)
    {
        DisablePhysics();

        DisableControl();
        
        if (damageType == DamageableComponent.DamageType.hit)
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

    private void OnDamage(object sender, DamageableComponent.DamageEventArgs args)
    {
        if (args.HitDamage > 0)
        {
            lastDamageType = args.Type;
            DisableControl();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
