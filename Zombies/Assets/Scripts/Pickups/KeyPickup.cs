using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour, IPickup
{
    [SerializeField] private string KeyName = "Key";

    public void SetKeyName(string keyName)
    {
        KeyName = keyName;
    }

    public bool ApplyPickup(GameObject receiver)
    {
        bool result = receiver.TryGetComponent(out KeyChain keychain);

        if (result && !keychain.HasKey(KeyName))
        {
            keychain.AddKey(KeyName);
        }

        return result;
    }
}
