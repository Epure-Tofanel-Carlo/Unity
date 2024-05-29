using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 5f;
    public float wanderRange = 2f;
    public float wanderInterval = 3f;
    public float stopDistance = 0.5f;

    private Transform player;
    private Rigidbody2D myRigidbody;
    private Vector3 enemyMovement;
    private bool isWandering;
    public List<Animator> animators;
    private Coroutine wanderCoroutine;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        animators = new List<Animator>();
        Transform enemy = transform;
        for (int i = 0; i < enemy.childCount; i++)
        {
            var anim = enemy.GetChild(i).GetComponent<Animator>();
            if (anim != null)
                animators.Add(anim);
        }
        wanderCoroutine = StartCoroutine(Wander());
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (wanderCoroutine != null)
            {
                StopCoroutine(wanderCoroutine);
                wanderCoroutine = null;
                isWandering = false;
            }

            if (distanceToPlayer > stopDistance)
            {
                enemyMovement = (player.position - transform.position).normalized;
            }
            else
            {
                enemyMovement = Vector3.zero;
            }
        }
        else
        {
            if (!isWandering)
            {
                wanderCoroutine = StartCoroutine(Wander());
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateAnimationAndMove();
    }

    private void UpdateAnimationAndMove()
    {
        if (enemyMovement != Vector3.zero)
        {
            MoveCharacter();
            foreach (Animator animator in animators)
            {
                animator.SetFloat("moveX", enemyMovement.x);
                animator.SetFloat("moveY", enemyMovement.y);
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
        myRigidbody.MovePosition(transform.position + enemyMovement * speed * Time.deltaTime);
    }

    private IEnumerator Wander()
    {
        isWandering = true;
        while (true)
        {
            Vector3 wanderTarget = new Vector3(
                transform.position.x + Random.Range(-wanderRange, wanderRange),
                transform.position.y + Random.Range(-wanderRange, wanderRange),
                transform.position.z);

            enemyMovement = (wanderTarget - transform.position).normalized;

            yield return new WaitForSeconds(wanderInterval);

            while (Vector3.Distance(transform.position, wanderTarget) > 0.1f)
            {
                enemyMovement = (wanderTarget - transform.position).normalized;
                MoveCharacter();
                yield return null;
            }

            enemyMovement = Vector3.zero;
        }
    }
}
