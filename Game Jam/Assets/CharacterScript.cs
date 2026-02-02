using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class CharacterScript : MonoBehaviour
{
    // Allgemien
    public Rigidbody2D myRigidBody;
    public bool isAlive = true;
    public GameObject floorDeathBox;
    public Collision2D collision;
    private bool lastDirectionLeft = true;

    // Springen
    private int jumpsLeft = 0;
    private bool isGrounded = false;
    public float jumpPower = 10;

    // WallCling
    public PhysicsMaterial2D NoFriction;
    public PhysicsMaterial2D WallFriction;
    private Collider2D col;
    public bool isOnWall = false;
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 10f;
    private int wallDir = 0;



    // Dash
    public bool hasDash = true;
    public float dashCooldown = 0.75f;
    public float dashTimer = 0;
    public float dashBoost = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider2D>();
        col.sharedMaterial = NoFriction;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        Dash();
    }

    private void Movement()
    {
        // Totesfall
        if (isAlive == false)
        {
            transform.position = new Vector3(0, 8, 1);
            isAlive = true;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpsLeft > 0)
        {
            if (isOnWall)
            {
                myRigidBody.linearVelocity = new Vector2(
                    wallJumpForceX * wallDir,
                    wallJumpForceY
                );

                isOnWall = false;
                col.sharedMaterial = NoFriction;
            }
            else
            {
                myRigidBody.linearVelocityY = jumpPower;
            }
            jumpsLeft--;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            if (myRigidBody.linearVelocityX > -5)
            {
                myRigidBody.linearVelocityX = -5;
            }
            lastDirectionLeft = true;

        }
        else if (Keyboard.current.dKey.isPressed)
        {
            if (myRigidBody.linearVelocityX < 5)
            {
                myRigidBody.linearVelocityX = 5;
            }
            lastDirectionLeft = false;
        }

        if (lastDirectionLeft && !Keyboard.current.aKey.isPressed || myRigidBody.linearVelocityX < -5)
        {
            myRigidBody.linearVelocityX += (float)0.5;
            if (myRigidBody.linearVelocityX > 0)
            {
                myRigidBody.linearVelocityX = 0;
            }
        }
        else if (!lastDirectionLeft && !Keyboard.current.dKey.isPressed || myRigidBody.linearVelocityX > 5)
        {
            myRigidBody.linearVelocityX -= (float)0.5;
            if (myRigidBody.linearVelocityX < 0)
            {
                myRigidBody.linearVelocityX = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGround(collision);

        if (collision.gameObject.CompareTag("DeathArea"))
        {
            isAlive = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            myRigidBody.linearVelocity = new Vector2(0, 0);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(isOnWall && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            myRigidBody.linearVelocityX = 20;
        }
        WallCling(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isOnWall)
        {
            isOnWall = false;
            col.sharedMaterial = NoFriction;
        }

        if (isGrounded)
        {
            isGrounded = false;
            jumpsLeft = 1;
        }
    }

    private void CheckGround(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpsLeft = 1;
                return;
            }
        }
    }


    private void Dash()
    {
        if (dashTimer < dashCooldown)
        {
            dashTimer += Time.deltaTime;
        }

        if (dashTimer >= dashCooldown && Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            if (lastDirectionLeft)
            {
                myRigidBody.linearVelocityX -= dashBoost;
            }
            else
            {
                myRigidBody.linearVelocityX += dashBoost;
            }
            dashTimer = 0;
        }
    }

    private void WallCling(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f && contact.normal.y < 0.2f)
            {
                isOnWall = true;
                col.sharedMaterial = WallFriction;
                jumpsLeft = 1;

                if(contact.normal.x > 0)
                {
                    wallDir = 1;
                }
                else
                {
                    wallDir = -1;
                }
            }
        }
    }
}
