using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ZombiePrefab;
    [SerializeField] private float SpawningCooldown = 10f;
    [SerializeField] private bool IsCooldownEnabled = false;
    [SerializeField] private Transform ParentTransform;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    public GameObject SpawnZombie()
	{
        var zombie = Instantiate(ZombiePrefab, transform.position, transform.rotation);
        zombie.transform.SetParent(ParentTransform);
        return zombie;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCooldownEnabled)
        {
            if (time <= 0)
            {
                SpawnZombie();
                time = SpawningCooldown;
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }
}
