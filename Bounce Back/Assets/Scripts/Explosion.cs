using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.pitch = Random.Range(0.7f, 1.3f);
    }
}
