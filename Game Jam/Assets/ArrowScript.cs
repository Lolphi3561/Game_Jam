using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int arrowSpeed = 50;
    public Rigidbody2D myRigidBody;
    public Transform crossbow;
    private SpriteRenderer crossbowSR;
    private static int direction = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crossbow = GameObject.FindGameObjectWithTag("Crossbow").transform;
        crossbowSR = crossbow.GetComponent<SpriteRenderer>();
        if (crossbowSR.flipY == true)
        {
            direction = -1;
        }
        myRigidBody.linearVelocityX = arrowSpeed * direction;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
