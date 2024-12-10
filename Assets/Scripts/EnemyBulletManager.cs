using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletManager : MonoBehaviour
{
    public ObjectPool<Disparos> disparosEnemy;
    [SerializeField] private Disparos bala;

    private void Awake()
    {
        disparosEnemy = new ObjectPool<Disparos>(CrearDisparo, null, ReleaseDisparo, DestroyDisparo);
    }

    private void DestroyDisparo(Disparos disparo)
    {
        Destroy(disparo);
    }

    private void ReleaseDisparo(Disparos disparo)
    {
        disparo.gameObject.SetActive(false);
    }

    private Disparos CrearDisparo()
    {
        Disparos disparoCopia = Instantiate(bala, transform.position, Quaternion.identity);
        disparoCopia.MyPool = disparosEnemy;
        return disparoCopia;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
