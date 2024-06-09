using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public float KnockbackForce = 5f;
    public float KnockbackCounter = .2f;
    public float KnockbackTime;
    public bool KnockbackFromRight;

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private BoxCollider2D boxCollider2D;

    private IInteractable ThisInteractable;
    private PlayerInput _playerInput;
    private InputAction _interactAction;

    public Animator animator;
    public ReactiveProperty<int> HP { get; private set; }
    public ReactiveProperty<int> Coins { get; private set; }
    private Subject<Unit> onDeath;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        Debug.Log("Awake called");
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions["Interact"];
        HP = new ReactiveProperty<int>(100);
        Coins = new ReactiveProperty<int>(0);
        onDeath = new Subject<Unit>();

        HP.Subscribe(newHP =>
        {
            if (newHP <= 0)
            {
                onDeath.OnNext(Unit.Default);
                animator.SetBool("Dead", true);
                DisablePlayerInput();
            }
        });
        _interactAction.performed += OnInteract;
    }

    private void OnEnable()
    {
        _interactAction.performed += OnInteract;
    }

    private void OnDisable()
    {
        _interactAction.performed -= OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if (isFacingRight && horizontal < 0f) 
        {
            Flip();
        }
        else if (!isFacingRight && horizontal > 0f) 
        {
            Flip();
        }
    }

    public void Jump(InputAction.CallbackContext context) 
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            //Debug.Log("JUMPING");
           //Animator.SetBool("Grounded", false);
        }

        if (context.canceled && rb.velocity.y > 0f) 
        {
            rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Flip() 
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnInteract(InputAction.CallbackContext context) 
    {
        if (ThisInteractable != null) 
        {
            ThisInteractable.OnInteract();
        }
    }

    public void SetIInstance(IInteractable interactable) 
    {
        ThisInteractable = interactable;
    }

    public void ClearIInstance()
    { 
        ThisInteractable = null; 
    }

    public void Move(InputAction.CallbackContext context) 
    {
       //Debug.Log("Pressing" + context);
        horizontal = context.ReadValue<Vector2>().x;
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private void DisablePlayerInput()
    {
        // Disable player input when HP reaches zero
        _playerInput.DeactivateInput();
    }

    //private void OnCollisionEnter2D(Collider2D collision)
    //{
      //  if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        //{
          //  Animator.SetBool("Grounded", true);
        //}
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
       // if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //{
          //  Animator.SetBool("Grounded", false);
        //}
    //}
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public IObservable<Unit> OnDeathAsObservable()
    {
        return onDeath.AsObservable();
    }
}
