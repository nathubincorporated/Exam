using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IInteractable
{
    private CharacterMovement player;
    private SpriteRenderer SpriteRenderer;
    private CircleCollider2D circleCollider;
    private Animator animator;
    private Animator camAnimator;
    private Camera cam;
    //private Animator playerAnimator;
    public void BombFunction()
    {
        cam = FindObjectOfType<Camera>();
        camAnimator = cam.GetComponent<Animator>();
        animator = GetComponent<Animator>();
        //playerAnimator = player.
        circleCollider = GetComponent<CircleCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider.enabled = false;
       

       // Debug.Log("YOU GOT BOMB");
        animator.SetTrigger("Triggered");
        camAnimator.SetTrigger("Trigger");
        player.HP.Value -= 100;

        if (player.HP.Value > 0) 
        {
            player.animator.SetTrigger("Damaged");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("PLAYER IS TRIGGERING SOMETHING");
            player = collision.GetComponent<CharacterMovement>();
            player.SetIInstance(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.ClearIInstance();
        }
    }

    public void OnInteract()
    {
        BombFunction();
    }
}
