using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public float _Speed = 5f;

    private Rigidbody2D _Rb;
    private Animator _PAnim;

    private Vector2 _MoveInput;

    private void Awake()
    {
        _Rb = GetComponent<Rigidbody2D>();
        _PAnim = GetComponent<Animator>();
    }

    public void SetMovement(Vector2 direction)
    {

        _MoveInput = direction;

        if (_MoveInput.sqrMagnitude > 0.01f)
        {
            _PAnim.SetFloat("Horizontal", _MoveInput.x);
            _PAnim.SetFloat("Vertical", _MoveInput.y);
        }

        _PAnim.SetFloat("Speed", _MoveInput.sqrMagnitude);
    }

    public bool HasMovement()
    {
        return _MoveInput.sqrMagnitude > 0.01f;
    }

    private void FixedUpdate()
    {
        _Rb.MovePosition(_Rb.position + _MoveInput * _Speed * Time.fixedDeltaTime);

    }
}
