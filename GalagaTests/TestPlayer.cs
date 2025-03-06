namespace GalagaTests;

using System;
using System.Numerics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using Galaga;
using NUnit.Framework;

public class TestsPlayer {
    public Player testPlayer;
    public Player testPlayer2;

    [SetUp]
    public void Setup() {
        testPlayer = new Player(
            new DynamicShape(new Vector2(0.45f, 0.1f), new Vector2(0.1f, 0.1f)),
            new Image("GalagaTests.Images.Player.png"));
        testPlayer2 = new Player(
            new DynamicShape(new Vector2(0.99f, 0.1f), new Vector2(0.1f, 0.1f)),
            new NoImage());
    }

    [Test]
    public void Test1() {
        Assert.AreEqual(1, 1);
    }


    [Test]
    public void TestSetMoveLeftAndRight() {
        testPlayer.SetMoveLeft(true);
        Assert.That(testPlayer.Shape.AsDynamicShape().Velocity.X, Is.EqualTo(-0.01f).Within(0.05f), "The velocity should be negative when moving left.");

        testPlayer.SetMoveLeft(false);
        Assert.That(testPlayer.Shape.AsDynamicShape().Velocity.X, Is.EqualTo(0.0f).Within(0.05f), "The velocity should be 0 when not moving.");

        testPlayer.SetMoveRight(true);
        Assert.That(testPlayer.Shape.AsDynamicShape().Velocity.X, Is.EqualTo(0.01f).Within(0.05f), "The velocity should be positive when moving right.");

        testPlayer.SetMoveRight(false);
        Assert.That(testPlayer.Shape.AsDynamicShape().Velocity.X, Is.EqualTo(0.0f).Within(0.05f), "The velocity should be 0 when not moving.");
    }


    [Test]
    public void TestMove() {
        testPlayer.SetMoveRight(true);
        testPlayer.Move();
        var newPositionRight = testPlayer.GetPosition();
        Assert.That(newPositionRight.X, Is.EqualTo(0.46f).Within(0.05f), "The player's position should move to the right when moving right.");

        testPlayer.SetMoveLeft(true);
        testPlayer.Move();
        var newPositionLeft = testPlayer.GetPosition();
        Assert.That(newPositionLeft.X, Is.EqualTo(0.45f).Within(0.05f), "The player's position should move back to the left when moving left.");
    }

    private bool ArePositionsEqual(Vector2 expected, Vector2 actual, float tolerance = 0.01f) {
        return Math.Abs(expected.X - actual.X) < tolerance && Math.Abs(expected.Y - actual.Y) < tolerance;
    }

    [Test]
    public void TestGetPosition() {
        var initialPosition = testPlayer.GetPosition();
        Assert.That(ArePositionsEqual(initialPosition, new Vector2(0.45f, 0.1f), 0.1f), "The initial position should be (0.45f, 0.1f).");

        testPlayer.SetMoveRight(true);
        testPlayer.Move();
        var newPosition = testPlayer.GetPosition();
        Assert.That(ArePositionsEqual(newPosition, new Vector2(0.46f, 0.1f), 0.1f), "The position should update when the player moves.");
    }

    [Test]
    public void TestKeyHandler() {
        testPlayer.KeyHandler(KeyboardAction.KeyPress, KeyboardKey.Left);
        Assert.That(testPlayer.Shape.AsDynamicShape().Velocity.X, Is.EqualTo(-0.01f).Within(0.05f), "The player should move left when left key is pressed.");

        testPlayer.KeyHandler(KeyboardAction.KeyRelease, KeyboardKey.Left);
        Assert.That(testPlayer.Shape.AsDynamicShape().Velocity.X, Is.EqualTo(0.0f).Within(0.05f), "The player should stop moving when left key is released.");

        testPlayer.KeyHandler(KeyboardAction.KeyPress, KeyboardKey.Right);
        Assert.That(testPlayer.Shape.AsDynamicShape().Velocity.X, Is.EqualTo(0.01f).Within(0.05f), "The player should move right when right key is pressed.");

        testPlayer.KeyHandler(KeyboardAction.KeyRelease, KeyboardKey.Right);
        Assert.That(testPlayer.Shape.AsDynamicShape().Velocity.X, Is.EqualTo(0.0f).Within(0.05f), "The player should stop moving when right key is released.");
    }


    [Test]
    public void TestPlayerMovementBoundaries() {
        float screenWidth = 1.0f;

        testPlayer.SetMoveLeft(true);
        testPlayer.Move();
        Assert.That(testPlayer.GetPosition().X, Is.EqualTo(0.45f - 0.01f).Within(0.05f), "The player should move left once.");

        testPlayer.SetMoveLeft(true);
        testPlayer.Move();
        Assert.That(testPlayer.GetPosition().X, Is.GreaterThanOrEqualTo(0.0f), "The player should not move beyond the left edge.");

        testPlayer.SetMoveRight(true);
        testPlayer.Move();
        Assert.That(testPlayer.GetPosition().X, Is.EqualTo(0.45f + 0.01f).Within(0.05f), "The player should move right once.");

        testPlayer.SetMoveRight(true);
        testPlayer.Move();
        float playerRightEdge = testPlayer.Shape.AsDynamicShape().Extent.X;
        Assert.That(testPlayer.GetPosition().X, Is.LessThanOrEqualTo(screenWidth - playerRightEdge), "The player should not move beyond the right edge.");
    }
}
