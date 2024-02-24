using Cinemachine;
using pec4.car;
using UnityEngine;

[RequireComponent(typeof(PecCarController))]
public class PecCarUserControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CarCamera;
    [SerializeField] private Transform DoorTransform;
    private PecCarController m_Car; // the car controller we want to use
    private PecCarAudio carAudio;
    private UserInput userInput;
    private bool isPossessed;
    private PlayerController rider;
    private CarAIManager aiManager;

    private void Awake()
    {
        // get the car controller
        m_Car = GetComponent<PecCarController>();
        carAudio = GetComponent<PecCarAudio>();
        userInput = new UserInput();
        aiManager = GetComponent<CarAIManager>();

        Unpossess();
    }

    private void FixedUpdate()
    {
        var moveVector = userInput.Player.Move.ReadValue<Vector2>();

        // pass the input to the car!
        float h = moveVector.x;
        float v = moveVector.y;
        float handbrake = userInput.Player.Jump.ReadValue<float>();
        m_Car.Move(h, v, v, handbrake);
    }

    public void Possess()
	{
        if (!isPossessed && rider != null)
        {
            if(aiManager != null && aiManager.IsAIDisabled())
			{
                aiManager.DisableAI();
			}

            isPossessed = true;
            userInput.Enable();
            rider.DisablePlayerCamera();
            CarCamera.enabled = true;
            rider.gameObject.SetActive(false);
            carAudio.UseSound(true);
        }
    }

    public void Unpossess()
	{
        if (isPossessed && rider != null)
        {
            carAudio.UseSound(false);
            isPossessed = false;
            userInput.Disable();
            CarCamera.enabled = false;
            rider.EnablePlayerCamera();

            rider.transform.position = DoorTransform.position;
            rider.transform.rotation = DoorTransform.rotation;
            rider.gameObject.SetActive(true);

            rider = null;
        }
    }

    public void SetRider(PlayerController playerController)
	{
        rider = playerController;
	}

	private void Update()
	{
		if(isPossessed && userInput.Player.Interaction.triggered)
		{
            Unpossess();
		}
	}
}
