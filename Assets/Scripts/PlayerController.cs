using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player components
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    // Jump variables
    float jumpPower;
    bool isGrounded;
    bool isCharging;
    bool isMaxed;
    public int maxJumpPower = 10;
    public float jumpChargeRate = 10;

    // GUI indicators
    Vector2 initDragPosition;
    Vector2 finalDragPosition;
    public GameObject initMarker;
    public GameObject finalMarker;

    // Particle effects
    public ParticleSystem sparkEffect;
    public ParticleSystem collideEffect;

    // Sound effects
    public AudioClip sfxCollide;
    public AudioClip sfxCharge;
    public AudioClip sfxMax;
    public AudioClip sfxJump;

    void Awake()
    {
        // Assign components on awake
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Disable GUI indicators on start
        initMarker.gameObject.SetActive(false);
        finalMarker.gameObject.SetActive(false);
    }

    void Update()
    {
        // Touch input handling
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (isGrounded && !isCharging)
                    {
                        initDragPosition = touch.position;
                        PlaceMarkers(initDragPosition);
                        StartCharge();
                    }
                    break;
                case TouchPhase.Ended:
                    if (isCharging)
                    {
                        finalDragPosition = touch.position;
                        Jump();
                        ResetCharge();
                    }
                    break;
            }
        }
        // End of touch input handling
        
        // Keyboard + Mouse input handling
        if (Input.GetMouseButtonDown(0))
        {
            if (isGrounded && !isCharging)
            {
                Vector3 mousePosition = Input.mousePosition;
                initDragPosition = new Vector2(mousePosition.x, mousePosition.y);
                PlaceMarkers(initDragPosition);
                StartCharge();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isCharging)
            {
                Vector3 mousePosition = Input.mousePosition;
                finalDragPosition = new Vector2(mousePosition.x, mousePosition.y);
            
                Jump();
                ResetCharge();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.RestartLevel();
        }
        // End of Keyboard + Mouse input handling

        if (isCharging) {
            Charge();
        }

        UpdateColor();
        CheckGrounded();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Create and play particles and sfx
        ParticleSystem effectClone = Instantiate(collideEffect);
        var main = effectClone.main;

        effectClone.transform.position = collision.GetContact(0).point;
        effectClone.transform.forward = collision.GetContact(0).normal;
        main.startSpeedMultiplier = Mathf.Abs(collision.relativeVelocity.magnitude);
        
        effectClone.Play();
        Destroy(effectClone.gameObject, effectClone.main.duration);

        PlayAudioClip(sfxCollide);
    }

    void CheckGrounded()
    {
        // Simple velocity check for determining if grounded
        isGrounded = rb.velocity.magnitude < .1f;
    }

    void ResetCharge()
    {
        jumpPower = 0;
        isCharging = false;
        isMaxed = false;
        initMarker.gameObject.SetActive(false);
        finalMarker.gameObject.SetActive(false);
    }

    void StartCharge()
    {
        isCharging = true;
        PlayAudioClip(sfxCharge);
    }

    void Charge()
    {
        // Increase jumpPower until greater than or equal to maxJumpPower
        if (jumpPower < maxJumpPower)
        {
            jumpPower += Time.deltaTime * jumpChargeRate;
        }
        else
        {
            MaxCharge();
        }

        // Position marker between intial drag point and current touch/mouse position
        Vector3 markerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        markerPosition = new Vector3(markerPosition.x, markerPosition.y, -1);
        finalMarker.transform.position = initMarker.transform.position + ((markerPosition - initMarker.transform.position) / 2);
    }

    void MaxCharge()
    {
        if (!isMaxed) {
            isMaxed = true;
            jumpPower = maxJumpPower;
            sparkEffect.transform.position = transform.position;
            sparkEffect.Play();

            PlayAudioClip(sfxMax);
        }
    }

    void Jump()
    {
        // Apply impulse force
        rb.AddForce((initDragPosition - finalDragPosition).normalized * jumpPower, ForceMode2D.Impulse);

        // Create and play particles and sfx
        ParticleSystem effectClone = Instantiate(collideEffect);
        var main = effectClone.main;

        effectClone.transform.position = transform.position + (Vector3.down * (transform.localScale.y / 2));
        effectClone.transform.forward = Vector3.up;
        main.startSpeedMultiplier = jumpPower;
        
        effectClone.Play();
        Destroy(effectClone.gameObject, effectClone.main.duration);

        PlayAudioClip(sfxJump);
    }

    void UpdateColor()
    {
        Color spriteColor = spriteRenderer.color;

        if (!isGrounded)
        {
            // Dull color indicates player cannot jump
            spriteColor = Color.gray;
        }
        else
        {
            // Sprite turns red while charging, yellow when max charge
            if (jumpPower == maxJumpPower)
                spriteColor.g = 1;
            else
                spriteColor.g = 1 - (jumpPower / maxJumpPower);
            spriteColor.b = 1 - (jumpPower / maxJumpPower);
        }
        
        spriteRenderer.material.color = spriteColor;
    }

    void PlaceMarkers(Vector3 inputPosition)
    {
        // Places and enables markers at inputPosition in world space
        Vector3 markerPosition = Camera.main.ScreenToWorldPoint(inputPosition);
        markerPosition = new Vector3(markerPosition.x, markerPosition.y, -1);
        initMarker.transform.position = markerPosition;
        finalMarker.transform.position = markerPosition;

        initMarker.gameObject.SetActive(true);
        finalMarker.gameObject.SetActive(true);
    }

    void PlayAudioClip(AudioClip clip)
    {
        // Retrieve game volume before playing audio clip
        audioSource.volume = AudioManager.Instance.GetVolume();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
