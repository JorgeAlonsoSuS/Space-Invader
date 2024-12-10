using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;
    private Player playerScript;
    private Enemy enemyScript;
    [SerializeField] private TMP_Text texto;
    private bool finishedGambling = false;
    private int diceResult;
    [SerializeField] private GameObject dice;

    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioClip upgrade;
    [SerializeField] AudioClip gambling;
    [SerializeField] AudioClip notUpgrade;
    [SerializeField] AudioClip diceAppear;
    [SerializeField] int bossMultiplyer = 5;

    [SerializeField] private int oleadas = 3;

    //Modificadores involucrados en el dado
    private float enemyBulletSpeed;
    private float enemySpeed;
    private float enemyHP;
    private float playerSpeed;
    private float playerBulletSpeed;
    private float bossHP;

    private int enemigosPWave = 10;

    private int shipsCrashed = 0;

    public int ShipsCrashed { get => shipsCrashed; set => shipsCrashed = value; }


    void Start()
    {
        playerScript = player.GetComponent<Player>();
        enemyScript = enemy.GetComponent<Enemy>();
        StartCoroutine(SpawnEnemies());
    }


    IEnumerator SpawnEnemies()
    {   

        for(int i = 0; i< oleadas; i++)//Oleadas
        {
            Debug.Log("Oleada" + (i+1));
            texto.SetText("Here comes an enemy wave!");
            enemigosPWave += 5;
            yield return new WaitForSeconds(2f);
            texto.SetText("");
            for(int j = 0; j < enemigosPWave; j++)//enemigos
            {
                Vector3 puntoAleatorio = new Vector3(transform.position.x, Random.Range(-4f, 4f), transform.position.z);
                Instantiate(enemy, puntoAleatorio, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }

            yield return new WaitForSeconds(3f + i*2); //Para controlar que el dado parzca sin enemigos
            finishedGambling = false;
            StartCoroutine(Gambling());
            yield return new WaitUntil(() => finishedGambling);
            

            
        }
        yield return new WaitForSeconds(2f);
        texto.SetText("Uh Oh... Here comes a bossfight");
        yield return new WaitForSeconds(2f);
        texto.SetText("");
        boss.GetComponent<Boss>().AddHP(shipsCrashed*bossMultiplyer);
        Instantiate(boss, transform.position, Quaternion.identity);
        
    }

    IEnumerator Gambling()
    {
        dice.SetActive(true);
        audioPlayer.PlayOneShot(diceAppear);
        dice.GetComponent<Button>().enabled = true;


        yield return new WaitForSeconds(5.5f);
        ApplyEffect(diceResult);
        yield return new WaitForSeconds(2f);
        texto.SetText("");
        finishedGambling = true;

        dice.SetActive(false);
    }

    public bool GetFinished()
    {
        return finishedGambling;
    }

    public void SetResult(int result)
    {
        diceResult = result;
    }

    private void ApplyEffect(int result)
    {
        audioPlayer.volume = 1f;
        switch (result)
        {
            case 0:
                Debug.Log("Wait what?...");
                texto.SetText("Wait what?...");
                break;

            case 1:
                Debug.Log("Te mejoro el disparo");
                texto.SetText("Faster bullets for you!");
                playerScript.AddBulletSpeed(5f);
                playerScript.AddRatio(0.2f);
                playerScript.pool.Clear();
                audioPlayer.PlayOneShot(upgrade);
                break;

            case 2:
                Debug.Log("Te mueves mas rapido");
                texto.SetText("Speed Speed Speeed!");
                playerScript.AddPlayerSpeed(5f);
                audioPlayer.PlayOneShot(upgrade);
                break;

            case 3:
                Debug.Log("Triple disparo!");
                if (playerScript.BulletNum < 3) { texto.SetText("Triple Shot???"); }
                else if (playerScript.BulletNum == 3) { texto.SetText("Quintuple Shot???"); }
                //playerScript.BulletNum += 1;
                if (playerScript.BulletNum < 3) { playerScript.BulletNum = 3; }
                else if(playerScript.BulletNum == 3) { playerScript.BulletNum = 5; }
                audioPlayer.PlayOneShot(upgrade);
                break;

            case 4:
                Debug.Log("Bum bum, esta ronda los enemigos no disparan");
                texto.SetText("Enemies have less ammo!!");
                enemyScript.ReduceBulletSpeed(1.5f);
                audioPlayer.PlayOneShot(upgrade);
                break;

            case 5:
                Debug.Log("TODO LO ANTERIOR");
                texto.SetText("YOU JUST GOT EVERY UPDTE POSSIBLE");
                playerScript.pool.Clear();
                playerScript.AddBulletSpeed(5f);
                playerScript.AddPlayerSpeed(5f);
                playerScript.BulletNum += 1;
                if (playerScript.BulletNum < 3) { playerScript.BulletNum = 3; }
                else if (playerScript.BulletNum == 3) { playerScript.BulletNum = 5; }
                enemyScript.ReduceBulletSpeed(1.5f);
                audioPlayer.PlayOneShot(upgrade);

                break;

            case 6:
                Debug.Log("oh oh... el juego es mas dif�cil");
                texto.SetText("Uh oh... The game just got harder...");
                enemigosPWave += 7;
                enemyScript.AddBulletSpeed(1f);
                enemyScript.AddSpeed(2.5f);
                enemyScript.AddHP(1);
                audioPlayer.PlayOneShot(notUpgrade);
                break;

        }
    }

    private void OnDestroy()
    {   
        //Antes de buildear quitar
        enemyScript.SetBulletSpeed(4f);
        enemyScript.SetSpeed(3f);
        enemyScript.SetHP(2);
        playerScript.SetBulletSpeed(8f);
        boss.GetComponent<Boss>().SetHp(20);
    }
}