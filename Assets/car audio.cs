using UnityEngine;

public class CarAudioManager : MonoBehaviour
{
    public AudioSource engineSource;
    public AudioSource idleMusicSource;
    public AudioSource drivingMusicSource;

    public AudioClip idleClip;
    public AudioClip slowAccelClip;
    public AudioClip mediumAccelClip;
    public AudioClip fastAccelClip;
    public AudioClip slowDecelClip;
    public AudioClip mediumDecelClip;
    public AudioClip fastDecelClip;

    public float slowThreshold = 2f;
    public float mediumThreshold = 5f;
    public float fastThreshold = 10f;
    public float musicFadeSpeed = 2f;

    private Rigidbody carRigidbody;
    private float previousSpeed = 0f;
    private CarController carController;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carController = GetComponent<CarController>();

        if (engineSource == null)
        {
            engineSource = gameObject.AddComponent<AudioSource>();
            engineSource.loop = true;
        }
        engineSource.clip = idleClip;
        engineSource.Play();

        idleMusicSource.loop = true;
        drivingMusicSource.loop = true;

        idleMusicSource.volume = 1f;
        drivingMusicSource.volume = 0f;

        idleMusicSource.Play();
        drivingMusicSource.Play();
    }

    void Update()
    {
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        float speedChange = currentSpeed - previousSpeed;
        previousSpeed = currentSpeed;

        if (carController.rtrigger.action.IsPressed()) // Accelerating
        {
            PlayAccelerationSound(speedChange);
        }
        else if (carController.ltrigger.action.IsPressed()) // Braking
        {
            PlayDecelerationSound(speedChange);
        }
        else // Idle
        {
            if (engineSource.clip != idleClip)
            {
                engineSource.clip = idleClip;
                engineSource.Play();
            }
        }

        // Handle background music fading
        if (currentSpeed < 0.1f) // Car is idle
        {
            idleMusicSource.volume = Mathf.Lerp(idleMusicSource.volume, 1f, Time.deltaTime * musicFadeSpeed);
            drivingMusicSource.volume = Mathf.Lerp(drivingMusicSource.volume, 0f, Time.deltaTime * musicFadeSpeed);
        }
        else // Car is moving
        {
            idleMusicSource.volume = Mathf.Lerp(idleMusicSource.volume, 0f, Time.deltaTime * musicFadeSpeed);
            drivingMusicSource.volume = Mathf.Lerp(drivingMusicSource.volume, 1f, Time.deltaTime * musicFadeSpeed);
        }
    }

    private void PlayAccelerationSound(float speedChange)
    {
        if (speedChange < slowThreshold)
        {
            ChangeAudioClip(slowAccelClip);
        }
        else if (speedChange < mediumThreshold)
        {
            ChangeAudioClip(mediumAccelClip);
        }
        else
        {
            ChangeAudioClip(fastAccelClip);
        }
    }

    private void PlayDecelerationSound(float speedChange)
    {
        float deceleration = Mathf.Abs(speedChange);
        if (deceleration < slowThreshold)
        {
            ChangeAudioClip(slowDecelClip);
        }
        else if (deceleration < mediumThreshold)
        {
            ChangeAudioClip(mediumDecelClip);
        }
        else
        {
            ChangeAudioClip(fastDecelClip);
        }
    }

    private void ChangeAudioClip(AudioClip newClip)
    {
        if (engineSource.clip != newClip)
        {
            engineSource.clip = newClip;
            engineSource.Play();
        }
    }
}
