using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int arrowSpeed = 50;
    private Transform crossbow;
    private static int direction = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crossbow = GameObject.FindGameObjectWithTag("Crossbow").transform;
        if (crossbow.localScale.y == -1)
        {
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += crossbow.right * arrowSpeed * direction * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
