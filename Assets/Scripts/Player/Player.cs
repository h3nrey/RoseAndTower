using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "player/new")]
public class Player : ScriptableObject
{
    public float velocity;
    public float baseJumpForce;
    public float fallingSpeed;
}
