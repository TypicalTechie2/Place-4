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
}
