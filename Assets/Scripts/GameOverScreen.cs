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
    public void Display() {
        gameObject.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("SampleScene");
    }
}
