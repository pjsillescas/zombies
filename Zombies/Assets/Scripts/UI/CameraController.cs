using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera CameraMain;
	[SerializeField] private CinemachineVirtualCamera CameraOptions;
	[SerializeField] private Canvas MainMenuCanvas;
	[SerializeField] private Canvas OptionsMenuCanvas;

	public static CameraController Instance;

	private bool isMain;

	// Start is called before the first frame update
	void Start()
	{
		if (Instance != null)
		{
			Debug.LogError("There is another camera controller.");
			return;
		}

		Instance = this;

		OptionsMenuCanvas.enabled = false;
		CameraOptions.enabled = false;
		CameraMain.enabled = true;
		MainMenuCanvas.enabled = true;
	}

	public void SwitchCamera()
	{
		CameraOptions.enabled = !CameraOptions.enabled;
		CameraMain.enabled = !CameraMain.enabled;
		OptionsMenuCanvas.enabled = !OptionsMenuCanvas.enabled;
		MainMenuCanvas.enabled = !MainMenuCanvas.enabled;

	}

	// Update is called once per frame
	void Update()
	{

	}
}
