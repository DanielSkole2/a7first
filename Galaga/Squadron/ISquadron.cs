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


public interface ISquadron {
    EntityContainer<Enemy> CreateEnemies(
        List<Image> enemyStrides,
        List<Image> enragedStrides,
        Func<IMovementStrategy> movement,
        Func<IHitStrategy> hit
    );
}