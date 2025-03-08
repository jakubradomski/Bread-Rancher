using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BreadSounds : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] sounds;

    public float minInterval = 10f;
    public float maxInterval = 40f;

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySoundRepeatedly());
    }

    IEnumerator PlaySoundRepeatedly()
    {
        while(true)
        {
            float randomInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomInterval);

            AudioClip selectedClip = GetRandomClip();
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(selectedClip);
        }
    }

    private AudioClip GetRandomClip()
    {
        return sounds[Random.Range(0, sounds.Length)];
    }
}
