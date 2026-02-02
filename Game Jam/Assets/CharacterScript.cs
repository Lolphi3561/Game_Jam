using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float jumpPower = 5;
    public int jumpCount = 2;
    public bool isAlive = true;
    public Collider2D collisionCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == false)
        {
            return;
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpCount >= 1)
        {
            myRigidBody.linearVelocity = Vector2.up * jumpPower;
            jumpCount -= 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            myRigidBody.linearVelocityX = -5;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            myRigidBody.linearVelocityX = 5;
        }
    }

    public bool DeathCauses()
    {
        if(gameObject.)

        return isAlive;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpCount = 2;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(Keyboard.current.spaceKey.isPressed == false)
        {
            jumpCount -= 1;
        }
    }
}
