using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private float SpawningCooldown = 30f;

    private PickupGeneratorComponent pickupGeneratorComponent;
    private float time;
	private void Awake()
	{
        pickupGeneratorComponent = GetComponent<PickupGeneratorComponent>();
	}

	// Start is called before the first frame update
	void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(time <= 0)
		{
            pickupGeneratorComponent.GenerateRandomPickup();
            time = SpawningCooldown;
		}
        else
		{
            time -= Time.deltaTime;
		}
    }
}
