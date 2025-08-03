using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioClip _ambience;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _ambienceSource;

    public void PlayMenuMusic()
    {
        if (_musicSource.isPlaying) _musicSource.Stop();
        _musicSource.clip = _menuMusic;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlayGameMusic()
    {
        if (_musicSource.isPlaying) _musicSource.Stop();
        _musicSource.clip = _gameMusic;
        _musicSource.loop = true;
        _musicSource.Play();

        if (_ambienceSource.isPlaying) _ambienceSource.Stop();
        _ambienceSource.clip = _ambience;
        _ambienceSource.loop = true;
        _ambienceSource.Play();
    }

    public void LerpVolume(float time, float to)
    {
        StartCoroutine(LerpVolumeCo(time, to));
    }

    IEnumerator LerpVolumeCo(float time, float to)
    {
        float elapsedTime = 0f;
        float startVolume = _musicSource.volume;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(startVolume, to, elapsedTime / time);
            yield return null;
        }
    }
}
