using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private PlayerBluePrint currentHealth;

    [SerializeField] private GameObject bloodParticle;

    [SerializeField] private Renderer renderer;
    [SerializeField] private float flashTime = 0.2f;

    private void Start()
    {
        currentHealth.setHealth(1);
        Reduce(1);
    }

    public void Reduce(int damage)
    {
        float health = currentHealth.getHealth();
        currentHealth.setHealth(health -  damage / maxHealth);
        CreateHitFeedback();
        if (health <= 0)
        {
            Die();
        }
    }

    public void AddHealth(int healthBoost)
    {
        float health = currentHealth.getHealth();
        int health1 = Mathf.RoundToInt(health * maxHealth);
        int val = health1 + healthBoost;
        health = (val > maxHealth ? maxHealth : val / maxHealth);
    }

    private void CreateHitFeedback()
    {
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
        StartCoroutine(FlashFeedback());
    }

    private IEnumerator FlashFeedback()
    {
        renderer.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(flashTime);
        renderer.material.SetInt("_Flash", 0);
    }

    private void Die()
    {
        Debug.Log("Died");
        currentHealth.setHealth(1);
    }
}

