using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI tieText;
    public HolderControllerScript holderControllerScript;
    public Material winningMaterial;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    //Creates a grid of cells using the specified prefab and dimensions.
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

    //Checks if the grid is full and calls other methods accordingly.
    public void CheckGridStatus()
    {
        bool isGridFull = true;
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

                    else
                    {
                        isGridFull = false;
                    }
                }
            }
        }

        if (isGridFull && CheckForWinCondition() == null)
        {
            holderControllerScript.isTimerActive = false;
            holderControllerScript.isGameOver = true;
            Debug.Log("Grid is Full!");
            tieText.gameObject.SetActive(true);
            holderControllerScript.restartGameButton.gameObject.SetActive(true);
            holderControllerScript.exitToMenuButton.gameObject.SetActive(true);
        }
    }

    //Adds a crate to the grid array at the specified position.
    public void AddCrateToGrid(GameObject crate, Vector3Int position)
    {
        gridArray[position.x, position.y, position.z] = crate;
    }

    //Checks for winning conditions by iterating through the grid array.
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
                        if (CheckForVerticalWin("Yellow Crate", x, y, z) || CheckForHorizontalWin("Yellow Crate", x, y, z) ||
                        CheckForDiagonalWin("Yellow Crate", x, y, z) || CheckForDiagonalWin1("Yellow Crate", x, y, z))

                            return "Yellow Crate";
                    }

                    else if (gridArray[x, y, z] != null && gridArray[x, y, z].CompareTag("Red Crate"))
                    {
                        if (CheckForVerticalWin("Red Crate", x, y, z) || CheckForHorizontalWin("Red Crate", x, y, z) ||
                        CheckForDiagonalWin("Red Crate", x, y, z) || CheckForDiagonalWin1("Red Crate", x, y, z))

                            return "Red Crate";
                    }
                }
            }
        }
        return null;
    }

    //Checks for vertical win conditions starting from the specified position.
    private bool CheckForVerticalWin(string crateTag, int startX, int startY, int startZ)
    {
        if (startY <= gridSizeY - 4)
        {
            for (int y = startY; y < startY + 4; y++)
            {
                if (gridArray[startX, y, startZ] == null || !gridArray[startX, y, startZ].CompareTag(crateTag))
                    return false;
            }

            Debug.Log("Vertical Win Condition Met!");
            LogWinningCrates(crateTag, startX, startY, startZ, 0, 1, 0);
            return true;
        }
        return false;
    }

    //Checks for horizontal win conditions starting from the specified position.
    private bool CheckForHorizontalWin(string crateTag, int startX, int startY, int startZ)
    {
        if (startX <= gridSizeX - 4)
        {
            for (int x = startX; x < startX + 4; x++)
            {
                if (gridArray[x, startY, startZ] == null || !gridArray[x, startY, startZ].CompareTag(crateTag))
                    return false;
            }

            Debug.Log("Horizontal Win Condition Met!");
            LogWinningCrates(crateTag, startX, startY, startZ, 1, 0, 0);
            return true;
        }
        return false;
    }

    //Checks for diagonal win conditions starting from the specified position.
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

            Debug.Log("Diagonal \\ Win Condition Met!");
            LogWinningCrates(crateTag, startX, startY, startZ, 1, 1, 0);
            return true;
        }
        return false;
    }

    private bool CheckForDiagonalWin1(string crateTag, int startX, int startY, int startZ)
    {

        // Check diagonal /
        if (startX >= 3 && startY <= gridSizeY - 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (gridArray[startX - i, startY + i, startZ] == null || !gridArray[startX - i, startY + i, startZ].CompareTag(crateTag))
                    return false;
            }

            Debug.Log("Diagonal / Win Condition Met!");
            LogWinningCrates(crateTag, startX, startY, startZ, -1, 1, 0);
            return true;
        }

        return false;
    }

    // Record Crates that formded winning pattern
    private void LogWinningCrates(string crateTag, int startX, int startY, int startZ, int stepX, int stepY, int stepZ)
    {

        for (int i = 0; i < 4; i++)
        {
            int x = startX + i * stepX;
            int y = startY + i * stepY;
            int z = startZ + i * stepZ;
            Debug.Log(crateTag + " at position (" + x + ", " + y + ", " + z + ") is part of the winning pattern.");

            GameObject winningCrate = gridArray[x, y, z];
            if (winningCrate != null)
            {
                Renderer crateRenderer = winningCrate.GetComponent<Renderer>();
                if (crateRenderer != null && crateRenderer.materials.Length >= 2)
                {
                    Material material = crateRenderer.material;
                    material = winningMaterial;
                    crateRenderer.material = material;
                }
            }
        }
    }
}
