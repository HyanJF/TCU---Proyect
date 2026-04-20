using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private MovementController movement;
    private Vector2 input;

    private void Awake()
    {
        movement = GetComponent<MovementController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        movement.SetMovement(input);
    }
}