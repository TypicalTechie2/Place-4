using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public HolderControllerScript holderControllerScript;
    public AudioSource playButtonAudio;
    public AudioClip restartAudio;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Restarts the game by regenerating the grid and resetting holder positions.
    public void RestartGame()
    {
        gridManager.GenerateGrid();
        holderControllerScript.ResetMainHolderPosition();
        holderControllerScript.ResetSubHoldersPosition();
        holderControllerScript.isReleased = false;
        playButtonAudio.PlayOneShot(restartAudio, 0.5f);

        StartCoroutine(RestartGameAfterDelay());
    }

    //Delays the restart of the game for a smoother transition.
    private IEnumerator RestartGameAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Loads the main menu scene.
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu Scene");
    }
}
