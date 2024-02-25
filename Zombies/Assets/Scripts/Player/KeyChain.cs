using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyChain : MonoBehaviour
{
	public event EventHandler<string> OnKeyAdded;
	private List<string> keys;

	private void Awake()
	{
		keys = new();
	}

	public void AddKey(string key)
	{
		keys.Add(key);
		OnKeyAdded?.Invoke(this, key);
	}

	public bool HasKey(string key)
	{
		return keys.Exists((k) => k == key);
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
