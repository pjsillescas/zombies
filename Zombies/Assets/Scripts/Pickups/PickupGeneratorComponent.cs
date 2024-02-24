using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGeneratorComponent : MonoBehaviour
{
    const float PICKUP_HEIGHT = 2f;
    
    [SerializeField] private List<PickupGenerator> PickupGenerators;
    [SerializeField] private bool DeleteOnGeneration;

    private DamageableComponent damageableComponent;

    public void GenerateRandomPickup()
    {
        if (PickupGenerators.Count > 0)
        {
            int k = UnityEngine.Random.Range(0, PickupGenerators.Count - 1);
            PickupGenerators[k].Generate(transform.position + new Vector3(0, PICKUP_HEIGHT, 0), transform.localRotation);
        }
    }

	private void Awake()
	{
        damageableComponent = GetComponent<DamageableComponent>();
        if (damageableComponent != null)
        {
            damageableComponent.OnHitPointsDepleted += GenerateRandomPickupMethod;
        }
	}

    private void GenerateRandomPickupMethod(object sender, DamageableComponent.DamageType damageType)
	{
        GenerateRandomPickup();

        if(DeleteOnGeneration)
		{
            Destroy(gameObject);
		}
    }
}
