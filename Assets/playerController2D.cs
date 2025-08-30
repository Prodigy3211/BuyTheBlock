using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 5f;
    
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      targetPosition = rb.position; 
    }

    // Update is called once per frame
    void Update()
    {
         //Detect Mouse Click
        if (Mouse.current.leftButton.wasPressedThisFrame) //left click
        {
            //Convert Mouse position to world point
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            targetPosition = new Vector2 (mouseWorldPosition.x, mouseWorldPosition.y);
        }
    }
    void FixedUpdate()
    {
        //Move towards tarrget position
        if ((targetPosition - rb.position).sqrMagnitude > 0.01f)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
    }
}
