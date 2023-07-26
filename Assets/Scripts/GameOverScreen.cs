using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Basic game over screen with restart button
/// </summary>
public class GameOverScreen : MonoBehaviour {
    /// <summary>
    /// Time between Display() call and GameOverScreen being displayed
    /// </summary>
    private const float _WAIT_TIME = 3f;

    /// <summary>
    /// Filename of the currently active scene
    /// </summary>
    private string _scene_name;

    /// <summary>
    /// Displays the GameOverScreen after a brief delay
    /// </summary>
    /// <param name="delay"> Seconds before GameOverScreen is displayed </param>
    /// <returns> IEnumerator corresponding to the Display coroutine </returns>
    public IEnumerator Display(float delay = GameOverScreen._WAIT_TIME) {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Retrieves and stores the filename of the current scene
    /// </summary>
    void Start() {
        this._scene_name = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Restarts the scene
    /// </summary>
    public void Restart() {
        SceneManager.LoadScene(this._scene_name);
    }
}
