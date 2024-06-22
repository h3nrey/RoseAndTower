using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "player/new")]
public class Player : ScriptableObject
{
    public float velocity;
    public float baseJumpForce;
    public float jumpCutForce;
    public float maxFallSpeed;
    public float fallMultiplier;
    public float arrowSpeed;
    public int arrowTotal;
    public float arrowRespawnCooldown;

}

