using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour, IPickup
{
	[SerializeField] private Weapon.WeaponType WeaponType = Weapon.WeaponType.Rifle;
	[SerializeField] private int AmmoNumber = 10;
	private bool isApplied = false;

	public void SetAmmoNumber(int ammoNumber)
	{
		AmmoNumber = ammoNumber;
	}

	public bool ApplyPickup(GameObject receiver)
	{
		bool result = receiver.TryGetComponent(out ShooterController controller);
		if (result && !isApplied)
		{
			isApplied = true;

			controller.AddAmmo(WeaponType, AmmoNumber);
		}

		return result;
	}
}
