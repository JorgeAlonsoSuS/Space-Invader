using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Spawner spawner;
    private void Awake()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //spawner.ShipsCrashed += 1;
        }
    }
}
