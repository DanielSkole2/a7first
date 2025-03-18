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

public class RandomSquadron : ISquadron {
    public EntityContainer<Enemy> CreateEnemies(
        List<Image> enemyStrides,
        List<Image> enragedStrides,
        Func<IMovementStrategy> movement,
        Func<IHitStrategy> hit) {
        
        EntityContainer<Enemy> enemies = new EntityContainer<Enemy>(10);
        Random rand = new Random();
        for (int i = 0; i < 10; i++) {
            IHitStrategy hitStrategy = hit();
            IMovementStrategy moveStrategy = movement();
            float randomX = (float)(rand.NextDouble() * 0.8 + 0.1);  
            float randomY = (float)(rand.NextDouble() * 0.5 + 0.5); 
            enemies.AddEntity(new Enemy(
                new DynamicShape(new Vector2(randomX, randomY), new Vector2(0.1f, 0.1f)),
                new ImageStride(80, enemyStrides),
                hitStrategy,
                new ImageStride(80, enragedStrides),
                moveStrategy
            ));
        }
        return enemies;
    }
}
