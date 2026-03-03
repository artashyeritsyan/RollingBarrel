using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static event Action<bool> OnItemCollect;
    public static event Action OnPlayerKilled;

    [SerializeField] float moveSpeed = 100;
    [SerializeField] float dashForce = 100;
    [SerializeField] float dashCost = 10;
    [SerializeField] int dashMaxCount = 2;
    private int dashesLeft;

    [SerializeField] MenuController menuController;

    Rigidbody rb;
    float gravity = -30f;
    [SerializeField] Vector3 spawnPoint = new(30, 0.5f, -188);

    public float movementX;
    float movementY;
    Vector3 moveDirection;

    bool gamePaused;
    bool isOverdrunked;
    bool isOnGround;
    bool isHaveSecondJump;

    Transform child;

    [SerializeField] Transform cameraTransform;

    private int alcoholLevel = 0;

    public GameObject beerCollectionEffect;

    [Header("Sounds")]
    public AudioSource BeerPouringSound;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0f, gravity, 0f);
        child = gameObject.GetComponentInChildren<Transform>();

        alcoholLevel = 0;
        gamePaused = false;
        isOverdrunked = false;
    }

    void FixedUpdate()
    {
        if (!gamePaused)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            moveDirection = camForward * movementY + camRight * movementX * 0.8f;

            //if (isOverdrunked)
            //{
            //    //float randomX = Random.Range(-1f, 1f);
            //    //float randomY = Random.Range(-1f, 1f);
            //    //movement = new Vector3(0, 0, -movementY);
            //    moveDirection = -moveDirection;
            //}

            rb.AddForce(moveDirection * moveSpeed);
        }
    }

    void OnEnable()
    {
        MenuController.ChangeAlcoholLevel += SetAlcoholLevel;
        MenuController.OnGameReset += ResetHandler;
        MenuController.OverDrunked += OverdrunkHandler;
    }

    void OnDisable()
    {
        MenuController.ChangeAlcoholLevel -= SetAlcoholLevel;
        MenuController.OnGameReset -= ResetHandler;
        MenuController.OverDrunked -= OverdrunkHandler;
    }

    void SetAlcoholLevel(int newLevel)
    {
        alcoholLevel = newLevel;
    }

    void ResetHandler ()
    {
        Restart();
    }

    public void Restart()
    {
        gamePaused = false;
        rb.linearVelocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        transform.position = spawnPoint;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void OverdrunkHandler(int duration)
    {
        StartCoroutine(WaitToComeArond(duration));
    }

    IEnumerator WaitToComeArond(int duration)
    {
        isOverdrunked = true;
        rb.linearVelocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);

        isOverdrunked = false;
        rb.linearVelocity = new Vector3(0, 0, 0);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    private void OnDash()
    {
        Debug.Log("Dash");

        if (menuController.GetBeerFilledValue() >= dashCost && dashesLeft > 0)
        {
            rb.AddForce((moveDirection + new Vector3(0, 0.5f, 0)) * dashForce);
            menuController.reduceBeerFilledValue(dashCost);
            Instantiate(beerCollectionEffect, transform.position, Quaternion.identity);
            Debug.Log("Pooof");

            --dashesLeft;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Restart();
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            OnPlayerKilled?.Invoke();
            Restart();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            dashesLeft = dashMaxCount;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Beer"))
        {
            BeerPouringSound.Play();
            other.gameObject.SetActive(false);
            CollectBeer();
        }

        if (other.gameObject.CompareTag("Snack"))
        {
            other.gameObject.SetActive(false);
            CollectSnack();
        }
        

    }

    private void CollectBeer()
    {
        OnItemCollect?.Invoke(true);
    }

    private void CollectSnack()
    {
        OnItemCollect?.Invoke(false);
    }

}
