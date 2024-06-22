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
    private Vector2 lastInput;

    // Events
    [SerializeField] private UnityEvent onJump;
    [SerializeField] private UnityEvent onReleaseJump;
    [SerializeField] private UnityEvent onShoot;

    // Shoot
    [ReadOnly] [SerializeField] private  GameObject _currArrow;
    [ReadOnly] [SerializeField] private int _remainingArrows;
    [SerializeField] private GameObject arrowObj;
    public Transform shootPoint;

    // Jump
    [ReadOnly] [SerializeField] private bool onGround;
    [SerializeField] public Transform groundPoint;
    [SerializeField] private float groundRange;
    [SerializeField] private LayerMask groundLayer;

    // Components
    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();

        onJump.AddListener(DoJump);
        onReleaseJump.AddListener(ApplyJumpCut);
        onShoot.AddListener(Shoot);

        _remainingArrows = _data.arrowTotal;
    }
    private void FixedUpdate() {
        Move();

        if(_rb.velocity.y < 0) {
            float yVel = Mathf.Max(_data.maxFallSpeed, _rb.velocity.y - _data.fallMultiplier);
            _rb.velocity = new(_rb.velocity.x, yVel, _rb.velocity.z);
        }

        CheckIfIsGround();
    }

    #region Input
    public void GetMove(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
        if(input.magnitude > 0) {
            lastInput = input;
        } 
        
    }

    public Vector2 GetLastInput() {
        return lastInput;
    }

    public void GetJump(InputAction.CallbackContext context) {
        if(context.started) {
            onJump.Invoke();
        }

        if(context.canceled) {
            onReleaseJump.Invoke();
        }
    }

    public void GetShoot(InputAction.CallbackContext context) {
        if(context.started) {
            onShoot.Invoke();
        }
    }
    #endregion

    private void Move() {
        _rb.velocity = new(input.x * _data.velocity * Time.fixedDeltaTime, _rb.velocity.y);

        if(input.x > 0) {
            Vector3 rot = new(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rot);
        } else if(input.x < 0) {
            Vector3 rot = new(transform.rotation.x, 180, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rot);
        }
    }
    private void DoJump() {
        if(!onGround) return;
        _rb.velocity = Vector2.up * _data.baseJumpForce;
    }

    private void ApplyJumpCut() {
        if(_rb.velocity.y <= 0) return; 
        _rb.velocity = new(_rb.velocity.x, _rb.velocity.y * _data.jumpCutForce, _rb.velocity.z);
    }

    private void Shoot() {
        if(!_currArrow) {
            _currArrow = Instantiate(arrowObj, shootPoint.position, Quaternion.identity);
            _currArrow.GetComponent<ArrowBehaviour>().SetupArrow(transform.right, _data, this);
            _remainingArrows--;
        } else {
            _currArrow.GetComponent<ArrowBehaviour>().HandleArrow(shootPoint);
        }
    }

    private void CheckIfIsGround() {
        Collider[] touchingObjs = Physics.OverlapSphere(groundPoint.position, groundRange, groundLayer);

        if(touchingObjs.Length > 0) {
            onGround = true;
            return;
        } 
        onGround = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(groundPoint.position, groundRange);
    }
}
