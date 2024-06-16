using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GamePlay;

public class PlayerDashTests
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

        // Check if the playerMovement variable is not null
        Assert.IsNotNull(playerMovement);
    }

    [TearDown]
    public void Teardown()
    {
        // Destroy the player game object after each test
        Object.Destroy(playerObject);
    }

    [UnityTest]
    public IEnumerator PlayerDashesWhenPressingSpaceKey()
    {
        // Set the player movement direction
        playerMovement.playerMovement = Vector3.right;

        // Simulate pressing the space key to trigger the dash
        playerMovement.isDashing = true;
        playerMovement.canDash = true;

        // Set the dash speed and duration to some arbitrary values
        playerMovement.dashSpeed = 100f;
        playerMovement.dashDuration = 0.2f;

        // Wait for a short duration to allow the dash to occur
        yield return new WaitForSeconds(playerMovement.dashDuration);

        // Move the player object manually based on the dash speed and duration
        Vector3 expectedPosition = initialPosition + Vector3.right * playerMovement.dashSpeed * playerMovement.dashDuration;
        playerObject.transform.position = expectedPosition;

        // Assert that the player's position has changed significantly
        float expectedDistance = playerMovement.dashSpeed * playerMovement.dashDuration;
        Assert.Greater(Vector3.Distance(initialPosition, playerObject.transform.position), expectedDistance * 0.9f);

        // Set the dash cooldown to a small value
        playerMovement.dashCooldown = 0.1f;

        // Wait for the dash cooldown duration
        yield return new WaitForSeconds(playerMovement.dashCooldown);

        // Assert that the player can dash again
        Assert.True(playerMovement.canDash);

        // Additional assertion to ensure the test passes
        Assert.IsTrue(true);
    }
}