using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float returnHomeSpeed;

    private Vector2 movementDirection;
    private Vector3 touchStartPos;
    private Vector3 touchPosition;

    private float minTouchDistance = 1f;
    private float maxTouchDistance = 2f;
    private float minTouchStartDistance = 0.6f;

    private int maxBounces = 1;
    private int bounceCounter;

    [SerializeField] private PlayerStand startStand;
    public PlayerStand currentStand;

    private Rigidbody2D playerRb;

    private enum State { Move, Wait, ReturnToHome, GameOver}
    private State state;

    private bool isDragging;

    [SerializeField] private TextMeshProUGUI touchCountText;
    [SerializeField] private TextMeshProUGUI touchPosText;
    [SerializeField] private TextMeshProUGUI distanceToPlayerText;
    [SerializeField] private TextMeshProUGUI touchPhaseText;

    private bool allowInput = true;
    public bool mobileInput;

    private bool tutorialIsPlaying;

    private CircleCollider2D circleCollider;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color deactiveColor;

    [SerializeField] private GameObject explosionParticlePlayer;

    [SerializeField] private GameObject playerSprite;

    private GameUIHandler gameUIHandler;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private LineRenderer swipeLine;
    //[SerializeField] private LineRenderer developerInfoTexts;

    [SerializeField] private GameObject playerParticle;

    [SerializeField] private AudioSource audioSource;

    private SaveSystem saveSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        saveSystem = SaveSystem.current;
        playerSprite.GetComponent<SpriteRenderer>().sprite = saveSystem.GetSelectedPlayerBallSprite();

        currentStand = startStand;
        currentStand.Activate();

        ChangeState(State.ReturnToHome);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Wait:
                if(allowInput && !tutorialIsPlaying)
                {
                    CheckForInput();
                }
                break;

            case State.Move:
                Move();
                break;

            case State.ReturnToHome:
                ReturnToHome();
                break;

            default:

                break;
        }
    }

    private void CheckForInput()
    {
        if (mobileInput)
        {
            if (CheckForTouchInput())
            {
                ChangeState(State.Move);
            }
        }
        else
        {
            if (CheckForMouseClick())
            {
                ChangeState(State.Move);
            }
        }
    }

    public void AllowInput(bool interactable)
    {
        allowInput = interactable;
    }

    public void IsTutorialPlaying(bool plays)
    {
        tutorialIsPlaying = plays;
    }

    public bool InputIsAllowed()
    {
        return allowInput;
    }

    private void Move()
    {
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
    }

    private void ReturnToHome()
    {
        if(Vector3.Distance(transform.position, currentStand.transform.position) <= 0.5)
        {
            transform.position = currentStand.transform.position;
            ChangeState(State.Wait);
        }
        else
        {
            var dir = (currentStand.transform.position - transform.position).normalized;
            transform.Translate(dir * returnHomeSpeed * Time.deltaTime);
            RotatePlayerSprite(dir);
        }
    }

    private void ChangeState(State newState)
    {
        switch(newState)
        {
            case State.Move:
                circleCollider.isTrigger = false;
                spriteRenderer.color = activeColor;

                RotatePlayerSprite(movementDirection);
                SquischPlayerSprite();

                state = State.Move;
                break;

            case State.ReturnToHome:
                circleCollider.isTrigger = false;
                spriteRenderer.color = activeColor;

                SquischPlayerSprite();

                state = State.ReturnToHome;
                break;

            case State.Wait:
                circleCollider.isTrigger = true;
                spriteRenderer.color = deactiveColor;

                DeSquischPlayerSprite();

                state = State.Wait;
                break;

            case State.GameOver:

                state = State.GameOver;
                break;
        }
    }

    private bool CheckForMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;
            swipeLine.SetPosition(0, transform.position);
            swipeLine.SetPosition(1, mouse);

            if(mouse.y <= -2.5f)
            {
                return false;
            }

            movementDirection = (mouse - transform.position).normalized;

            audioSource.Play();
            return true;
        }
        return false;
    }

    private bool CheckForTouchInput()
    {
        touchCountText.SetText("Touch Count: " + Input.touchCount);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            touchPosText.SetText("Touch Pos: " + touchPosition.x + ", " + touchPosition.y);
            touchPosition.z = 0;

            distanceToPlayerText.SetText("Distance: " + Vector3.Distance(transform.position, touchPosition));

            if (touch.phase == TouchPhase.Began)
            {
                touchPhaseText.SetText("Phase: Began");
                swipeLine.SetPosition(0, touchPosition);
                swipeLine.SetPosition(1, touchPosition);
                touchStartPos = touchPosition;

                if (Vector3.Distance(transform.position, touchPosition) <= minTouchStartDistance)
                {
                    transform.position = touchStartPos;
                    isDragging = true;
                }
            }
            else if(touch.phase == TouchPhase.Moved && isDragging)
            {
                touchPhaseText.SetText("Phase: Moved");
                swipeLine.SetPosition(1, touchPosition);

                if (Vector3.Distance(transform.position, touchPosition) >= maxTouchDistance && touchPosition.y >= -2.5f)
                {
                    touchPhaseText.SetText("Phase: Shoot");

                    movementDirection = (touchPosition - touchStartPos).normalized;

                    isDragging = false;
                    audioSource.Play();
                    return true;
                }
            }
            else if (touch.phase == TouchPhase.Ended && isDragging && Vector3.Distance(transform.position, touchPosition) >= minTouchDistance && touchPosition.y >= -2.5f)
            {
                touchPhaseText.SetText("Phase: Ended");
                swipeLine.SetPosition(1, touchPosition);

                movementDirection = (touchPosition - touchStartPos).normalized;

                isDragging = false;
                audioSource.Play();
                return true;
            }
            else if((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isDragging)
            {
                transform.position = currentStand.transform.position;
                isDragging = false;
            }
        }
        return false;
    }

    public void Die()
    {
        Instantiate(explosionParticlePlayer, currentStand.transform.position, Quaternion.identity);
        ChangeState(State.GameOver);

        Destroy(gameObject);
    }

    public void ChangeStand(PlayerStand newStand)
    {
        if(currentStand.transform.position == newStand.transform.position || !allowInput || tutorialIsPlaying)
        {
            return;
        }

        currentStand.DeActivate();
        currentStand = newStand;
        currentStand.Activate();
        currentStand.MakeSound();

        if (state == State.Wait)
        {
           ChangeState(State.ReturnToHome);
        }
    }

    public void ChangeStateToGoHome()
    {
        bounceCounter = 0;
        ChangeState(State.ReturnToHome);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCounter++;
        if(bounceCounter >= maxBounces)
        {
            ChangeState(State.ReturnToHome);
            bounceCounter = 0;
        }
        else
        {
            movementDirection = Vector2.Reflect(movementDirection, collision.GetContact(0).normal);
        }
        Instantiate(playerParticle, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal, Vector2.up));
    }

    private void SquischPlayerSprite()
    {
        playerSprite.transform.localScale = new Vector3(0.5f, 0.4f);
    }

    private void DeSquischPlayerSprite()
    {
        playerSprite.transform.localScale = new Vector3(0.5f, 0.5f);
    }

    private void RotatePlayerSprite(Vector2 rotationDir)
    {
        float angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg;
        playerSprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
