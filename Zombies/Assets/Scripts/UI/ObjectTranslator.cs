using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ObjectTranslator : MonoBehaviour
{
	[Serializable]
	public struct ObjectData
	{
		public TextMeshProUGUI text;
		public string key;
	}

	[SerializeField] private List<ObjectData> Objects;
	public static ObjectTranslator Instance = null;

	// Start is called before the first frame update
	void Start()
	{
		if (Instance != null)
		{
			Debug.LogError("There is another object translator");
			return;
		}

		Instance = this;

		TranslationService.Language language = GameConfiguration.GetLanguage();
		Translate(language);
	}

	public void Translate(TranslationService.Language language)
	{
		foreach (var data in Objects)
		{
			data.text.text = TranslationService.Instance.Translate(data.key, language);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
