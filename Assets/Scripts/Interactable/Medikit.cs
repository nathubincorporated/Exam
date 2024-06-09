using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medikit : MonoBehaviour, IInteractable
{
    private CharacterMovement player;
    private SpriteRenderer SpriteRenderer;
    private CircleCollider2D circleCollider;
    private Animator playeranimator;
    public void MedikitFunction()
    {
        playeranimator = player.GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider.enabled = false;
        SpriteRenderer.enabled = false;

        //Debug.Log("YOU GOT MEDIKIT");
        player.HP.Value += 50;
        playeranimator.SetTrigger("Active");
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
        MedikitFunction();
    }
}
