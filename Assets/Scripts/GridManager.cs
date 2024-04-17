using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public GameObject[] cratePrefab;
    public int gridSizeX = 7;
    public int gridSizeY = 6;
    public int gridSizeZ = 1;
    public float cellSize = 1f;
    public GameObject[,,] gridArray;
    public GameObject mainHolder;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        CheckGridStatus();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGrid()
    {
        gridArray = new GameObject[gridSizeX, gridSizeY, gridSizeZ];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 spawnPosition = new Vector3(x * cellSize, y * cellSize + 0.5f, z * cellSize);
                    GameObject cell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity);
                    cell.transform.SetParent(transform);
                    gridArray[x, y, z] = null;
                }
            }
        }
    }

    public void CheckGridStatus()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    if (gridArray[x, y, z] != null)
                    {
                        Debug.Log("Cell at position (" + x + ", " + y + ", " + z + ") is not null.");
                    }
                }
            }
        }
    }

    public void AddCrateToGrid(GameObject crate, Vector3Int position)
    {
        gridArray[position.x, position.y, position.z] = crate;
    }

    public string CheckForWinCondition()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    if (gridArray[x, y, z] != null && gridArray[x, y, z].CompareTag("Yellow Crate"))
                    {
                        // Check for vertical win
                        if (CheckForVerticalWin("Yellow Crate", x, y, z) || CheckForHorizontalWin("Yellow Crate", x, y, z) || CheckForDiagonalWin("Yellow Crate", x, y, z))
                            return "Yellow Crate";
                    }

                    else if (gridArray[x, y, z] != null && gridArray[x, y, z].CompareTag("Red Crate"))
                    {
                        if (CheckForVerticalWin("Red Crate", x, y, z) || CheckForHorizontalWin("Red Crate", x, y, z) || CheckForDiagonalWin("Red Crate", x, y, z))
                            return "Red Crate";
                    }
                }
            }
        }
        return null;
    }

    private bool CheckForVerticalWin(string crateTag, int startX, int startY, int startZ)
    {
        if (startY <= gridSizeY - 4)
        {
            for (int y = startY; y < startY + 4; y++)
            {
                if (gridArray[startX, y, startZ] == null || !gridArray[startX, y, startZ].CompareTag(crateTag))
                    return false;
            }
            return true;
        }
        return false;
    }

    private bool CheckForHorizontalWin(string crateTag, int startX, int startY, int startZ)
    {
        if (startX <= gridSizeX - 4)
        {
            for (int x = startX; x < startX + 4; x++)
            {
                if (gridArray[x, startY, startZ] == null || !gridArray[x, startY, startZ].CompareTag(crateTag))
                    return false;
            }
            return true;
        }
        return false;
    }

    private bool CheckForDiagonalWin(string crateTag, int startX, int startY, int startZ)
    {
        // Check diagonal \
        if (startX <= gridSizeX - 4 && startY <= gridSizeY - 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (gridArray[startX + i, startY + i, startZ] == null || !gridArray[startX + i, startY + i, startZ].CompareTag(crateTag))
                    return false;
            }
            return true;
        }

        // Check diagonal /
        if (startX >= 3 && startY <= gridSizeY - 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (gridArray[startX - i, startY + i, startZ] == null || !gridArray[startX - i, startY + i, startZ].CompareTag(crateTag))
                    return false;
            }
            return true;
        }

        return false;
    }
}
