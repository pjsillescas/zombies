using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IPickup
{
    [SerializeField] private GameObject WeaponPrefab;
    private bool isApplied = false;
    public bool ApplyPickup(GameObject receiver)
    {
        bool result = receiver.TryGetComponent(out ShooterController controller);
        if (result && !isApplied)
        {
            isApplied = true;
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }

            controller.AddPrimaryWeapon(WeaponPrefab);
        }

        return result;
    }
}
