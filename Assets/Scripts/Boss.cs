using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Boss : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public string sceneToLoadOnDeath = "RhythmLevel1";
    public GameObject nextScene;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;

    public enum WalkalbeDirection { Right, Left }

    private WalkalbeDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    private void Start()
    {
        nextScene.SetActive(false);
    }
    public WalkalbeDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkalbeDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkalbeDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }

            }

            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }

    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        if (damageable.Health <= 0)
        {
            nextScene.SetActive(true);
        }

    }


    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded)


                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),
                    rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }

    }


    private void FlipDirection()
    {
        if (WalkDirection == WalkalbeDirection.Right)
        {
            WalkDirection = WalkalbeDirection.Left;
        }
        else if (WalkDirection == WalkalbeDirection.Left)
        {
            WalkDirection = WalkalbeDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDectected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }

    private void Die()
    {
        Debug.Log("Boss is dying...");
    
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        Debug.Log("Starting scene load coroutine...");
        yield return new WaitForSeconds(3f);
        Debug.Log("Attempting to load scene: " + sceneToLoadOnDeath);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadOnDeath);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }



}