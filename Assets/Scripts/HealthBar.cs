using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * UI player health bar
 * Displays player health remaining using a heart tally
 */
public class HealthBar : MonoBehaviour
{
    // Variable values assigned in Inspector
    public Robo player;
    public GameObject heart_prefab;

    // Variables instantiated during runtime
    public Heart[] hearts = new Heart[50];
    
    [Range(1, 10)]
    public int max_hearts = 5;

    
    void Start() {
        if(max_hearts > hearts.Length) {
            max_hearts = hearts.Length;
        }

        // Populates heart array up to index max_hearts
        for(int idx = 0; idx < max_hearts; idx++) {
            hearts[idx] = (
                Instantiate(heart_prefab, Vector3.zero, Quaternion.identity, transform)
                .GetComponent<Heart>()
                .Init(idx)
            );
        }
    }
    

    
    public void UpdateHealthBar(){
        // Ensures player's health variable never exceeds the max number of hearts in use
        if (player.curr_health > max_hearts) {
            player.curr_health = max_hearts;
        }

        // Sets heart sprites to match player health
        for (int i = 0; i < player.curr_health; i++) {
            hearts[i].SetMode(HEART_MODE.active);
        }
        for (int i = player.curr_health; i < max_hearts; i++) {
            hearts[i].SetMode(HEART_MODE.inactive);
        }
    }
}
