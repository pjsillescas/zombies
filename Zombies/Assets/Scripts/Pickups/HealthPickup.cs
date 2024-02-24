using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{
    public float HealthPoints = 20;
    private bool isApplied = false;

    public bool ApplyPickup(GameObject receiver)
    {
        bool result = receiver.TryGetComponent(out DamageableComponent damageableComponent);
        if (!result)
        {
            damageableComponent = receiver.GetComponentInChildren<DamageableComponent>();
            result = damageableComponent != null;
        }

        if (result && !isApplied)
        {
            isApplied = true;

            damageableComponent.AddHitPoints(HealthPoints);
        }

        return result;
    }

    public void SetHealthPoints(float healthPoints)
    {
        HealthPoints = healthPoints;
    }
}
