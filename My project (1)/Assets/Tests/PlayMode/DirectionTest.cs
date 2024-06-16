using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GamePlay;

public class PlayerMovementTests
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
    public IEnumerator PlayerMovesWhenPressingKeys()
    {
        // Simulate pressing the movement keys for a short duration
        float duration = 0.1f;
        SimulatePressKey(KeyCode.W);
        SimulatePressKey(KeyCode.A);
        SimulatePressKey(KeyCode.S);
        SimulatePressKey(KeyCode.D);

        // Wait for a short duration to allow movement to occur
        yield return new WaitForSeconds(duration);

        // Assert that the player's position has changed
        Assert.AreNotEqual(initialPosition, playerObject.transform.position);
    }

    private void SimulatePressKey(KeyCode keyCode)
    {
        // Simulate a key press event 
        switch (keyCode)
        {
            case KeyCode.W:
                playerMovement.playerMovement = Vector3.up;
                break;
            case KeyCode.A:
                playerMovement.playerMovement = Vector3.left;
                break;
            case KeyCode.S:
                playerMovement.playerMovement = Vector3.down;
                break;
            case KeyCode.D:
                playerMovement.playerMovement = Vector3.right;
                break;
        }
    }
}