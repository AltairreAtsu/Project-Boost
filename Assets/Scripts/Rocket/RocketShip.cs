using UnityEngine;


public partial class RocketShip : MonoBehaviour {
	#region Component Variables
	private Rigidbody rigidBody;
    private AudioSource audioSource;
	private LevelManager levelManager;
	#endregion

	#region Power Up Variables
	private PowerUp.Types powerUp = PowerUp.Types.None;

	private bool heatShield = false;
	private float immunityTimeStep = 0f;
	#endregion

	private bool invulnerable = false;

	private bool inWater = false;
	private Vector3 originalGravity = Vector3.zero;

	private enum State { Alive, Dying, Trancending }
	private State state = State.Alive;

	// Serialized Fields
	#region Thrust Varriables
	[Header ("Thrust Varriables")]
    [SerializeField] private float mainThrust = 1f;
    [SerializeField] private float rcsThrust = 1f;
	[Space]
	[SerializeField] private Vector3 waterGravity = Vector3.zero;
	[SerializeField] private float waterThrust = 1f;
	[SerializeField] private float waterRCS = 1f;
	#endregion
	#region Particle System Variables
	[Header ("Particle Systems")]
	[SerializeField] private ParticleSystem mainThrustParticles = null;
	[SerializeField] private ParticleSystem deathParticles = null;
	[SerializeField] private ParticleSystem sucessParticles = null;
	[SerializeField] private ParticleSystem splashParticle = null;
	#endregion
	#region Audio Clip Refrences
	[Header ("Audio Clips")]
	[SerializeField] private AudioClip mainThrustSfx = null;
	[SerializeField] private AudioClip deathSfx = null;
	[SerializeField] private AudioClip jingleSfx = null;
	#endregion
	#region Other Variables
	[Header ("Misc")]
	[SerializeField] private MeshRenderer shield = null;
	[SerializeField] private float heatShieldDurration = 2.5f;
	[SerializeField] private float loadDelay = 1f;
	#endregion

	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
		levelManager = GetComponent<LevelManager>();
		originalGravity = Physics.gravity;
	}
	
	void Update () {
		if (state == State.Alive)
		{
			RespondToThrustInput();
			RespondToRotationInput();
			RespondToPowerUpInput();
			DoPowerUpTick();
			if (Application.isEditor)
				RespondToDebugInput();
		}

	}

	#region Collision and Trigger Handling
	private void OnCollisionEnter(Collision collision)
    {
		if (state != State.Alive || invulnerable) { return; }

        switch (collision.gameObject.tag)
		{
			case "Friendly":
				print("Okay!");
				break;
			case "Finish":
				StartSucessSequence();
				break;
			default:
				StartDeathSequence();
				break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (state != State.Alive) { return; }
		if (other.tag == "Fire" && heatShield) { return; }

		if(other.tag == "Water" && !inWater)
		{
			Physics.gravity = waterGravity;
			inWater = true;

			splashParticle.GetComponent<AudioSource>().Play();
			splashParticle.transform.position  = transform.position;
			splashParticle.Play();

			return;
		}

		if (other.tag != "Trigger" && other.tag != "Water")
		{
			StartDeathSequence();
		}
	}

	private void OnTriggerStay(Collider other)
	{
		AirHazard airHazard = other.GetComponent<AirHazard>();

		if (airHazard != null)
		{
			Recoil(other.transform.up, airHazard.GetRecoil());
			return;
		}

	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Water" && inWater)
		{
			Physics.gravity = originalGravity;
			inWater = false;
		}
	}
	#endregion

	#region Input Handling
	private void RespondToRotationInput()
    {
        rigidBody.freezeRotation = true;
		float rotationThisFrame;

		if (inWater)
		{
			 rotationThisFrame = waterRCS * Time.deltaTime;
		}
		else
		{
			 rotationThisFrame = rcsThrust * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
		{
			ApplyThrust();
		}
		else
        {
			audioSource.Stop();
			mainThrustParticles.Stop();
        }
    }

	private void RespondToDebugInput()
	{
		if( Input.GetKeyDown(KeyCode.C))
		{
			invulnerable = !invulnerable;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			levelManager.LoadNextScene();
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			TriggerHeatShield();
		}
	}

	private void RespondToPowerUpInput()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			switch (powerUp)
			{
				case PowerUp.Types.None:
					break;
				case PowerUp.Types.Heatshield:
					TriggerHeatShield();
					powerUp = PowerUp.Types.None;
					break;
			}
		}
	}
	#endregion

	#region Thrust Application
	private void Recoil(Vector3 dir, float recoilAmount)
	{
		rigidBody.AddRelativeForce(dir * recoilAmount * Time.deltaTime);
	}

	private void ApplyThrust()
	{
		if (inWater)
		{
			rigidBody.AddRelativeForce(Vector3.up * waterThrust * Time.deltaTime);
		}
		else
		{
			rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
		}


		if (!audioSource.isPlaying)
			audioSource.PlayOneShot(mainThrustSfx);
		mainThrustParticles.Play();
	}
	#endregion

	#region Death and Victory Handling
	private void StartSucessSequence()
	{
		mainThrustParticles.Stop();
		audioSource.Stop();
		audioSource.PlayOneShot(jingleSfx);
		sucessParticles.Play();
		state = State.Trancending;
		levelManager.Invoke("LoadNextScene", loadDelay + jingleSfx.length);
	}

	private void StartDeathSequence()
	{
		mainThrustParticles.Stop();
		audioSource.Stop();
		audioSource.PlayOneShot(deathSfx);
		deathParticles.Play();
		Physics.gravity = originalGravity;
		state = State.Dying;
		levelManager.Invoke("ReloadLevel", loadDelay + deathSfx.length);
	}
	#endregion

	#region PowerUp Handling
	private void DoPowerUpTick()
	{
		if(heatShield && Time.time - immunityTimeStep >= heatShieldDurration)
		{
			heatShield = false;
			shield.enabled = false;
		}
	}

	public void AwardPowerUp(PowerUp.Types powerUp)
	{
		if (this.powerUp == PowerUp.Types.None) this.powerUp = powerUp;
	}

	private void TriggerHeatShield()
	{
		heatShield = true;
		shield.enabled = true;
		immunityTimeStep = Time.time;
	}
	#endregion
}
