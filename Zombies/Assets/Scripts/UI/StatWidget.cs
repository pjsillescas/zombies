using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatWidget : MonoBehaviour
{
    public static StatWidget Instance;

    [SerializeField] private TextMeshProUGUI StatText;

    private int numRemainingZombies;
    private int numKilledZombies;

	private void Awake()
	{
        if (Instance != null)
        {
            Debug.LogError("");
            return;
        }

        Instance = this;

        numRemainingZombies = 0;
        numKilledZombies = 0;
	}

    public void UpdateNumRemainingZombies(int numRemainingZombies)
	{
        this.numRemainingZombies = numRemainingZombies;
        FormatWidgetString();
    }
    
    public void UpdateNumKilledZombies(int numKilledZombies)
    {
        this.numKilledZombies = numKilledZombies;
        FormatWidgetString();
    }

    private void FormatWidgetString()
	{
        if (StatText != null)
        {
            StatText.text = $"{numKilledZombies} / {numRemainingZombies}";
        }
	}

    // Start is called before the first frame update
    void Start()
    {
        ;
    }

    // Update is called once per frame
    void Update()
    {
        ;
    }
}
