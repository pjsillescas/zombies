using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerManager : MonoBehaviour
{
	public static CarSpawnerManager Instance = null;

	[SerializeField] private GameObject CarPrefab;
	[SerializeField] private int MaxCars = 10;
	[SerializeField] private List<Transform> CarSources;
	[SerializeField] private List<Transform> CarDestinations;
	[SerializeField] private float SpawnTime = 2f;
	[SerializeField] private Transform ParentTransform;

	private float currentTime;
	private int currentCars;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There is another car spawner manager.");
			return;
		}

		Instance = this;
		currentTime = SpawnTime;
		currentCars = 0;
	}

	public RoadMap.Route GetRoute(int sourceIndex)
	{
		return RoadMap.Instance.GetRoute(sourceIndex);
	}

	private void SpawnCar()
	{
		var position = RoadMap.Instance.GetRandomPosition();

		int sourceIndex = position.Index;
		var car = Instantiate(CarPrefab, position.Position.position, Quaternion.identity);
		car.transform.SetParent(ParentTransform);
		var carAiManager = car.GetComponentInChildren<CarAIManager>();
		carAiManager.OnDestinationReached += CarDestinationReached;
		carAiManager.SetEndRouteNodeIndex(sourceIndex);
	}

	private void CarDestinationReached(object sender, CarAIManager carAIManager)
	{
		Debug.Log("car destination reached");
		/*
        carAIManager.DestroyCar();

        currentCars--;
        */
	}

	public List<Transform> GetCarDestinations()
	{
		return CarDestinations;
	}

	// Update is called once per frame
	void Update()
	{
		currentTime -= Time.deltaTime;

		if (currentTime <= 0)
		{
			if (currentCars < MaxCars)
			{
				SpawnCar();
				currentCars++;
				Debug.Log($"there are {currentCars} cars");
			}

			currentTime = SpawnTime;
		}
	}
}
