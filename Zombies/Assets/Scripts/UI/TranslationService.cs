using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TranslationService : MonoBehaviour
{
	public static TranslationService Instance = null;
	[SerializeField] public Dictionary<(string, Language), string> translations;


	[Serializable]
	public class DictionaryKey
	{
		public string term;
		public string lang;
		public string translation;
	}

	[Serializable]
	public class JsonData
	{
		public List<DictionaryKey> translations;
	}

	public enum Language { en, es };

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There is another translation service.");
			return;
		}

		Instance = this;

		LoadTranslationData();
	}

	private void LoadTranslationData()
	{
		var jsonAsset = Resources.Load<TextAsset>("translations");
		var data = JsonUtility.FromJson<JsonData>(jsonAsset.ToString());

		translations = new();
		foreach (var node in data.translations)
		{
			Language lang = (node.lang.Equals("es")) ? Language.es : Language.en;
			translations.Add((node.term, lang), node.translation);
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	public string Translate(string term, Language lang) => (translations.ContainsKey((term, lang))) ? translations[(term, lang)] : term;

	// Update is called once per frame
	void Update()
	{

	}
}
