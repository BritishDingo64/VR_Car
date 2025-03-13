using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Sources -----------")]
    [SerializeField] private AudioSource musicSource;  // Background music audio source
    [SerializeField] private AudioSource SFXSource;    // General sound effects audio source

    [Header("--------- Audio Clips -----------")]
    public AudioClip background;      // Normal background music clip
    public AudioClip backgroundFast; // Fast background music clip (for when the car is speeding)
    public AudioClip UISound;         // Jumpscare sound effect
    public AudioClip doorPhase;       // Door open sound effect
    public AudioClip revSound;        // Rev sound effect (when accelerating the car)

    void Start()
    {
        // Start normal background music when the game begins
        if (background != null && musicSource != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;  // Loop the background music
            musicSource.Play();       // Play the background music
        }
    }

    // Method to get the current music volume
    public float GetMusicVolume()
    {
        if (musicSource != null)
        {
            return musicSource.volume;
        }
        return 0.1f;  // Default volume if the musicSource is not found
    }

    // Method to adjust the volume of the background music
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);  // Clamp volume between 0 and 1
        }
    }

    // Play the jumpscare sound effect
    public void PlayUISound()
    {
        if (UISound != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(UISound);  // Play jumpscare sound once
        }
    }

    // Play the door open sound effect
    public void PlayDoorPhase()
    {
        if (doorPhase != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(doorPhase);  // Play door open sound once
        }
    }

    // Play the rev sound effect when accelerating the car
    public void PlayRevSound()
    {
        if (revSound != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(revSound);  // Play rev sound once
        }
    }

    // Switch background music based on the car's speed
    public void SwitchBackgroundMusic(bool isFast)
    {
        if (musicSource != null)
        {
            if (isFast && backgroundFast != null)
            {
                // Switch to fast background music
                if (musicSource.clip != backgroundFast)
                {
                    musicSource.clip = backgroundFast;
                    musicSource.Play(); // Play the fast background music
                }
            }
            else if (!isFast && background != null)
            {
                // Switch back to normal background music
                if (musicSource.clip != background)
                {
                    musicSource.clip = background;
                    musicSource.Play(); // Play the normal background music
                }
            }
        }
    }
}
