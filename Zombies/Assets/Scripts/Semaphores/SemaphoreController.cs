using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemaphoreController : MonoBehaviour
{
    [SerializeField] private List<Semaphore> Semaphores;
    [SerializeField] private bool InitialGoUp = false;
    [SerializeField] private float SwitchTime = 10f;

    private float time;
    private bool goUp;

    // Start is called before the first frame update
    void Start()
    {
        time = SwitchTime;
        goUp = InitialGoUp;
        //goUp = semaphore.GetGoUp();
        UpdateSemaphoresState();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if(time <= 0)
		{
            time = SwitchTime;
            goUp = !goUp;
		}

        UpdateSemaphoresState();
    }

    private void UpdateSemaphoresState()
	{
        foreach (var semaphore in Semaphores)
        {
            semaphore.SetGoUp(goUp);
        }
    }
}
