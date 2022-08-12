using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Takes and handles input and movement for a player character
public class PlayerMovement : MonoBehaviour
{
    public bool Busy = false;
    public bool CanMove = true;
    public Vector2 LastDir;
    public Vector2 MovementInput;

    [SerializeField]
    private float _moveSpeed = 1f;

    private Animator _anim;
    private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();
    private float _collisionOffset = 0.05f;
    private ContactFilter2D _movementFilter;
    private PlayerAnimation _playerAnimation;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        _playerAnimation.AnimControls();
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            // If movement input is not 0, try to move
            if (MovementInput != Vector2.zero)
            {
                LastDir = MovementInput;
                bool success = TryMove(MovementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(MovementInput.x, 0));
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, MovementInput.y));
                }
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = _rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                _movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                _castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                _moveSpeed * Time.fixedDeltaTime + _collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                _rb.MovePosition(_rb.position + direction * _moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Can't move if there's no direction to move in
            return false;
        }

    }

    public void LockMovement()
    {
        CanMove = false;
    }

    public void UnlockMovement()
    {
        CanMove = true;
    }
}