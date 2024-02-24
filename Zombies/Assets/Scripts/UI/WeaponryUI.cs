using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI AmmoText;

	private TranslationService.Language language;

	private void Awake()
	{
        ShooterController.OnAnyWeaponSelected += OnWeaponSelected;
        ShooterController.OnAnyWeaponShot += OnWeaponSelected;
    }

	private void Start()
	{
		language = GameConfiguration.GetLanguage();
	}

	private void OnWeaponSelected(object sender, Weapon weapon)
	{
        NameText.text = TranslationService.Instance.Translate(weapon.GetName().Trim().ToLower(),language);
        AmmoText.text = string.Format("{0,10:D3}",weapon.GetAmmo());
	}
}
