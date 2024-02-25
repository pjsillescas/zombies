using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredCharacter : MonoBehaviour
{
	[SerializeField] private List<Rigidbody> BodyParts;
	[SerializeField] private Color BodyColor = Color.blue;
	[SerializeField] private float ExplosionForce = 800f;
	[SerializeField] private float ExplosionRadius = 10f;

	// Start is called before the first frame update
	void Start()
	{
		BodyParts.ForEach(ExplodePart);

		Destroy(gameObject, 3f);
	}

	private void ExplodePart(Rigidbody part)
	{
		part.gameObject.GetComponent<MeshRenderer>().material.color = BodyColor;
		part.AddExplosionForce(ExplosionForce, part.transform.position, ExplosionRadius);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
