using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    private HolderControllerScript holderControllerScript;
    private bool crateInstantiated = false;
    private GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        holderControllerScript = GameObject.Find("Main Holder").GetComponent<HolderControllerScript>();
        gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

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
            //crateRB.constraints = RigidbodyConstraints.FreezeAll;
            // Get the position in grid coordinates
            Vector3Int gridPosition = GetGridPosition(transform.position);
            // Add the crate to the gridArray
            gridManager.AddCrateToGrid(this.gameObject, gridPosition);
            gridManager.CheckGridStatus();

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