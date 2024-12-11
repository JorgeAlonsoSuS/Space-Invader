using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    // Array of dice sides sprites to load from Resources folder
    [SerializeField]
    private Sprite[] diceSides;

   // [SerializeField]
    //private GameObject manager;

    [SerializeField]
    private Spawner spawner;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer rend;

    private Image imagen;

    private Button boton;

    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip gambling;

	// Use this for initialization
	private void Start () {

        // Assign Renderer component
        rend = GetComponent<SpriteRenderer>();
        imagen = GetComponent<Image>();
        boton = GetComponent<Button>();
        //spawner = manager.GetComponent<Spawner>();
        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        //diceSides = Resources.LoadAll<Sprite>("DiceSides/");
	}
	
    // If you left click over the dice then RollTheDice coroutine is started
    private void OnMouseUp()
    {
        StartCoroutine("RollTheDice");

    }

    public void StartRoll()
    {
        StartCoroutine(RollTheDiceCanvas());
        audioPlayer.PlayOneShot(gambling);

    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log(finalSide);
    }

    private IEnumerator RollTheDiceCanvas()
    {
        boton.enabled = false;
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            imagen.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        spawner.SetResult(finalSide);

        // Show final dice value in Console
        //Debug.Log(finalSide);
    }
}
