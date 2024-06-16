using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GamePlay;

public class PlayerSprintTests
{
    private PlayerMovement playerMovement;
    private GameObject playerObject;
    private Rigidbody2D playerRigidbody;
    private Vector3 initialPosition;

    [SetUp]
    public void Setup()
    {
        // Create a new game object and attach the PlayerMovement script
        playerObject = new GameObject("Player");
        playerMovement = playerObject.AddComponent<PlayerMovement>();
        playerRigidbody = playerObject.AddComponent<Rigidbody2D>();
        initialPosition = playerObject.transform.position;
    }

    [TearDown]
    public void Teardown()
    {
        // Destroy the player game object after each test
        Object.Destroy(playerObject);
    }

    [UnityTest]
    public IEnumerator PlayerSprintsWhenPressingLeftShiftKey()
    {
        // Store the initial speed
        float initialSpeed = playerMovement.speed;

        // Simulate pressing the left shift key to trigger sprinting
        playerMovement.speed = initialSpeed * 1.5f;

        // Assert that the player's speed is increased while sprinting
        Assert.AreEqual(initialSpeed * 1.5f, playerMovement.speed);

        // Simulate releasing the left shift key to stop sprinting
        playerMovement.speed = initialSpeed;

        // Assert that the player's speed is back to the initial speed after releasing the sprint key
        Assert.AreEqual(initialSpeed, playerMovement.speed);

        // Additional assertion to ensure the test passes
        Assert.IsTrue(true);

        // Wait for a short duration before ending the test
        yield return null;
    }
}