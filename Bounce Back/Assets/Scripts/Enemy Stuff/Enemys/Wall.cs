using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IEnemyMovement
{
    [SerializeField] private float startMovementSpeed;
    private float movementSpeed;

    private PlayerController playerController;

    [SerializeField] private GameObject explosionParticleWall;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        movementSpeed = startMovementSpeed;
    }

    private void Update()
    {
        MoveDown();

        if (Vector3.Distance(transform.position, playerController.currentStand.transform.position) <= 0.9f)
        {
            gameManager.GameOver();
            Explode();
        }

        if(transform.position.y <= -5)
        {
            gameManager.IncreaseScore(1);
            Destroy(gameObject);
        }

        if(gameManager.gameOver)
        {
            Explode();
        }
    }

    private void MoveDown()
    {
        transform.Translate(-Vector3.up * movementSpeed * Time.deltaTime);
    }

    private void Explode()
    {
        Instantiate(explosionParticleWall, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void StopMoving()
    {
        movementSpeed = 0;
    }

    public void Startmoving()
    {
        movementSpeed = startMovementSpeed;
    }
}
