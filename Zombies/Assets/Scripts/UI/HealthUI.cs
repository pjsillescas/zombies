using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public delegate void OnDeathDelegate();

    [SerializeField] DamageableComponent Owner;
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] Slider HealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (Owner != null)
        {
            Owner.OnDamage += OnDamage;
        }
    }

    public void OnDamage(object sender, DamageableComponent.DamageEventArgs args)
    {
        if (HealthText != null)
        {
            HealthText.text = Mathf.RoundToInt(args.HitPoints).ToString();
        }

        if (HealthSlider != null)
        {
            //Debug.Log("damage " + args.HitPoints + "/" + args.MaxHitPoints);
            HealthSlider.value = args.HitPoints / args.MaxHitPoints;
        }
    }
}
