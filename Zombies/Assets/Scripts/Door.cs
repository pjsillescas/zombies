using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string Key = "";
	[SerializeField] private Transform ClosedPosition;
	[SerializeField] private Transform OpenPosition;
	[SerializeField] private Transform DoorObject;
	[SerializeField] private float TimeToMove = 2f;
	[SerializeField] private float TimeOpen = 0f;

	private Transform initialPosition;
	private Transform finalPosition;
	private bool activeDoor;
	private bool isOpen;
	private bool useProximitySensor;

	private void Awake()
	{
		useProximitySensor = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(useProximitySensor && other.gameObject.TryGetComponent(out KeyChain keychain))
		{
            if(Key == "" || keychain.HasKey(Key))
			{
				if (!isOpen)
				{
					Open();
				}
			}
		}
	}

	public void SetUseProximitySensor(bool useProximitySensor)
	{
		this.useProximitySensor = useProximitySensor;
	}

	public void Open()
	{
		ActivateDoor(ClosedPosition, OpenPosition);
	}

	public void Close()
	{
		ActivateDoor(OpenPosition,ClosedPosition);
	}

	private void ActivateDoor(Transform initialPosition,Transform finalPosition)
	{
		if (!activeDoor)
		{
			this.initialPosition = initialPosition;
			this.finalPosition = finalPosition;

			time = 0;
			DoorObject.transform.position = initialPosition.position;
			activeDoor = true;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		activeDoor = false;
    }

	const float DISTANCE_THRESHOLD = 0.001f;
	float time;
    // Update is called once per frame
    void Update()
    {
        if(activeDoor)
		{
			time += Time.deltaTime;
			DoorObject.transform.position = Vector3.Lerp(initialPosition.position, finalPosition.position, time / TimeToMove);

			if(Vector3.Distance(DoorObject.transform.position,finalPosition.position) < DISTANCE_THRESHOLD)
			{
				isOpen = !isOpen;
				activeDoor = false;
				if(TimeOpen > 0)
				{
					StartCoroutine(ForceClose());
				}
			}
		}
    }

	private IEnumerator ForceClose()
	{
		if (isOpen)
		{
			yield return new WaitForSeconds(TimeOpen);

			Close();
		}

		yield return null;
	}
}
