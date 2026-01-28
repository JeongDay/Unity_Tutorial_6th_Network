using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject[] animObjs;
    
    private Rigidbody2D rb;

    private Vector3 moveInput;

    public float moveSpeed = 2f;
    public float jumpPower = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        animObjs[0].SetActive(moveInput.x == 0);
        animObjs[1].SetActive(moveInput.x != 0);

        // Flip 기능
        if (moveInput.x != 0)
        {
            int scaleX = moveInput.x < 0 ? 1 : -1;
            transform.localScale = new Vector3(scaleX, 1, 1);
            
            rb.linearVelocityX = moveInput.x * moveSpeed; // 이동 기능
        }
    }

    void OnMove(InputValue value)
    {
        var input = value.Get<Vector2>();
        moveInput = new Vector3(input.x, 0, 0);
    }

    void OnJump(InputValue value)
    {
        rb.AddForceY(jumpPower, ForceMode2D.Impulse);
    }
}