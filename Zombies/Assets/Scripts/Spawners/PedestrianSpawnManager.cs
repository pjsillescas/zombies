using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawnManager : MonoBehaviour
{
    public static PedestrianSpawnManager Instance = null;

    [SerializeField] private GameObject PedestrianPrefab;
    [SerializeField] private int MaxPedestrians = 10;
    [SerializeField] private List<Transform> PedestrianSources;
    [SerializeField] private List<Transform> PedestrianDestinations;
    [SerializeField] private float SpawnTime = 2f;
    [SerializeField] private Transform ParentTransform;


    private float currentTime;
    private int currentPedestrians;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is another pedestrian spawn manager.");
            return;
        }

        Instance = this;
        currentTime = SpawnTime;
        currentPedestrians = 0;
    }

    private void SpawnPedestrian()
    {
        int sourceIndex = UnityEngine.Random.Range(0, PedestrianSources.Count - 1);
        var pedestrian = Instantiate(PedestrianPrefab, PedestrianSources[sourceIndex].position, Quaternion.identity);
        var pedestrianAiManager = pedestrian.GetComponentInChildren<PedestrianAIManager>();
        pedestrianAiManager.OnDestinationReached += PedestrianDestinationReached;

        pedestrian.GetComponent<DamageableComponent>().OnHitPointsDepleted += PedestrianDead;
        pedestrian.transform.SetParent(ParentTransform);
        Debug.Log("spawn pedestrian " + pedestrian.name + " " + pedestrian.transform.position + " in " + PedestrianSources[sourceIndex].name);
    }

    private void PedestrianDestinationReached(object sender, PedestrianAIManager pedestrianAiManager)
    {
        /*
        pedestrianAiManager.DestroyPedestrian();

        currentPedestrians--;
        */
    }

    private void PedestrianDead(object sender, DamageableComponent.DamageType damageType)
	{
        currentPedestrians--;
        Debug.Log("pedestrian dead");
	}

    public List<Transform> GetPedestrianDestinations()
    {
        return PedestrianDestinations;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            if (currentPedestrians < MaxPedestrians)
            {
                SpawnPedestrian();
                currentPedestrians++;
                Debug.Log($"there are {currentPedestrians} pedestrians");
            }

            currentTime = SpawnTime;
        }
    }
}
