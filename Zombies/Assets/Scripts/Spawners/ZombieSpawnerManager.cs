using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerManager : MonoBehaviour
{
	[SerializeField] private float MinSpawningCooldown = 3f;
	[SerializeField] private float SpawningCooldown = 10f;
	[SerializeField] private float DeltaTimeCooldown = 0.5f;
	[SerializeField] private int MaxZombiesInGame = 100;
	[SerializeField] private GameObject player;
	[SerializeField] private Transform ParentTransform;

	private List<ZombieSpawner> zombieSpawners;

	private float time;
	int maxSpawnerIndex;

	private void Awake()
	{
		var spawners = FindObjectsOfType<ZombieSpawner>();

		zombieSpawners = new(spawners);
		maxSpawnerIndex = zombieSpawners.Count - 1;

		SpawnAll();

		//Debug.Log("found " + zombieSpawners.Count + " spawners");
	}

	private void SpawnAll()
	{
		foreach (var spawner in zombieSpawners)
		{
			var zombie = spawner.SpawnZombie();
			zombie.transform.SetParent(ParentTransform);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		time = 0;
		TimerUI.Instance.OnTimerTick += OnTimerTick;
	}

	private void OnTimerTick(object sender, EventArgs args)
	{
		if (SpawningCooldown > MinSpawningCooldown)
		{
			SpawningCooldown -= DeltaTimeCooldown;
		}
	}

	protected int GetNearestSpawnerIndex()
	{
		Vector3 position = player.transform.position;

		int index = 0;
		if (zombieSpawners.Count > 0)
		{
			Vector3 spawnerPosition = zombieSpawners[0].transform.position;
			float distanceSqrt = Vector3.SqrMagnitude(spawnerPosition - position);
			for (var k = 0; k <= maxSpawnerIndex; k++)
			{
				var spawner = zombieSpawners[k];
				spawnerPosition = spawner.transform.position;
				float distanceSqrtNew = Vector3.SqrMagnitude(spawnerPosition - position);

				if (distanceSqrtNew <= distanceSqrt)
				{
					distanceSqrt = distanceSqrtNew;
					index = k;
				}
			}
		}

		return index;
	}

	protected int GetNearSpawnerIndex()
	{
		Vector3 position = player.transform.position;

		int index = 0;
		if (zombieSpawners.Count > 0)
		{
			Vector3 spawnerPosition = zombieSpawners[0].transform.position;
			float distanceSqrt = Vector3.SqrMagnitude(spawnerPosition - position);
			for (var k = 0; k <= maxSpawnerIndex; k++)
			{
				var spawner = zombieSpawners[k];
				spawnerPosition = spawner.transform.position;
				float distanceSqrtNew = Vector3.SqrMagnitude(spawnerPosition - position);

				if (distanceSqrtNew <= distanceSqrt)
				{
					distanceSqrt = distanceSqrtNew;
					index = k;
				}
			}
		}

		float rand = UnityEngine.Random.value;

		if (rand > 0.3)
		{
			if (rand <= 0.6)
			{
				if (index == 0)
				{
					index++;
				}
				else
				{
					index--;
				}
			}
			else
			{
				if (index == maxSpawnerIndex)
				{
					index--;
				}
				else
				{
					index++;
				}
			}
		}

		return index;
	}
	protected int getFarthestSpawnerIndex()
	{
		Vector3 position = player.transform.position;

		int index = 0;
		if (zombieSpawners.Count > 0)
		{
			Vector3 spawnerPosition = zombieSpawners[0].transform.position;
			float distanceSqrt = Vector3.SqrMagnitude(spawnerPosition - position);
			//foreach (var spawner in zombieSpawners)
			for (var k = 0; k <= maxSpawnerIndex; k++)
			{
				var spawner = zombieSpawners[k];
				spawnerPosition = spawner.transform.position;
				float distanceSqrtNew = Vector3.SqrMagnitude(spawnerPosition - position);

				if (distanceSqrtNew >= distanceSqrt)
				{
					distanceSqrt = distanceSqrtNew;
					index = k;
				}
			}
		}

		return index;
	}

	protected int GetRandomSpawnerIndex()
	{
		return (maxSpawnerIndex > 0) ? UnityEngine.Random.Range(0, maxSpawnerIndex) : 0;
	}

	protected virtual ZombieSpawner GetSpawner()
	{
		//int index = Random.Range(0,maxSpawnerIndex);
		//int index = GetRandomSpawnerIndex();
		int index = GetNearestSpawnerIndex();
		//int index = GetNearSpawnerIndex();
		//Debug.Log("spawning at spawner " + index);
		return (index > 0) ? zombieSpawners[index] : null;
	}

	// Update is called once per frame
	void Update()
	{
		if (time <= 0)
		{
			if (LevelManager.Instance.GetNumZombiesInGame() < MaxZombiesInGame)
			{
				var spawner = GetSpawner();

				spawner?.SpawnZombie();

				time = SpawningCooldown;
			}
		}
		else
		{
			time -= Time.deltaTime;
		}
	}
}
