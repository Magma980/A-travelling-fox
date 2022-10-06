using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public class Bound
    {
        public readonly float Start;
        public readonly float End;

        public Bound(int start, int end)
        {
            this.Start = start;
            this.End = end;
        }
    }
    enum FoxState
    {
        Start,
        Idle,
        Walking,
        Winner,
        Dead
    }

    private Animator playerAnim;

    private GameManager gameManager;

    private Collider foxCollider;

    public float speedForward = 3.0f;

    public float turnSpeed = 150.0f;

    private float horizontalInput;
    private float forwardInput;

    private float xRangeLow = -3;
    private float xRangeHigh = -75;
    public float zRangeLow = 23;
    public float zRangeHigh = 32;

    private int currentLevel = 0;
    private int totalLives = 1;

    internal readonly Bound[] levelBounds = new Bound[]
    {
        // bounds of the 1st level
        new Bound(-3, -75),
        // bounds of the 2nd level
        new Bound(-79, -160),

        new Bound(-164, -249),

        new Bound(-253, -335),

        new Bound(-339, -416),

        new Bound(-420, -435),
    };

    public ParticleSystem explosionParticle;

    private FoxState foxState = FoxState.Start;

    private TMP_Text livesText;

    public bool gameOver;

    private AudioSource playerAudio;
    public AudioClip crashSound;
    private bool hasPowerup;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerAnim = GetComponent<Animator>();
        foxCollider = GetComponent<Collider>();
        playerAudio = GetComponent<AudioSource>();

        livesText = GameObject.Find("LivesText").GetComponent<TMP_Text>();
        livesText.gameObject.SetActive(false);
    }

    public void SetFoxState()
    {
        foxState = FoxState.Idle;
    }

    private void Update()
    {
        if (FoxState.Start == foxState) return;
        //if (gameOver) return;
        if (FoxState.Dead == foxState) return;

        if (FoxState.Winner == foxState) return;

        var isWalkForward = Input.GetKeyDown("w") | Input.GetKeyDown(KeyCode.UpArrow);
        var isWalkBackward = Input.GetKeyDown("s") | Input.GetKeyDown(KeyCode.DownArrow);
        //var isWalkLeft = Input.GetKeyDown("a") | Input.GetKeyDown(KeyCode.LeftArrow);
        //var isWalkRight = Input.GetKeyDown("d") | Input.GetKeyDown(KeyCode.RightArrow);

        var isStopWalking = Input.GetKeyUp("w") | Input.GetKeyUp("s") |
            Input.GetKeyUp(KeyCode.UpArrow) | Input.GetKeyUp(KeyCode.DownArrow);

        if (isWalkForward)
        {
            gameManager.Level1.gameObject.SetActive(false);
            playerAnim.SetInteger("walkState", 1);
            foxState = FoxState.Walking;
            //isWalking = true;
        }

        else if (isWalkBackward)
        {
            playerAnim.SetInteger("walkState", 2);
            foxState = FoxState.Walking;
            //isWalking = true;
        }
        else if (isStopWalking && FoxState.Walking == foxState)//isWalking)
        {
            playerAnim.SetInteger("walkState", 0);
            //isWalking = false;
            foxState = FoxState.Idle;
        }

        // This is where we get player input
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // Move the vehicle forward
        transform.Translate(Time.deltaTime * speedForward * forwardInput * Vector3.forward);

        var center = foxCollider.bounds.center;
        transform.RotateAround(center, Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        // Keep the player in bounds X
        if (transform.position.x > xRangeLow)
        {
            transform.position = new Vector3(xRangeLow, transform.position.y, transform.position.z);
        }
        if (transform.position.x < xRangeHigh)
        {
            // we came to the end
            transform.position = new Vector3(xRangeHigh, transform.position.y, transform.position.z);
            foxState = FoxState.Winner;
            playerAnim.SetInteger("walkState", 5);
            showLevelText(currentLevel + 1);
            StartCoroutine(FoxWinnerWait());

            Debug.Log("WINNER !");
        }

        // Keep the player in bounds Z
        if (transform.position.z < zRangeLow)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeLow);
        }
        if (transform.position.z > zRangeHigh)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeHigh);
        }
    }

    IEnumerator FoxWinnerWait()
    {
        yield return new WaitForSeconds(1);

        UpdateLevelBounds();
        Debug.Log("FoxWinnerWait is done !");
        unShowLevelText(currentLevel);
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        return;
        /*/
        if (collision.gameObject.CompareTag("Cars"))
        //*/
        {
            // decease the number of lives
            --totalLives;
            if (totalLives > 0)
            {
                UpdateLivesIndicator();
            }
            Debug.Log("Game Over !");

            gameOver = true;
            foxState = FoxState.Dead;
            playerAnim.SetBool("Death_b", true);
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            gameManager.GameOver();
        }

        else if (collision.gameObject.CompareTag("Powerup") && hasPowerup)
        {
            Debug.Log("Eating powerup");
            hasPowerup = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            Debug.Log("Powerup trigger");

            // increase the number of lives
            ++totalLives;
            UpdateLivesIndicator();
        }
    }

    private void UpdateLivesIndicator()
    {
        livesText.text = $"Lives: {totalLives}";
    }

    void UpdateLevelBounds()
    {
        // increase the level count
        currentLevel++;
        // update the current start and end bounds
        if (currentLevel >= levelBounds.Length) return;

        xRangeLow = levelBounds[currentLevel].Start;
        xRangeHigh = levelBounds[currentLevel].End;

        transform.position = new Vector3(xRangeLow, transform.position.y, transform.position.z);

        playerAnim.SetInteger("walkState", 0);
        foxState = FoxState.Idle;
    }

    void showLevelText(int n)
    {
        if (n == 1)
        {
            gameManager.Level2.gameObject.SetActive(true);

        }
        else if (n == 2)
        {
            gameManager.Level3.gameObject.SetActive(true);

        }
        else if (n == 3)
        {
            gameManager.Level4.gameObject.SetActive(true);

        }
        else if (n == 4)
        {
            gameManager.Level5.gameObject.SetActive(true);

        }
        else
        {
            gameManager.Level6.gameObject.SetActive(true);
        }
    }

    void unShowLevelText(int n)
    {
        Debug.Log(currentLevel);
        if (n == 1)
        {
            gameManager.Level2.gameObject.SetActive(false);

        }
        else if (n == 2)
        {
            gameManager.Level3.gameObject.SetActive(false);

        }
        else if (n == 3)
        {
            gameManager.Level4.gameObject.SetActive(false);

        }
        else if (n == 4)
        {
            gameManager.Level5.gameObject.SetActive(false);

        }
        else
        {
            gameManager.Level6.gameObject.SetActive(false);
            gameManager.restartButton.gameObject.SetActive(true);
        }
    }
}
