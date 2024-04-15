using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridPrefab;
    public int gridSizeX = 7;
    public int gridSizeY = 6;
    public int gridSizeZ = 1;
    public float cellSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 spawnPosition = new Vector3(x * cellSize, y * cellSize + 0.5f, z * cellSize);
                    GameObject cell = Instantiate(gridPrefab, spawnPosition, Quaternion.identity);
                    cell.transform.SetParent(transform);
                }
            }
        }
    }
}
