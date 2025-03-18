namespace Galaga.Squadron;

using System;
using System.Collections.Generic;
using System.Numerics;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga.Hit;
using Galaga.Movement;

public class SimpleGridSquadron : ISquadron {
    public EntityContainer<Enemy> CreateEnemies(
        List<Image> enemyStrides,
        List<Image> enragedStrides,
        Func<IMovementStrategy> movement,
        Func<IHitStrategy> hit) {
        
        EntityContainer<Enemy> enemies = new EntityContainer<Enemy>(10);
        int rows = 2, cols = 5;
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                IHitStrategy hitStrategy = hit();
                IMovementStrategy moveStrategy = movement();
                enemies.AddEntity(new Enemy(
                    new DynamicShape(new Vector2(0.1f + j * 0.1f, 0.7f + i * 0.1f), new Vector2(0.1f, 0.1f)),
                    new ImageStride(80, enemyStrides),
                    hitStrategy,
                    new ImageStride(80, enragedStrides),
                    moveStrategy
                ));
            }
        }
        return enemies;
    }
}
