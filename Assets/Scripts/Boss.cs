using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private GameObject bala;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject sp1;
    private Spawner spawner;
    [SerializeField] private int hp = 20;
    private int direction = 1;
    private GameObject player;
    private GameObject canvas;
    private GameObject layout;


    private void Awake()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas");
        layout = canvas.transform.Find("Layut").gameObject;
        if (layout == null)
        {
            Debug.LogError("No se encontró el GameObject hijo con el nombre especificado.");
        }

    }

    void Start()
    {
        StartCoroutine(SpawnBullets());
    }

    // Update is called once per frame
    void Update()
    {
        
        Movement();
    }

    IEnumerator SpawnBullets()
    {
        while (true)
        {
            Instantiate(bala, sp1.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(bulletSpeed);
        }

    }

    private void Movement()
    {
        if(transform.position.y >= 4)
        {
            direction = -1;
        }
        else if(transform.position.y <= -4)
        {
            direction = 1;
        }

        transform.Translate(new Vector2(0, direction).normalized * velocidad * Time.deltaTime);
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);

    }

    public void AddHP(int added)
    {
        hp += added;
    }

    public void SetHp(int sethp)
    {
        hp = sethp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PBullet"))
        {
            Destroy(collision.gameObject);
            hp--;
            if (hp == 0)
            {
                Destroy(this.gameObject);
                layout.SetActive(true);
                TMP_Text victory = layout.transform.Find("Victory").GetComponent<TMP_Text>();
                victory.text = "VICTORY!";
                victory.color = Color.green;
            }

        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            player.SetActive(false);
            spawner.gameObject.SetActive(false);
            layout.SetActive(true);


        }
    }
}
