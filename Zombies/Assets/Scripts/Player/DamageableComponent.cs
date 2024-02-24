using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableComponent : MonoBehaviour
{
    public enum DamageType { sword, rifle, shotgun, deadZone, zombie, hit, none }

    public class DamageEventArgs : EventArgs
    {
        public float MaxHitPoints;

        public float HitPoints;

        public float HitDamage;

        public Vector3 HitPosition;

        public DamageType Type;
    }

    public event EventHandler<DamageEventArgs> OnDamage;
    public event EventHandler<DamageType> OnHitPointsDepleted;

    [SerializeField] private float MaxHitPoints = 100f;
    [SerializeField] private GameObject GoreParticleSystemPrefab;
    [SerializeField] private GameObject HitParticleSystemPrefab;

    private float hitPoints;
    private bool canHaveDamage;

    private void Start()
    {
        ResetComponent();
    }

    public void ResetComponent()
    {
        canHaveDamage = true;
        hitPoints = MaxHitPoints;

        OnDamage?.Invoke(this, new DamageEventArgs()
        {
            HitPoints = hitPoints,

            HitDamage = 0,

            MaxHitPoints = MaxHitPoints,
        });
    }

    private void SpawnHitParticles(Vector3 position)
	{
        bool isGoreMode = GameConfiguration.GetGoreMode();
        var particleSystemPrefab = (isGoreMode) ? GoreParticleSystemPrefab : HitParticleSystemPrefab;

        if(particleSystemPrefab != null)
		{
            Instantiate(particleSystemPrefab, position, new Quaternion(0, 0, 0, 0));
		}
	}


    public void ApplyDamage(float damage, Vector3 position, DamageType damageType)
    {
        if (hitPoints <= 0 || !canHaveDamage)
        {
            return;
        }

        if (damage > 0)
        {
            hitPoints = Mathf.Clamp(hitPoints - damage, 0, MaxHitPoints);

            SpawnHitParticles(position);

            OnDamage?.Invoke(this, new DamageEventArgs()
            {
                HitPoints = hitPoints,
                HitDamage = damage,
                MaxHitPoints = MaxHitPoints,
                HitPosition = position,
                Type = damageType
            });

            if (hitPoints <= 0)
            {
                OnHitPointsDepleted?.Invoke(this, damageType);
            }
        }
    }

    public void AddHitPoints(float newHitPoints)
    {
        hitPoints = Mathf.Clamp(hitPoints + newHitPoints, 0, MaxHitPoints);

        OnDamage?.Invoke(this, new DamageEventArgs()
        {
            HitPoints = hitPoints,

            HitDamage = 0,

            MaxHitPoints = MaxHitPoints,
        });
    }

    public void SetHitPoints(float hitPoints)
	{
        MaxHitPoints = hitPoints;

        AddHitPoints(hitPoints);
	}

    public float GetHitPoints()
    {
        return hitPoints;
    }

    public void EnableDamage()
	{
        canHaveDamage = true;
	}

    public void DisableDamage()
	{
        canHaveDamage = false;
	}

}
