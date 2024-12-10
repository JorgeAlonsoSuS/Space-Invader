using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Disparos : MonoBehaviour
{
    [SerializeField] public float velocidad;
    [SerializeField] private Vector3 direccion;

    private ObjectPool<Disparos> myPool;
    private float timer;

    public ObjectPool<Disparos> MyPool { get => myPool; set => myPool = value; }

    private void Awake()
    {
        
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);

        timer += Time.deltaTime;

        if(timer >= 4)
        {
            timer = 0;
            myPool.Release(this);
        }
    }


}
