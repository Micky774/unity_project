using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Robo player;
	public Slider health_slider;
	
	public void UpdateHealthBar() {
		health_slider.value = 
		Mathf.Clamp(player.curr_health / player.max_health, 
		0f, 
		1f);
	}
}
