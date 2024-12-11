using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float ratioDisparo;
    [SerializeField] private Disparos bullet;
    [SerializeField] private GameObject[] spawnpoints;
    [SerializeField] private GameObject[] lives;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip damage;
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject layout;
    private int bulletNum = 2; 
    private float temporizador = 0.5f;
    private int hp = 6;
    private Disparos balasero;
    private bool canTrigger = true;
    private float bulletSpeed = 0;
    public ObjectPool<Disparos> pool;

    public int BulletNum { get => bulletNum; set => bulletNum = value; }

    private void Awake()
    {
        pool = new ObjectPool<Disparos>(CrearDisparo, null, ReleaseDisparo, DestroyDisparo);
    }

    private void DestroyDisparo(Disparos disparo)
    {
        Destroy(disparo);
    }

    private void ReleaseDisparo(Disparos disparo)
    {
        disparo.gameObject.SetActive(false);
    }

    private void GetDisparo(Disparos disparo)
    {
        throw new NotImplementedException();
    }

    private Disparos CrearDisparo()
    {
        Disparos disparoCopia = Instantiate(bullet, transform.position, Quaternion.identity);
        disparoCopia.MyPool = pool;
        return disparoCopia;
    }

    void Start()
    {
        balasero = bullet.GetComponent<Disparos>();
    }

    // Update is called once per frame
    void Update()
    {

        Movimiento();
        DelimitarMovimiento();
        Disparar();
        
    }

    void Movimiento()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(inputH, inputV).normalized * velocidad * Time.deltaTime);
    }

    void DelimitarMovimiento()
    {
        float xClamped = Mathf.Clamp(transform.position.x, -8f, 8f);
        float yClamped = Mathf.Clamp(transform.position.y, -4.5f, 4f);
        transform.position = new Vector3(xClamped, yClamped, 0);
    }

    void Disparar()
    {
        temporizador += 1 * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && temporizador > ratioDisparo) 
        {   
            //Instantiate(bullet, spawnpoints[0].transform.position, Quaternion.identity);
            //Instantiate(bullet, spawnpoints[1].transform.position, Quaternion.identity);
            //if (triple) { Instantiate(bullet, spawnpoints[2].transform.position, Quaternion.identity); }
            temporizador = 0;
            audioPlayer.PlayOneShot(shoot);

            for(int i = 0; i < bulletNum; i++)
            {
                Disparos disparoCopia =  pool.Get();
                disparoCopia.gameObject.SetActive(true);
                disparoCopia.velocidad += bulletSpeed;
                disparoCopia.transform.position = spawnpoints[i].transform.position;
                if(bulletNum > 2)
                {
                    if(i==0) { disparoCopia.transform.rotation = Quaternion.Euler(0,0,30f); }
                    else if(i==1) { disparoCopia.transform.rotation = Quaternion.Euler(0, 0, -30f); }
                    else if(i==2) { disparoCopia.transform.rotation = Quaternion.Euler(0, 0, 0f); }
                    else if(i==3) { disparoCopia.transform.rotation = Quaternion.Euler(0, 0, 75f); }
                    else if (i == 4) { disparoCopia.transform.rotation = Quaternion.Euler(0, 0, -75f); }

                }

            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger)
        {
            if (collision.gameObject.CompareTag("EBullet") || collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
                lives[hp - 1].gameObject.GetComponent<Image>().enabled = false;
                hp--;
                audioPlayer.PlayOneShot(damage);
                StartCoroutine(TriggerCooldown());
            }

            if (collision.gameObject.CompareTag("BBullet"))
            {
                Destroy(collision.gameObject);
                lives[hp - 1].gameObject.GetComponent<Image>().enabled = false;
                lives[hp - 2].gameObject.GetComponent<Image>().enabled = false;
                hp -= 2; ;
                audioPlayer.PlayOneShot(damage);
                StartCoroutine(TriggerCooldown());
            }
            else if (collision.gameObject.CompareTag("Boss"))
            {
                lives[hp - 1].gameObject.GetComponent<Image>().enabled = false;
                lives[hp - 2].gameObject.GetComponent<Image>().enabled = false;
                hp -= 2; ;
                audioPlayer.PlayOneShot(damage);
                StartCoroutine(TriggerCooldown());
            }
        }

        if (hp == 0)
        {
            gameObject.SetActive(false);
            spawner.SetActive(false);
            layout.SetActive(true);
        }
    }

    private IEnumerator TriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(1f);
        canTrigger = true;
    }

    public void AddBulletSpeed(float speed)
    {
        bulletSpeed += speed;
    }

    public void SetBulletSpeed(float speed)
    {
        balasero.velocidad = speed;
    }

    public void AddPlayerSpeed(float speed)
    {
        velocidad += speed;
    }

    public void AddRatio(float speed)
    {
        ratioDisparo -= speed;
    }
}
