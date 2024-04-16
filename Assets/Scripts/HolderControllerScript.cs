using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderControllerScript : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject holder_1;
    public GameObject holder_2;
    public GameObject[] cratePrefabs;
    public float holderReleaseSpeed = 10f;
    public bool isReleased = false;
    private float holder1XBoundary = 11f;
    private float holder2XBoundary = -5f;
    private int crateIndex = 0;
    private GameObject spawnedCrates;
    private float mainHolderMoveSpeed = 1;

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
    }

    public void MainHolderMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > 0 && !isReleased)
        {
            transform.Translate(Vector3.left * mainHolderMoveSpeed);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < (gridManager.gridSizeX - 1) * gridManager.cellSize && !isReleased)
        {
            transform.Translate(Vector3.right * mainHolderMoveSpeed);
        }
    }

    public void HolderRelease()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isReleased)
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

    public void CrateInstantiate()
    {
        if (crateIndex >= cratePrefabs.Length)
        {
            crateIndex = 0;
        }
        Vector3 spawnPosition = transform.position;
        spawnedCrates = Instantiate(cratePrefabs[crateIndex], spawnPosition, Quaternion.identity);
        spawnedCrates.transform.parent = transform;

        crateIndex++;
    }

    public void ResetMainHolderPosition()
    {
        transform.position = new Vector3(3, 8, 0);
    }

    public void ResetSubHoldersPosition()
    {
        holder_1.transform.position = new Vector3(-1.5f, 1, 0);
        holder_2.transform.position = new Vector3(7.5f, 1, 0);
    }

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
}
