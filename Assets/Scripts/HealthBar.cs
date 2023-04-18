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
	public Robo player;
	public Heart[] hearts;
	public int max_hearts;
	public Sprite filled_heart;
	public Sprite empty_heart;
	
	void Start() {
		// Gets max number of hearts on display to be used
		max_hearts = Mathf.Clamp(player.max_health, 0, hearts.Length);
	}
	
	
	public void UpdateHealthBar(){
		// Ensures player's health variable never exceeds the max number of hearts in use
		if (player.curr_health > max_hearts) {
			player.curr_health = max_hearts;
		}

		// Sets heart sprites to match player health
		for (int i = 0; i < max_hearts; i++) {
			if(i < player.curr_health) {
				hearts[i].GetComponent<Image>().sprite = filled_heart;
			}
			else {
				hearts[i].GetComponent<Image>().sprite = empty_heart;
			}
		}
	}
}
