using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketShip : MonoBehaviour {
    private Rigidbody rigidBody;
    private AudioSource audioSource;

	private bool invulnerable = false;
	private bool inWater = false;
	private bool heatShield = false;
	private float immunityTimeStep = 0f;

	private Vector3 originalGravity = Vector3.zero;

    private enum State { Alive, Dying, Trancending}
    private State state = State.Alive;

	public enum PowerUpTypes { Heatshield, None }
	private PowerUpTypes powerUp = PowerUpTypes.None;

    [SerializeField] private float mainThrust = 1f;
    [SerializeField] private float rcsThrust = 1f;
	[Space]
	[SerializeField] private Vector3 waterGravity = Vector3.zero;
	[SerializeField] private float waterThrust = 1f;
	[SerializeField] private float waterRCS = 1f;
	[Space]
	[SerializeField] private float loadDelay = 1f;
	[Space]
	[SerializeField] private float heatShieldDurration = 2.5f;
	[Space]
	[SerializeField] private ParticleSystem mainThrustParticles = null;
	[SerializeField] private ParticleSystem deathParticles = null;
	[SerializeField] private ParticleSystem sucessParticles = null;
	[SerializeField] private ParticleSystem splashParticle = null;
	[Space]
	[SerializeField] private AudioClip mainThrustSfx = null;
	[SerializeField] private AudioClip deathSfx = null;
	[SerializeField] private AudioClip jingleSfx = null;
	[Space]
	[SerializeField] private MeshRenderer shield = null;

	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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

	private void Recoil(Vector3 dir, float recoilAmount)
	{
		rigidBody.AddRelativeForce(dir * recoilAmount * Time.deltaTime);
	}

	private void StartSucessSequence()
	{
		mainThrustParticles.Stop();
		audioSource.Stop();
		audioSource.PlayOneShot(jingleSfx);
		sucessParticles.Play();
		state = State.Trancending;
		Invoke("LoadNextScene", loadDelay + jingleSfx.length);
	}

	private void StartDeathSequence()
	{
		mainThrustParticles.Stop();
		audioSource.Stop();
		audioSource.PlayOneShot(deathSfx);
		deathParticles.Play();
		state = State.Dying;
		Invoke("LoadFirstLevel", loadDelay + deathSfx.length);
	}

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

	private void RespondToDebugInput()
	{
		if( Input.GetKeyDown(KeyCode.C))
		{
			invulnerable = !invulnerable;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			LoadNextScene();
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
				case PowerUpTypes.None:
					break;
				case PowerUpTypes.Heatshield:
					TriggerHeatShield();
					powerUp = PowerUpTypes.None;
					break;
			}
		}
	}

	private void DoPowerUpTick()
	{
		if(heatShield && Time.time - immunityTimeStep >= heatShieldDurration)
		{
			heatShield = false;
			shield.enabled = false;
		}
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		state = State.Alive;
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		state = State.Alive;
	}

	private void TriggerHeatShield()
	{
		heatShield = true;
		shield.enabled = true;
		immunityTimeStep = Time.time;
	}

	public void AwardPowerUp(PowerUpTypes powerUp)
	{
		if (this.powerUp == PowerUpTypes.None) this.powerUp = powerUp;
	}

	//TEMP
	public void Quit()
	{
		Application.Quit();
	}

}
