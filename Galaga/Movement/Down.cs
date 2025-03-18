namespace Galaga.Movement;

using System;
using System.Numerics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Galaga.Hit;
using Galaga;

public class Down : IMovementStrategy {
    private float speed;

    public Down(float speed) {
        this.speed = speed;
    }

    public void Scale(float factor) {
        this.speed *= factor;
    }

    public void Move(Enemy enemy) {
        DynamicShape shape = enemy.Shape as DynamicShape;
        if (shape != null) {
            shape.Position = new Vector2(shape.Position.X, shape.Position.Y - speed);
        }
    }
}
