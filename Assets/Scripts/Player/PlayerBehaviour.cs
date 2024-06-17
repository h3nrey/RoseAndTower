using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] [Expandable] private Player _data;

    [SerializeField] private Vector2 input;

    [SerializeField] private UnityEvent onJump;
    // Components
    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();

        onJump.AddListener(DoJump);
    }
    private void FixedUpdate() {
        _rb.velocity = new(input.x * _data.velocity * Time.fixedDeltaTime, _rb.velocity.y);

        if(_rb.velocity.y < 0) {
            _rb.velocity = new(_rb.velocity.x, Mathf.Min(_rb.velocity.y * 0.5f, _data.fallingSpeed), _rb.velocity.z);
        }
    }

    public void GetMove(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
    }

    public void GetJump(InputAction.CallbackContext context) {
        if(context.started) {
            onJump.Invoke();
        }
    }

    private void DoJump() {
        _rb.velocity = Vector2.up * _data.baseJumpForce;
    }
}
