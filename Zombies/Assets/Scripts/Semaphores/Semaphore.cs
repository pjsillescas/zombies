using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore : MonoBehaviour
{
	public enum SemaphoreState { Red, Amber, Green, }

	[SerializeField] Material SemaphoreMaterial;
	[SerializeField] Material RedMaterial;
	[SerializeField] Material RedLightMaterial;
	[SerializeField] Material AmberMaterial;
	[SerializeField] Material AmberLightMaterial;
	[SerializeField] Material GreenMaterial;
	[SerializeField] Material GreenLightMaterial;

	private const int SEMAPHORE_MATERIAL_INDEX = 0;

	private const int RED_MATERIAL_INDEX = 1;
	private const int AMBER_MATERIAL_INDEX = 2;
	private const int GREEN_MATERIAL_INDEX = 3;
	private SemaphoreState state;

	private MeshRenderer meshRenderer;
	private bool goUp = true;
	float time = 1;

	public SemaphoreState GetState()
	{
		return state;
	}

	public void SetGoUp(bool goUp)
	{
		this.goUp = goUp;
	}

	public bool GetGoUp() => goUp;

	// Start is called before the first frame update
	void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		Material[] materials = meshRenderer.materials;
		materials[SEMAPHORE_MATERIAL_INDEX] = SemaphoreMaterial;
		meshRenderer.materials = materials;

		SetState(SemaphoreState.Red);
	}

	// Update is called once per frame
	void Update()
	{
		time -= Time.deltaTime;
		if (time <= 0)
		{
			time = 1;
			SetState(GetNextState());
		}
	}

	public SemaphoreState GetNextState()
	{
		SemaphoreState nextState = state;

		switch (state)
		{
			case SemaphoreState.Red:
				if (!goUp)
				{
					nextState = SemaphoreState.Amber;
				}
				break;
			case SemaphoreState.Amber:
				nextState = (goUp) ? SemaphoreState.Red : SemaphoreState.Green;
				break;
			case SemaphoreState.Green:
			default:
				if (goUp)
				{
					nextState = SemaphoreState.Amber;
				}
				break;
		}

		return nextState;
	}

	public void SetState(SemaphoreState state)
	{
		this.state = state;
		Material[] materials = meshRenderer.materials;
		switch (state)
		{
			case SemaphoreState.Red:
				//Debug.Log("red");
				materials[RED_MATERIAL_INDEX] = RedLightMaterial;
				materials[AMBER_MATERIAL_INDEX] = AmberMaterial;
				materials[GREEN_MATERIAL_INDEX] = GreenMaterial;
				break;
			case SemaphoreState.Amber:
				//Debug.Log("amber");
				materials[RED_MATERIAL_INDEX] = RedMaterial;
				materials[AMBER_MATERIAL_INDEX] = AmberLightMaterial;
				materials[GREEN_MATERIAL_INDEX] = GreenMaterial;
				break;
			case SemaphoreState.Green:
			default:
				//Debug.Log("green");
				materials[RED_MATERIAL_INDEX] = RedMaterial;
				materials[AMBER_MATERIAL_INDEX] = AmberMaterial;
				materials[GREEN_MATERIAL_INDEX] = GreenLightMaterial;
				break;
		}

		meshRenderer.materials = materials;
	}


}
