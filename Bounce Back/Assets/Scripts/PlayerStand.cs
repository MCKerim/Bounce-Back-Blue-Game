using UnityEngine;

public class PlayerStand : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject background;
    [SerializeField] private AudioSource audioSource;

    private void OnMouseDown()
    {
        if(playerController != null)
        {
            playerController.ChangeStand(this);
        }
    }

    public void Activate()
    {
        background.SetActive(true);
    }

    public void DeActivate()
    {
        background.SetActive(false);
    }

    public void MakeSound()
    {
        audioSource.Play();
    }
}
