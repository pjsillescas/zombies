using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	public float AngularSpeed = 10;
	public Vector3 RotationAxis = new(1, 0, 0);

	// Start is called before the first frame update
	void Start()
	{
		RotationAxis = RotationAxis.normalized;
	}

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(RotationAxis, AngularSpeed * Time.deltaTime);
	}
}
