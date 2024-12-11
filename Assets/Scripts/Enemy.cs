using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private GameObject bala;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject sp1;
    private Spawner spawner;
    [SerializeField] private int hp = 2;
    private AudioSource audioPlayer;
    [SerializeField] AudioClip hit;
    private EnemyBulletManager bulletManager;
    private bool isTrigger = false;

    private void Awake()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        audioPlayer = GameObject.Find("SFX").GetComponent<AudioSource>();
        bulletManager = GameObject.Find("Spawner").GetComponent<EnemyBulletManager>();
    }
    void Start()
    {
        StartCoroutine(SpawnBullets());
        StartCoroutine(GetReady());
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
    }



    private IEnumerator SpawnBullets()
    {   
        while(true)
        {
            //Instantiate(bala, sp1.transform.position, Quaternion.identity);
            Disparos copia = bulletManager.disparosEnemy.Get();
            copia.transform.position = sp1.transform.position;
            copia.gameObject.SetActive(true);
            yield return new WaitForSeconds(bulletSpeed);
        }

    }

    private IEnumerator GetReady()
    {
        yield return new WaitForSeconds(0.5f);
        isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger)
        {
            if (collision.gameObject.CompareTag("PBullet"))
            {
                Destroy(collision.gameObject);
                hp--;
                if (hp == 0)
                {
                    audioPlayer.PlayOneShot(hit);
                    Destroy(this.gameObject);
                }

            }

            if (collision.gameObject.CompareTag("Wall"))
            {
                spawner.ShipsCrashed += 1;
                Destroy(this.gameObject);
            }
        }
    }

    //public void ReduceBulletSpeed(float speed)
    //{      
    //        bulletSpeed += speed;
    //}

    public void SetBulletSpeed(float speed)
    {
        bulletSpeed = speed;
    }

    public void AddBulletSpeed(float speed)
    {
        if(bulletSpeed - speed > 0)
        {
            bulletSpeed -= speed;
        }
    }

    public void AddSpeed(float speed)
    {
        velocidad += speed;
    }

    public void SetSpeed(float speed)
    {
        velocidad = speed;
    }

    public void AddHP(int amount) 
    {
        hp += amount;
    }

    public void SetHP(int amount)
    {
        hp = amount;
    }
}
