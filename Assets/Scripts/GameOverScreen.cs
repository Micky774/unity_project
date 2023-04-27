using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Game over screen scripting.
 * Provides methods for displaying a game over screen and restarting the game from said screen.
 */
public class GameOverScreen : MonoBehaviour
{
    public static float wait_time = 3f;

    private string _scene_name;

    // Displays game over screen after a short time
    public IEnumerator Display() {
        yield return new WaitForSeconds(wait_time);
        this.gameObject.SetActive(true);
    }

    void Start(){
        this._scene_name = SceneManager.GetActiveScene().name;
    }

    /* NOTE: Player's current fire action must be removed from PlayerInputs before this is called
     * Otherwise, PlayerInputs retains reference to old Robo instance, causing errors.
     */
    public void Restart(){
        SceneManager.LoadScene(this._scene_name);
    }
}
