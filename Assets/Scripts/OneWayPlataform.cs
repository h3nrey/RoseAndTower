using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class OneWayPlataform : MonoBehaviour
{
    [SerializeField] private bool isOneWay;

    [ReadOnly] [SerializeField] private Collider col;

    [SerializeField] private float plataformRange;
    [SerializeField] private LayerMask playerMask;
    private Collider[] touchingObj;


    private void Update() {

    }

    private void FixedUpdate() {
        Vector3 size = new(transform.localScale.x / 2, (plataformRange + transform.localScale.y) / 2, 1);
        touchingObj = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, playerMask);

        foreach (Collider obj in touchingObj) {
            print(obj.name);
            if(obj.gameObject.tag == "Player") {
                PlayerBehaviour player = obj.gameObject.GetComponent<PlayerBehaviour>();
                print("colliding with player");
                if(player.groundPoint.position.y > transform.position.y) {
                    col.isTrigger = false;
                } else {
                    col.isTrigger = true;
                }
            }
        }
    }
    private void Start() {
        col = GetComponent<Collider>();
    }

    private void OnDrawGizmosSelected() {
        Vector3 size = new(transform.localScale.x, plataformRange + transform.localScale.y, 1);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
