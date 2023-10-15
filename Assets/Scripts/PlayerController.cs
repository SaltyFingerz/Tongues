using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    //https://www.youtube.com/watch?v=7iYWpzL9GkM
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
  //  public MovementJoystick movementJoystick; 
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public float speed = 1f;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = transform.position;

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && movementInput == Vector2.zero)
        {

            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;

        }
        else
            target = transform.position;
        if (movementInput == Vector2.zero)
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

      /*  if (movementJoystick.joystickVec.y != 0)
        {
            rb.velocity = new Vector2(movementJoystick.joystickVec.x * moveSpeed, movementJoystick.joystickVec.y * moveSpeed);
        }

        else
        {
            rb.velocity = Vector2.zero;
        }
      */

    





        if(movementInput != Vector2.zero)
        {
           bool success = TryMove(movementInput);

            if(!success) //smoothen movement when moving and colliding, to slide along objects.
            {
                success = TryMove(new Vector2(movementInput.x, 0));

            }

            if (!success)
            {
                success = TryMove(new Vector2(0, movementInput.y));
            }

            animator.SetBool("isMoving", success);
        } else
        {
            animator.SetBool("isMoving", false);
        }

        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if(movementInput.y < 0 && movementInput.x == 0)
        {
            animator.SetBool("isMovingTowards", true);
        }
        else
        {
            animator.SetBool("isMovingTowards", false);
        }

        if(movementInput.y > 0 && movementInput.x == 0)
        {
            animator.SetBool("isMovingAway", true);
        }
        else
        {
            animator.SetBool("isMovingAway", false);
        }

    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {

            int count = rb.Cast( //checks if move is possible before doing it.
                    direction,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
        

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    
}
