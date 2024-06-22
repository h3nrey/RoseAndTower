using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    Player _data;
    PlayerBehaviour _player;
    Rigidbody _rb;
    Vector3 _dir;
    float speed;
    [ReadOnly] [SerializeField] private bool _canMove;
    public bool isEnabled;
    private float respawnTime;
    private void OnEnable() {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate() {
        if(_canMove) {
            _rb.velocity = speed * Time.fixedDeltaTime * _dir;  
        }
    }

    public void SetupArrow(Vector3 dir, Player data, PlayerBehaviour player) {
        _dir = dir;
        _data = data;
        speed = data.arrowSpeed;
        _player = player;

        EnableArrow();
    }

    public void HandleArrow(Transform point) {
        EnableArrow();
    }

    private void EnableArrow() {
        if(respawnTime < Time.time && !_data) return;

        _canMove = false;
        _canMove = true;
        _dir = _player.GetLastInput();
        transform.position = _player.shootPoint.position;
        isEnabled = true;

        respawnTime = Time.time + _data.arrowRespawnCooldown;
    }
    private void DestroySelf(Transform point) {     
        _canMove = false;
        gameObject.SetActive(false);
        _rb.velocity = Vector3.zero;
        isEnabled = false;
    }
}
