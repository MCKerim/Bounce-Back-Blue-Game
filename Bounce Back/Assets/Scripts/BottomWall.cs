using UnityEngine;

public class BottomWall : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerController.ChangeStateToGoHome();
    }
}
