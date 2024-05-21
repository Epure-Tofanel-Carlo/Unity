using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    private float speed = 6f;
    private Rigidbody2D myRigidbody;
    [SerializeField] private Vector3 playerMovement;
    public List<Animator> animators;

    private void Start()
    {
        animators = new List<Animator>();
        Transform player = transform;
        for (int i = 0; i < player.childCount; i++)
        {
            var anim = player.GetChild(i).GetComponent<Animator>();
            if (anim != null)
                animators.Add(anim);
        }
        playerMovement = Vector3.zero;
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        playerMovement = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            playerMovement.y = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            playerMovement.x = Input.GetAxisRaw("Horizontal");
        }




        UpdateAnimationAndMove();
    }

    private void UpdateAnimationAndMove()
    {
        if (playerMovement != Vector3.zero)
        {
            MoveCharacter();
            foreach (Animator animator in animators)
            {
                animator.SetFloat("moveX", playerMovement.x);
                animator.SetFloat("moveY", playerMovement.y);
                animator.SetBool("moving", true);
            }
        }
        else
        {
            foreach (Animator animator in animators)
            {
                animator.SetBool("moving", false);
            }

        }
    }

    private void MoveCharacter()
    {
        myRigidbody.MovePosition(transform.position + playerMovement * speed * Time.deltaTime);
    }
}
