using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Enemy
{
    private const float MAX_SPEED = 4;
    private const float ACCELERATION_RATE = 1;

    protected override void Start()
    {
        Rigidbody2D player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        MoveBehaviour myMoveBehaviour = new FollowPlayer(this, player, MAX_SPEED, ACCELERATION_RATE);
        this.myCombatBehaviour = new JustMove(myMoveBehaviour);
    }
}
