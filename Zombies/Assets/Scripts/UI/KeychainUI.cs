using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeychainUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> KeyList;
    private int numKeys = 0;
    private TranslationService.Language language;

	// Start is called before the first frame update
	void Start()
    {
        language = GameConfiguration.GetLanguage();
        ResetKeychain();
        KeyChain keychain = FindAnyObjectByType<KeyChain>();
        if(keychain != null)
		{
            keychain.OnKeyAdded += OnKeyAdded;
		}
    }

    private void OnKeyAdded(object sender, string key)
	{
        AddKey(key);
	}

    public void ResetKeychain()
	{
        foreach (var text in KeyList)
        {
            text.text = "";
        }

        numKeys = 0;
    }

    public void AddKey(string key)
	{
        if(numKeys < KeyList.Count)
		{
            KeyList[numKeys].text = TranslationService.Instance.Translate(key.ToLower(),language);
            numKeys++;
		}
	}

}
