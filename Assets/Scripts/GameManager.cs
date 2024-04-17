using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public HolderControllerScript holderControllerScript;
    public CrateController crateControllerScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        gridManager.GenerateGrid();
        holderControllerScript.CrateInstantiate();
        holderControllerScript.isReleased = false;
        holderControllerScript.ResetMainHolderPosition();
        holderControllerScript.ResetSubHoldersPosition();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
