using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    private HolderControllerScript holderControllerScript;
    private bool crateInstantiated = false;
    private GridManager gridManager;
    private AudioSource audioSource;
    public AudioClip crateDropSound;
    public AudioClip winSound;

    // Start is called before the first frame update
    void Start()
    {
        holderControllerScript = GameObject.Find("Main Holder").GetComponent<HolderControllerScript>();
        gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Handles collision events for crates, triggers instantiation of new crates, and checks for win conditions.
    private void OnCollisionEnter(Collision collision)
    {
        if (!crateInstantiated && (collision.collider.CompareTag("Ground") ||
        collision.collider.CompareTag("Yellow Crate") ||
        collision.collider.CompareTag("Red Crate")))
        {
            holderControllerScript.CrateInstantiate();
            holderControllerScript.isReleased = false;
            holderControllerScript.ResetMainHolderPosition();
            holderControllerScript.ResetSubHoldersPosition();
            crateInstantiated = true;
            audioSource.PlayOneShot(crateDropSound, 1f);
            //crateRB.constraints = RigidbodyConstraints.FreezeAll;
            // Get the position in grid coordinates
            Vector3Int gridPosition = GetGridPosition(transform.position);
            // Add the crate to the gridArray
            gridManager.AddCrateToGrid(this.gameObject, gridPosition);
            gridManager.CheckGridStatus();

            if (gridManager.CheckForWinCondition() != null)
            {
                string winningCrateTag = gridManager.CheckForWinCondition();
                Debug.Log(winningCrateTag + " Won!");

                // Perform actions for winning condition here (e.g., display win message, end the game)

                holderControllerScript.DisplayWinnerText(winningCrateTag);
                audioSource.PlayOneShot(winSound, 1f);
            }
        }
    }

    private Vector3Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / gridManager.cellSize);
        int y = Mathf.RoundToInt(worldPosition.y / gridManager.cellSize);
        int z = Mathf.RoundToInt(worldPosition.z / gridManager.cellSize);
        return new Vector3Int(x, y, z);
    }
}
