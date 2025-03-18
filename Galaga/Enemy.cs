namespace Galaga;

using System;
using System.Numerics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Galaga.Hit;
using Galaga.Movement;

public class Enemy : Entity {

    private IHitStrategy hitStrategy;
    private IMovementStrategy movementStrategy;
    private int hitpoints;
    private IBaseImage normalImage;
    private IBaseImage enragedImage;
    private bool isEnraged = false;

    public Enemy(DynamicShape shape, IBaseImage image, IHitStrategy hitStrategy, IBaseImage enragedImage, IMovementStrategy movementStrategy) 
        : base(shape, image) {
        this.hitStrategy = hitStrategy;
        this.hitpoints = 3;
        this.normalImage = image;
        this.enragedImage = enragedImage;
        this.movementStrategy = movementStrategy;
    }

    public void TakeHit() {
        hitStrategy.Hit(this);
    }

    public void DecreaseHitpoints() {
        hitpoints--;
        if (hitpoints <= 0) {
            DeleteEntity();
        }
    }

    public int GetHitpoints() {
        return hitpoints;
    }

    public void IncreaseSpeed(float factor) {
        DynamicShape dynamicShape = Shape as DynamicShape;
        if (dynamicShape != null) {
            dynamicShape.Velocity *= factor;
        }
    }

    public void Teleport(float minX, float maxX, float minY, float maxY) {
        Random random = new Random();
        float newX = (float)(random.NextDouble() * (maxX - minX) + minX);
        float newY = (float)(random.NextDouble() * (maxY - minY) + minY);
        Shape.Position = new Vector2(newX, newY);
    }

    public void Enrage() {
        if (!isEnraged) {
            Image = enragedImage;
            IncreaseSpeed(1.5f);
            isEnraged = true;
        }
    }

    public void ResetImage() {
        Image = normalImage;
    }

    
    public void Update() {
        movementStrategy.Move(this); 
    }

    
    public void ScaleMovement(float factor) {
        movementStrategy.Scale(factor); 
    }
}