using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovementController : NetworkBehaviour
{
    public float movementSpeed  = 5f;
    public CharacterController controller = null;
    Vector2 previousInput;

    public override void OnStartAuthority()
    {
        enabled = true;
        InputManager.Controls.Player.Move.performed += ContextMenu=> SetMovement(ContextMenu.ReadValue<Vector2>());
        InputManager.Controls.Player.Move.canceled += ContextMenu=> ResetMovement();
    }

    [ClientCallback]
    private void Update()
    {
        Move();
    }
    [Client]
    void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    void ResetMovement() => previousInput = Vector2.zero;
    [Client]
    void Move()
    {
        Vector3 right = controller.transform.right;
        Vector3 forward = controller.transform.forward;

        right.y = 0f;
        forward.y = 0f;

        Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

        controller.Move(movement * movementSpeed * Time.deltaTime);
    }

}
