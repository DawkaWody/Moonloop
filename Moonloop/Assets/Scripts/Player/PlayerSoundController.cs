using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _respawnSound;

    private AudioSource _audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        _audioSource.PlayOneShot(_jumpSound);
    }

    public void PlayDeathSound()
    {
        _audioSource.PlayOneShot(_deathSound);
    }

    public void PlayRespawnSound()
    {
        _audioSource.PlayOneShot(_respawnSound);
    }
}
