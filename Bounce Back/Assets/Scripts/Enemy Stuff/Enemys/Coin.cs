using UnityEngine;

public class Coin : MonoBehaviour, IEnemyMovement
{
    [SerializeField] private int minCoins;
    [SerializeField] private int maxCoins;
    [SerializeField] private PopUpText coinsTextPrefab;

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
        int randomCoins = Random.Range(minCoins, maxCoins + 1);
        SaveSystem.current.ChangeCoins(randomCoins);
        gameManager.IncreasCoinsFromOneRound(randomCoins);

        PopUpText coinsTextGameObject = Instantiate(coinsTextPrefab, transform.position, Quaternion.identity);
        coinsTextGameObject.Serialize("+" + randomCoins + " Coins");

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
