using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianAIManager : MonoBehaviour
{
    private const int ROAD_AREA_ID = 3;
    private const float ROAD_AREA_COST = 500f;

    public event EventHandler<PedestrianAIManager> OnDestinationReached;
    public delegate void OnAnimationFinishDelegate();

    private TravelState travelState;
    private RunawayState runawayState;
    private IState currentState;
    private Material material;
    private Radar radar;

    public void RunOnDestinationReach()
	{
        //Debug.Log("ondestinationReach");
        OnDestinationReached?.Invoke(this,this);
	}

    public virtual void SetSpeeds(float walkSpeed, float runSpeed)
    {
        travelState.SetWalkSpeed(walkSpeed);
        runawayState.SetRunSpeed(runSpeed);
    }

    private void SetCurrentState(IState newState)
    {
        currentState?.OnStateExit();
        currentState = newState;

        currentState?.OnStateEnter();
    }


    public Radar GetRadar()
    {
        return radar;
    }

    // Start is called before the first frame update
    void Start()
    {
        radar = GetComponentInChildren<Radar>();
        material = GetComponentInChildren<Renderer>().material;

        material.color = Color.yellow;

        travelState = new();
        runawayState = new();

        travelState.SetAIManager(this);
        runawayState.SetAIManager(this);

        SetCurrentState(travelState);

        var agent = GetComponent<NavMeshAgent>();
        agent.SetAreaCost(ROAD_AREA_ID, ROAD_AREA_COST);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Tick();
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

    public enum PedestrianState
    {
        Travel,
        Runaway,
    }

    
    public void SetState(PedestrianState state)
    {
        IState nextState;
        switch (state)
        {
            case PedestrianState.Runaway:
                nextState = runawayState;
                break;
            case PedestrianState.Travel:
            default:
                nextState = travelState;
                break;
        }

        SetCurrentState(nextState);
    }

    public void DestroyPedestrian()
    {
        Debug.Log("destroy pedestrian");
        Destroy(transform.gameObject);
    }

}
