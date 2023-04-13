using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Robo player;
	public Heart[] hearts;
	public int max_hearts;
	public Sprite filled_heart;
	public Sprite empty_heart;
	
	void Start() {
		max_hearts = Mathf.Clamp(player.max_health, 0, hearts.Length);
	}
	
	public void UpdateHealthBar(){
		if (player.curr_health > max_hearts) {
			player.curr_health = max_hearts;
		}
		for (int i = 0; i < max_hearts; i++) {
			if(i < player.curr_health) {
				hearts[i].GetComponent<Image>().sprite = filled_heart;
			}
			else {
				hearts[i].GetComponent<Image>().sprite = empty_heart;
			}
		}
	}
	
	//public Slider health_slider;
	
	/*public void UpdateHealthBar() {
		health_slider.value = 
		Mathf.Clamp(player.curr_health / player.max_health, 
		0f, 
		1f);
	}*/
}
