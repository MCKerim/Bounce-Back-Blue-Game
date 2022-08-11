using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void PlaySound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }
}
