using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;


namespace GamePlay
{

    public class PlayerMovement : MonoBehaviour
    {
        private float speed = 10f;
        private Rigidbody2D myRigidbody;
        [SerializeField] private Vector3 playerMovement;
        public List<Animator> animators;

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed = 50f;
        [SerializeField] private float dashDuration = 0.09f;
        [SerializeField] private float dashCooldown = 0.5f; // Adjusted cooldown
        private bool isDashing;
        private bool canDash = true;



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

        private void Update()
        {
            if (!isDashing)
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
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = 15;
                }
                else
                {
                    speed = 10;
                }

                if (Input.GetKeyDown(KeyCode.Space) && canDash)
                {
                    StartCoroutine(Dash());
                }
            }
        }

        private void FixedUpdate()
        {
            if (!isDashing)
            {
                UpdateAnimationAndMove();
            }
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
            playerMovement = playerMovement.normalized;
            myRigidbody.MovePosition(transform.position + playerMovement * speed * Time.deltaTime);
        }

        private IEnumerator Dash()
        {
            isDashing = true;
            canDash = false;
            Vector3 dashDirection = playerMovement.normalized;
            float startTime = Time.time;

            while (Time.time < startTime + dashDuration)
            {
                myRigidbody.velocity = dashDirection * dashSpeed;
                yield return null;
            }

            myRigidbody.velocity = Vector2.zero; // Stop the dash movement
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }

}