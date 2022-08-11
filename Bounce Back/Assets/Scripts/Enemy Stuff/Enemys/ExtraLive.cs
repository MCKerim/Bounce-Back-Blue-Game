using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLive : MonoBehaviour, IEnemyMovement
{
    [SerializeField] private PopUpText extraLiveTextPrefab;

    [SerializeField] private float startMovementSpeed;
    private float movementSpeed;

    [SerializeField] private GameObject explosionParticle;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameManager.GetDamage(-1, false);

        PopUpText extraLiveTextGameObject = Instantiate(extraLiveTextPrefab, transform.position, Quaternion.identity);
        extraLiveTextGameObject.Serialize("+1 Live");

        gameManager.ShowScreenShake();

        Explode();
    }

    private void Explode()
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
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
