using UnityEngine;

public class CrossbowScript : MonoBehaviour
{

    public GameObject Arrow;
    public float spawnRate = 2;
    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnArrow();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnArrow();
            timer = 0;
        }
    }

    void spawnArrow()
    {
        Instantiate(Arrow, transform.position, Arrow.transform.rotation);
    }
}
