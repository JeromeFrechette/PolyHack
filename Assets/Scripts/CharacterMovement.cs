using System;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    public InventoryHandler inventoryHandler;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        rb.velocity = moveDirection * moveSpeed;

        //UnityEngine.Debug.Log($"H: {horizontalInput}");
        //UnityEngine.Debug.Log($"V: {verticalInput}");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Set all animations to false by default
            animator.SetBool("MovingLeft", false);
            animator.SetBool("MovingRight", false);
            animator.SetBool("MovingUp", false);
            animator.SetBool("MovingDown", false);

            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                // Horizontal movement is greater
                if (horizontalInput < 0)
                {
                    // Moving left
                    animator.SetBool("MovingLeft", true);
                }
                else
                {
                    // Moving right
                    animator.SetBool("MovingRight", true);
                }
            }
            else
            {
                // Vertical movement is greater
                if (verticalInput < 0)
                {
                    // Moving down
                    animator.SetBool("MovingDown", true);
                }
                else
                {
                    // Moving up
                    animator.SetBool("MovingUp", true);
                }
            }
            // Set Idle to false since the character is moving
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("MovingLeft", false);
            animator.SetBool("MovingRight", false);
            animator.SetBool("MovingUp", false);
            animator.SetBool("MovingDown", false);
            // No movement, set Idle to true
            animator.SetBool("Idle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            SpriteRenderer spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Assuming you have a SpriteRenderer, you can access its sprite property
                Sprite itemSprite = spriteRenderer.sprite;

                // Create a new inventory item using the name, tag, and sprite of the collided GameObject
                InventoryHandler.Item newItem = new InventoryHandler.Item
                {
                    itemName = other.gameObject.name,
                    type = other.gameObject.tag,
                    sprite = itemSprite
                };

                // Call the AddItem method from the assigned InventoryHandler
                inventoryHandler.AddItem(newItem);

                // Destroy the collided GameObject
                Destroy(other.gameObject);
            }
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Can"))
        {
            inventoryHandler.HandleThrowAway(other.gameObject.tag);
        }
    }
}

