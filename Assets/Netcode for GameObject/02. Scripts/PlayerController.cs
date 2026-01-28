using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public enum ActionType { Idle, Walk, Attack }
    public ActionType actionType = ActionType.Idle;
    
    public GameObject[] animObjs;
    
    private NetworkVariable<ActionType> currentAnimState = new NetworkVariable<ActionType>();
    
    private Rigidbody2D rb;
    private Vector3 moveInput;

    public float moveSpeed = 2f;
    public float jumpPower = 3f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        rb = GetComponent<Rigidbody2D>();
        currentAnimState.OnValueChanged += SetAnimObject;

        if (IsOwner)
        {
            CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();
            cameraFollow.SetTarget(transform);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        
        currentAnimState.OnValueChanged -= SetAnimObject;
    }

    void FixedUpdate()
    {
        if (IsOwner)
            MovementServerRpc(moveInput);
    }

    [ServerRpc]
    private void MovementServerRpc(Vector3 moveInput)
    {
        if (currentAnimState.Value == ActionType.Attack)
            return;
        
        if (moveInput.x != 0)
        {
            currentAnimState.Value = ActionType.Walk;
            
            int scaleX = moveInput.x < 0 ? 1 : -1;
            transform.localScale = new Vector3(scaleX, 1, 1);
            
            rb.linearVelocityX = moveInput.x * moveSpeed; // 이동 기능
        }
        else
            currentAnimState.Value = ActionType.Idle;
    }

    void OnMove(InputValue value)
    {
        var input = value.Get<Vector2>();
        moveInput = new Vector3(input.x, 0, 0);
    }

    void OnJump(InputValue value)
    {
        if (IsOwner)
            JumpServerRpc();
    }

    [ServerRpc]
    private void JumpServerRpc()
    {
        rb.AddForceY(jumpPower, ForceMode2D.Impulse);
        
    }

    void OnAttack(InputValue value)
    {
        if (IsOwner)
            AttackServerRpc();
    }
    
    [ServerRpc]
    private void AttackServerRpc()
    {
        if (currentAnimState.Value == ActionType.Attack)
            return;
        
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        currentAnimState.Value = ActionType.Attack;
        yield return new WaitForSeconds(1f);

        currentAnimState.Value = ActionType.Idle;
    }

    private void SetAnimObject(ActionType preType, ActionType newType)
    {
        for (int i = 0; i < animObjs.Length; i++)
            animObjs[i].SetActive(i == (int)newType);
    }
}