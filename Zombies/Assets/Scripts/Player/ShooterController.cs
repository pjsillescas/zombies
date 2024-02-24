using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ShooterController : MonoBehaviour
{
    public static event EventHandler<Weapon> OnAnyWeaponSelected;
    public static event EventHandler<Weapon> OnAnyWeaponShot;
    public GameObject[] PrimaryWeaponPrefabs;
    public GameObject SecondaryWeaponPrefab;
    public Transform WeaponSocket;
    public Transform SwordSocket;

    private UserInput userInput;
    private Weapon currentPrimaryWeapon;
    private int indexWeapon;
    private int numWeapons;
    private List<GameObject> objWeapons;
    private Weapon secondaryWeapon;
    private GameObject objSecondaryWeapon;
    private Coroutine shootCoroutine;
    private Animator animator;

    private GameObject InstantiateWeapon(GameObject prefab)
    {
        GameObject weaponObj = Instantiate(prefab, WeaponSocket.position, WeaponSocket.rotation);
        var weapon = weaponObj.GetComponent<Weapon>();
        weapon.SetShooterController(this);

        weaponObj.transform.parent = (weapon.GetWeaponType() == Weapon.WeaponType.Sword) ? SwordSocket : WeaponSocket;
        weaponObj.SetActive(false);

        return weaponObj;
    }

    private void Awake()
    {
        userInput = new UserInput();
        userInput.Player.Enable();

        indexWeapon = 0;
        numWeapons = PrimaryWeaponPrefabs.Length;

        if (numWeapons > 0)
        {
            objWeapons = new();
            foreach (var prefab in PrimaryWeaponPrefabs)
            {
                GameObject weaponObj = InstantiateWeapon(prefab);
                objWeapons.Add(weaponObj);
            }

            if (objWeapons.Count > 0)
            {
                EquipCurrentWeapon();
            }
            else
			{
                Debug.Log("No primary weapons created");
			}
        }

        if (SecondaryWeaponPrefab != null)
        {
            objSecondaryWeapon = InstantiateWeapon(SecondaryWeaponPrefab);
            secondaryWeapon = objSecondaryWeapon.GetComponent<Weapon>();
        }
        else
        {
            objSecondaryWeapon = null;
        }

        animator = GetComponent<Animator>();

        ActivateInput();
    }

    public void ActivateInput()
    {
        userInput.Player.ShootPrimary.started += PrimaryShoot_Performed;
        userInput.Player.ShootPrimary.canceled += PrimaryShoot_Canceled;
    }

    public void DeactivateInput()
    {
        userInput.Player.ShootPrimary.started -= PrimaryShoot_Performed;
        userInput.Player.ShootPrimary.canceled -= PrimaryShoot_Canceled;
    }

    private void OnDestroy()
    {
        DeactivateInput();
    }

    public void AddPrimaryWeapon(GameObject weaponPrefab)
    {
        objWeapons.Add(InstantiateWeapon(weaponPrefab));
        numWeapons++;

        indexWeapon = numWeapons - 1;
        EquipCurrentWeapon();
    }

    private void PrimaryShoot_Performed(CallbackContext obj)
    {
        if (gameObject.activeSelf)
        {
            shootCoroutine = StartCoroutine(ContinuouslyShoot());
        }
    }

    private void PrimaryShoot_Canceled(CallbackContext obj)
    {
        StopCoroutine(shootCoroutine);
    }

    private IEnumerator ContinuouslyShoot()
    {
        while (true)
        {
            if (currentPrimaryWeapon != null && currentPrimaryWeapon.Shoot())
            {
                animator.SetTrigger(currentPrimaryWeapon.GetAnimationTrigger());
                OnAnyWeaponShot?.Invoke(this, currentPrimaryWeapon);
            }
            
            yield return null;
        }
    }

    private void EquipCurrentWeapon()
    {
        if (currentPrimaryWeapon != null)
        {
            currentPrimaryWeapon.gameObject.SetActive(false);
        }

        currentPrimaryWeapon = objWeapons[indexWeapon].GetComponent<Weapon>();
        OnAnyWeaponSelected?.Invoke(this, currentPrimaryWeapon);

        currentPrimaryWeapon.gameObject.SetActive(true);
    }

    public void ShowSecondaryWeapon()
	{
        secondaryWeapon.ActivateWeapon();
        currentPrimaryWeapon.DeactivateWeapon();
    }

    public void RestorePrimaryWeapon()
	{
        secondaryWeapon.DeactivateWeapon();
        currentPrimaryWeapon.ActivateWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        bool shootSecondary = userInput.Player.ShootSecondary.triggered && userInput.Player.ShootSecondary.ReadValue<float>() > 0;
        if (shootSecondary && secondaryWeapon != null)
        {
            secondaryWeapon.Shoot();
            animator.SetTrigger(secondaryWeapon.GetAnimationTrigger());
            OnAnyWeaponShot?.Invoke(this, secondaryWeapon);
        }

        if (numWeapons <= 0)
        {
            return;
        }
        
        float swap = userInput.Player.SwapWeapon.ReadValue<float>();
        if (swap > 0)
        {
            indexWeapon = (indexWeapon + 1) % numWeapons;
            EquipCurrentWeapon();
        }
        if (swap < 0)
        {
            indexWeapon = (indexWeapon + numWeapons - 1) % numWeapons;
            EquipCurrentWeapon();
        }

    }

    public bool AddAmmo(Weapon.WeaponType type, int ammoNumber)
    {
        bool result = false;

        foreach (var obj in objWeapons)
        {
            Weapon weapon = obj.GetComponent<Weapon>();
            if (weapon.GetWeaponType() == type)
            {
                result = true;
                weapon.AddAmmo(ammoNumber);
                OnAnyWeaponShot?.Invoke(this, currentPrimaryWeapon);
            }
        }


        if (secondaryWeapon != null && secondaryWeapon.GetWeaponType() == type)
        {
            result = true;
            secondaryWeapon.AddAmmo(ammoNumber);
            OnAnyWeaponShot?.Invoke(this, secondaryWeapon);
        }

        return result;
    }

    public void ResetComponent()
    {
        foreach (var weaponObject in objWeapons)
        {
            weaponObject.GetComponent<Weapon>().ResetAmmo();
        }

        if (secondaryWeapon != null)
        {
            secondaryWeapon.ResetAmmo();
        }

        OnAnyWeaponShot?.Invoke(this, currentPrimaryWeapon);
    }

    public UserInput GetUserInput()
    {
        return userInput;
    }

    public void ActivateSwordSlash()
	{
        if(secondaryWeapon != null && secondaryWeapon.GetWeaponType() == Weapon.WeaponType.Sword)
		{
            (secondaryWeapon as Sword).ActivateSwordSlash();
		}
	}

    public void DeactivateSwordSlash()
	{
        if (secondaryWeapon != null && secondaryWeapon.GetWeaponType() == Weapon.WeaponType.Sword)
        {
            (secondaryWeapon as Sword).DeactivateSwordSlash();
        }
    }

    public void FinishSwordSlash()
	{
        if (secondaryWeapon != null && secondaryWeapon.GetWeaponType() == Weapon.WeaponType.Sword)
        {
            (secondaryWeapon as Sword).FinishSwordSlash();
        }
    }
}
