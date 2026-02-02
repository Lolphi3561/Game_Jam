using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class CharacterScript : MonoBehaviour
{
    // Allgemien
    public Rigidbody2D myRigidBody;
    public float jumpPower = 10;
    public int jumpCount = 2;
    public bool isAlive = true;
    public GameObject floorDeathBox;
    public Collision2D collision;

    private bool lastDirectionLeft = true;

    // WallCling
    public PhysicsMaterial2D NoFriction;
    public PhysicsMaterial2D WallFriction;
    private Collider2D col;


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

        WallCling();
    }

    private void Movement()
    {
        // Totesfall
        if (isAlive == false)
        {
            transform.position = new Vector3(0, 8, 1);
            isAlive = true;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpCount >= 1)
        {
            myRigidBody.linearVelocityY = jumpPower;
            jumpCount -= 1;
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
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                jumpCount = 2;
                break;
            }
        }

        if (collision.gameObject.CompareTag("DeathArea"))
        {
            isAlive = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            myRigidBody.linearVelocityX = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(Keyboard.current.spaceKey.isPressed == false)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    jumpCount -= 1;
                    break;
                }
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
            if (contact.normal.y == 0f)
            {
                col.sharedMaterial = WallFriction;
                jumpCount = 1;
                break;
            }
        }
    }
}
