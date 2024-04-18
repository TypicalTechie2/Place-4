using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource cameraAudio;
    public AudioClip startGameSound;
    public AudioClip BackgroundMusic;
    public AudioClip gameExitAudio;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Initiates the start of the game by playing an audio clip and loading the game scene after a delay.
    public void StartGame()
    {
        audioSource.PlayOneShot(startGameSound, 0.5f);
        StartCoroutine(StartGameAfterDelay());
    }

    //Delays the start of the game for a smoother transition.
    private IEnumerator StartGameAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Game Scene");
    }

    //Initiates the exit sequence by stopping background music, playing an exit audio clip, and quitting the application after a delay.
    public void GameExit()
    {
        cameraAudio.Stop();
        audioSource.PlayOneShot(gameExitAudio, 1f);
        StartCoroutine(ExitTheGameAfterDelay());
    }

    //Delays the exit of the game for a smoother transition.
    private IEnumerator ExitTheGameAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If the game is built for PC, exit the application
        Application.Quit();
#endif
    }
}
