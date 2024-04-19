using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HolderControllerScript : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject holder_1;
    public GameObject holder_2;
    public GameObject[] cratePrefabs;
    public float holderReleaseSpeed = 10f;
    public bool isReleased = false;
    private float holder1XBoundary = 6f;
    private float holder2XBoundary = 0f;
    private int crateIndex = 0;
    private GameObject spawnedCrates;
    private float mainHolderMoveSpeed = 1;
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;
    public GameObject player1TurnIndicator;
    public GameObject player2TurnIndicator;
    public TextMeshProUGUI player1WonText;
    public TextMeshProUGUI player2WonText;
    public Button restartGameButton;
    public Button exitToMenuButton;
    public TextMeshProUGUI countDownText;
    private float countdownTimer = 5f;
    private bool isTimerActive = false;
    public AudioSource audioSource;
    public AudioClip timerSound;

    // Start is called before the first frame update
    void Start()
    {
        CrateInstantiate();
    }

    // Update is called once per frame
    void Update()
    {
        MainHolderMovement();
        HolderRelease();
        HolderXBoundary();

        if (isTimerActive)
        {
            countDownText.text = "" + Mathf.RoundToInt(countdownTimer);
            countdownTimer -= Time.deltaTime;

            if (countdownTimer <= 0f)
            {
                // Timer expired, move holder and release crate
                MoveHolderAndReleaseCrate();
                audioSource.PlayOneShot(timerSound, 1f);
            }
        }

        if (gridManager.CheckForWinCondition() != null)
        {
            isTimerActive = false;
        }
    }

    //Handles the movement of the main holder left or right.
    public void MainHolderMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > 0 && !isReleased && gridManager.CheckForWinCondition() == null)
        {
            transform.Translate(Vector3.left * mainHolderMoveSpeed);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < (gridManager.gridSizeX - 1) * gridManager.cellSize
                    && !isReleased && gridManager.CheckForWinCondition() == null)
        {
            transform.Translate(Vector3.right * mainHolderMoveSpeed);
        }
    }

    //Releases the crates from the holders when Space key is pressed.
    public void HolderRelease()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isReleased && gridManager.CheckForWinCondition() == null)
        {
            if (!CheckStackedCrateLimit())
            {
                isReleased = true;

                if (spawnedCrates != null)
                {
                    spawnedCrates.transform.parent = null;
                }
            }

        }

        else if (isReleased)
        {
            holder_1.transform.Translate(Vector3.right * holderReleaseSpeed * Time.deltaTime, Space.World);
            holder_2.transform.Translate(Vector3.left * holderReleaseSpeed * Time.deltaTime, Space.World);
        }
    }

    //Ensures the holders stay within predefined boundaries.
    void HolderXBoundary()
    {
        if (holder_1.transform.position.x > holder1XBoundary)
        {
            holder_1.transform.position = new Vector3(holder1XBoundary, holder_1.transform.position.y, holder_1.transform.position.z);
        }

        if (holder_2.transform.position.x < holder2XBoundary)
        {
            holder_2.transform.position = new Vector3(holder2XBoundary, holder_2.transform.position.y, holder_2.transform.position.z);
        }
    }

    //Instantiates crates onto the holders alternately.
    public void CrateInstantiate()
    {
        if (crateIndex >= cratePrefabs.Length)
        {
            crateIndex = 0;
        }

        string crateTag = cratePrefabs[crateIndex].tag;
        Vector3 spawnPosition = transform.position;
        spawnedCrates = Instantiate(cratePrefabs[crateIndex], spawnPosition, Quaternion.Euler(0, 0, 180));
        spawnedCrates.transform.parent = transform;

        StartTimer();

        if (crateTag == "Yellow Crate")
        {
            HighLightPlayer1Text();
        }

        else if (crateTag == "Red Crate")
        {
            HighlightPlayer2Text();
        }

        Debug.Log(crateTag + " instantiated");

        crateIndex++;
    }

    public void StartTimer()
    {
        countdownTimer = 5f; // Reset timer
        isTimerActive = true;
    }

    private void MoveHolderAndReleaseCrate()
    {
        // Reset timer and flag
        isTimerActive = false;

        // Move holder to a random position
        MoveHolderToRandomPosition();

        // Release the crate
        AutoReleaseCrate();
    }

    private void MoveHolderToRandomPosition()
    {
        if (gridManager.CheckForWinCondition() == null)
        {
            // Calculate random position within valid boundaries
            float randomX = Random.Range(holder2XBoundary, holder1XBoundary);

            // Calculate the nearest grid cell position
            float nearestGridX = Mathf.Round(randomX / gridManager.cellSize) * gridManager.cellSize;

            // Move holder to the nearest grid cell position
            Vector3 randomPosition = new Vector3(nearestGridX, transform.position.y, transform.position.z);
            transform.position = randomPosition;
        }

    }

    private void AutoReleaseCrate()
    {
        // Check if holder has room to release a crate
        if (!CheckStackedCrateLimit() && gridManager.CheckForWinCondition() == null)
        {
            // Release the crate
            isReleased = true;
            if (spawnedCrates != null)
            {
                spawnedCrates.transform.parent = null;
            }
        }
        // If no room, move holder again and release another crate
        else
        {
            MoveHolderAndReleaseCrate();
        }
    }

    private void HighLightPlayer1Text()
    {
        player1Text.color = Color.yellow;
        player2Text.color = Color.white;
        player1TurnIndicator.SetActive(true);
        player2TurnIndicator.SetActive(false);
    }

    private void HighlightPlayer2Text()
    {
        player1Text.color = Color.white;
        player2Text.color = Color.red;
        player1TurnIndicator.SetActive(false);
        player2TurnIndicator.SetActive(true);
    }

    //Resets the position of the main holder.
    public void ResetMainHolderPosition()
    {
        transform.position = new Vector3(3, 8, 0);
    }

    //Resets the position of the sub holders.
    public void ResetSubHoldersPosition()
    {
        holder_1.transform.position = new Vector3(-1.5f, 1, 0);
        holder_2.transform.position = new Vector3(7.5f, 1, 0);
    }

    //Checks if the stacked crate limit is reached.
    private bool CheckStackedCrateLimit()
    {
        // Cast a ray downwards from the mainHolder position to check for stacked crates
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down);

        // Count the number of crates hit by the ray
        int stackedCrateCount = 0;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Yellow Crate") || hit.collider.CompareTag("Red Crate"))
            {
                stackedCrateCount++;
                if (stackedCrateCount >= 6)
                {
                    return true; // Return true if six or more crates are stacked
                }
            }
        }

        return false; // Return false if less than six crates are stacked
    }

    //Displays the winner's text based on the provided winner.
    public void DisplayWinnerText(string winner)
    {
        if (winner == "Yellow Crate")
        {
            player1WonText.gameObject.SetActive(true);
            restartGameButton.gameObject.SetActive(true);
            exitToMenuButton.gameObject.SetActive(true);
        }

        else if (winner == "Red Crate")
        {
            player2WonText.gameObject.SetActive(true);
            restartGameButton.gameObject.SetActive(true);
            exitToMenuButton.gameObject.SetActive(true);
        }
    }
}
