using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
  
    public float moveSpeed = 1f;

    // Player Rigidbody2D
    public Rigidbody2D rigidbody2D;
    public Animator animator;

    // Vector pentru stocarea directiei de mers
    Vector2 movement;

    // Metoda Update este o data per frame
    void Update()
    {
        // iau practic ultimul movment pentru a putea face si animatia de idle mai diversificata

        if(movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("LastHorizontal", movement.x);
            animator.SetFloat("LastVertical", movement.y);

        }
        // citesc intrearea de pe axa, WASD
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        // sa dam update la variabile din animator cu numele vertical/horizontal ca sa isi dea trigger animatia 
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
      
        animator.SetFloat("Speed", movement.magnitude);
    }

    // Metoda FixedUpdate este apelata la intervale fixe si o folosesc pentru actualizari pe fizici
    void FixedUpdate()
    {
        // Deplasarea Jucatorului conform vectorului de miscare, vitezei si timpului fixat delta
        rigidbody2D.MovePosition(rigidbody2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
