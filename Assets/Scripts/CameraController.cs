using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerController playerController;

    [SerializeField] Vector3 delta = new(0, 9, -8);
    [SerializeField] float cameraRotationX = 15;

    [Header("Controls")]
    [SerializeField] float rotationSpeed = 100f;
    public float smoothTime = 0.3f;

    [Header("Collision")]
    [SerializeField] LayerMask collisionMask = ~0; 
    [SerializeField] float cameraRadius = 0.25f;
    [SerializeField] float collisionBuffer = 0.15f; 

    float yawAngle;
    float targetYaw;
    float yawVelocity;

    Vector3 positionVelocity;

    bool gamePaused;

    private int alcoholLevel = 0;

    void Start()
    {
        alcoholLevel = 0;
        gamePaused = false;
    }

    void LateUpdate()
    {
        if (!gamePaused)
        {
            CameraRotationHandler();
        }
    }

    void CameraRotationHandler()
    {
        playerController = player.GetComponent<PlayerController>();

        float inputX = playerController.movementX;
        targetYaw += inputX * rotationSpeed * Time.deltaTime;

        yawAngle = Mathf.SmoothDampAngle(yawAngle, targetYaw, ref yawVelocity, smoothTime);


        Quaternion rot = Quaternion.Euler(0f, yawAngle, 0f);
        Vector3 desiredOffset = rot * delta;
        Vector3 desiredPos = player.transform.position + desiredOffset;

        Vector3 pivot = player.transform.position + Vector3.up * delta.y;

        Vector3 dir = desiredPos - pivot;
        float dist = dir.magnitude;
        if (dist > 0.0001f) dir /= dist;

        Vector3 finalPos = desiredPos;

        if (Physics.SphereCast(pivot, cameraRadius, dir, out RaycastHit hit, dist, collisionMask, QueryTriggerInteraction.Ignore))
        {
            float safeDist = Mathf.Max(hit.distance - collisionBuffer, 0.05f);
            finalPos = pivot + dir * safeDist;
        }

        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref positionVelocity, smoothTime);

        Quaternion lookRotation = Quaternion.LookRotation(pivot - transform.position);
        Quaternion finalRotation = lookRotation * Quaternion.Euler(cameraRotationX, 0f, 0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, 10f * Time.deltaTime);
    }


    void OnEnable()
    {
        MenuController.ChangeAlcoholLevel += SetAlcoholLevel;
        MenuController.OnGameReset += ResetGame;
        PlayerController.OnPlayerKilled += PlayerKilled;
    }

    void OnDisable()
    {
        MenuController.ChangeAlcoholLevel -= SetAlcoholLevel;
        MenuController.OnGameReset -= ResetGame;
    }

    void PlayerKilled()
    {
        gamePaused = true;
    }

    void ResetGame()
    {
        gamePaused = false;
        transform.position = player.transform.position + delta;
        transform.rotation = Quaternion.Euler(cameraRotationX, player.transform.rotation.y, player.transform.rotation.z);
    }

    void SetAlcoholLevel(int newLevel)
    {
        // TODO
    }
}