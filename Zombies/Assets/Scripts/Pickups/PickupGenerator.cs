using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PickupGenerator
{
	public enum PickupType { Key, Ammo, Health }

	public GameObject Pickup;
	public PickupType Type;
	public string KeyName;
	public int InitialValue;
	public int FinalValue;

	public GameObject Generate(Vector3 position, Quaternion rotation)
	{
		GameObject pickup = GameObject.Instantiate(Pickup, position, rotation);
		if (Type == PickupType.Key)
		{
			pickup.GetComponent<KeyPickup>().SetKeyName(KeyName);
		}
		else if (Type == PickupType.Ammo)
		{
			pickup.GetComponent<AmmoPickup>().SetAmmoNumber(UnityEngine.Random.Range(InitialValue, FinalValue + 1));
		}
		else if (Type == PickupType.Health)
		{
			pickup.GetComponent<HealthPickup>().SetHealthPoints(UnityEngine.Random.Range(InitialValue, FinalValue + 1));
		}

		return pickup;
	}
}