using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 7f;


    private InputManager inputManager;
    private Vector2 move;

    private void Awake() {
        inputManager = new InputManager();
        inputManager.Player.Enable();
    }

    private void OnDestroy() {
        inputManager.Dispose();
    }

    private void Update() {
        MovePlayer();
    }

    public void MovePlayer() {
        Vector2 inputVector = GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        transform.position += moveDir * moveDistance;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = inputManager.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}
