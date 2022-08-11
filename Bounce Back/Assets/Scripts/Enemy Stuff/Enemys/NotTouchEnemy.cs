using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotTouchEnemy : MonoBehaviour, IEnemyMovement
{
    [SerializeField] private float startMovementSpeed;
    private float movementSpeed;

    [SerializeField] private GameObject explosionParticleEnemyWallmix;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        movementSpeed = startMovementSpeed;
    }

    private void Update()
    {
        MoveDown();

        if (transform.position.y <= -5)
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
        transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameManager.GetDamage(1, false);
        gameManager.ShowScreenShake();

        Explode();
    }

    private void Explode()
    {
        Instantiate(explosionParticleEnemyWallmix, transform.position, Quaternion.identity);
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
