using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public TMP_Text hptext;
    public TMP_Text cointext;
    public GameObject Panel;
    public CharacterMovement player;

    private Animator animator;
    public Button Button1;
    public Button Button2;

    private void Start()
    {
        animator = Panel.GetComponent<Animator>();
       
        player.HP.Subscribe(hp =>
        {
            hptext.text = "HP:" + hp;
        }).AddTo(this);

        player.Coins.Subscribe(coins => 
        { cointext.text = "Coins:" + coins; 
        
        }).AddTo(this);

        //Subscribe to death event
        player.OnDeathAsObservable().Subscribe(_ =>
        {
            Button1.interactable = true;
            Button2.interactable = true;
            animator.SetBool("DeathActive", true);
        }).AddTo(this);
    }
}
