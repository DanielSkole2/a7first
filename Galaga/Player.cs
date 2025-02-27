namespace Galaga;

using System.Numerics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.GUI;
using DIKUArcade.Input;

public class Player : Entity {


    private float moveLeft = 0.0f;
    private float moveRight = 0.0f;
    private const float MOVEMENT_SPEED = 0.01f;

    public Player(DynamicShape shape, IBaseImage image) : base(shape, image) { }


    private void UpdateVelocity() {
        DynamicShape dynamicShape = Shape as DynamicShape;
        if (dynamicShape != null) {
            dynamicShape.Velocity = new Vector2(moveRight + moveLeft, 0.0f);
        }
    }


    public void SetMoveLeft(bool val) {
        if (val) {
            moveLeft = -MOVEMENT_SPEED;
        } else {
            moveLeft = 0.0f;
        }
        UpdateVelocity();
    }


    public void SetMoveRight(bool val) {
        if (val) {
            moveRight = MOVEMENT_SPEED;
        } else {
            moveRight = 0.0f;
        }
        UpdateVelocity();
    }


    public void Move() {
        DynamicShape dynamicShape = Shape as DynamicShape;
        if (dynamicShape != null) {
            float newX = dynamicShape.Position.X + dynamicShape.Velocity.X;

            if (newX < 0.0f) {
                newX = 0.0f;
            } else if (newX > 1.0f - dynamicShape.Extent.X) {
                newX = 1.0f - dynamicShape.Extent.X;
            }

            dynamicShape.Position = new Vector2(newX, dynamicShape.Position.Y);
        }
    }

    // Method to return the player's position
    public Vector2 GetPosition() {
        DynamicShape dynamicShape = Shape as DynamicShape;
        return dynamicShape?.Position ?? Vector2.Zero;
    }

    // KeyHandler method to handle keyboard input
    public void KeyHandler(KeyboardAction action, KeyboardKey key) {
        if (action == KeyboardAction.KeyPress) {
            if (key == KeyboardKey.Left) {
                SetMoveLeft(true);
            } else if (key == KeyboardKey.Right) {
                SetMoveRight(true);
            } else if (key == KeyboardKey.Space) {
                // TODO: Fire a shot (implement this in the Game class)
            }
        } else if (action == KeyboardAction.KeyRelease) {
            if (key == KeyboardKey.Left) {
                SetMoveLeft(false);
            } else if (key == KeyboardKey.Right) {
                SetMoveRight(false);
            }
        }
    }
}
