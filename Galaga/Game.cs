namespace Galaga; 

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

public class Game : DIKUGame {

    private Player player;
    private EntityContainer<Enemy> enemies;
    private EntityContainer<PlayerShot> playerShots;
    private IBaseImage playerShotImage;
    private GameEventBus gameEventBus;
    private AnimationContainer enemyExplosions;
    private List<Image> explosionStrides;
    private const int EXPLOSION_LENGTH_MS = 500;

    public Game(WindowArgs windowArgs) : base(windowArgs) {

        player = new Player(
            new DynamicShape(new Vector2(0.45f, 0.1f), new Vector2(0.1f, 0.1f)),
            new Image("Galaga.Assets.Images.Player.png")
        );


        List<Image> images = ImageStride.CreateStrides(4, "Galaga.Assets.Images.BlueMonster.png");
        const int numEnemies = 8;
        enemies = new EntityContainer<Enemy>(numEnemies);
        for (int i = 0; i < numEnemies; i++) {
            enemies.AddEntity(new Enemy(
                new DynamicShape(new Vector2(0.1f + (float) i * 0.1f, 0.9f), new Vector2(0.1f, 0.1f)),
                new ImageStride(80, images)
            ));
        }


        playerShots = new EntityContainer<PlayerShot>();
        playerShotImage = new Image("Galaga.Assets.Images.BulletRed2.png");

        gameEventBus = new GameEventBus();
        gameEventBus.Subscribe<AddExplosionEvent>(AddExplosion);

        enemyExplosions = new AnimationContainer(numEnemies);
        explosionStrides = ImageStride.CreateStrides(8, "Galaga.Assets.Images.Explosion.png");
    }


    ~Game() {
        gameEventBus.Unsubscribe<AddExplosionEvent>(AddExplosion);
    }

    public override void Render(WindowContext context) {
        player.RenderEntity(context);
        enemies.RenderEntities(context);
        playerShots.RenderEntities(context);
        enemyExplosions.RenderAnimations(context);
    }

    public override void Update() {
        player.Move();
        IterateShots();


        foreach (var enemy in enemies) {
            if (enemy is Enemy e) {
                float horizontalMovement = (float) Math.Sin(e.Shape.Position.Y * 0.1f) * 0.02f;
                e.Shape.Position = new Vector2(e.Shape.Position.X + horizontalMovement, e.Shape.Position.Y);
            }
        }


        gameEventBus.ProcessEvents();
    }

    private void IterateShots() {
        playerShots.Iterate(shot => {
            DynamicShape shotShape = shot.Shape as DynamicShape;
            shotShape.Position += PlayerShot.GetVelocity();

            if (shotShape.Position.Y > 1.0f) {
                shot.DeleteEntity();
            } else {
                enemies.Iterate(enemy => {
                    if (enemy is Enemy e) {
                        var collision = CollisionDetection.Aabb(shotShape, e.Shape);
                        if (collision.Collision) {
                            shot.DeleteEntity();
                            e.DeleteEntity();


                            gameEventBus.RegisterEvent(new AddExplosionEvent(e.Shape.Position, new Vector2(0.1f, 0.1f)));
                        }
                    }
                });
            }
        });
    }


    public void AddExplosion(AddExplosionEvent addExplosionEvent) {

        var explosionShape = new StationaryShape(addExplosionEvent.Position, addExplosionEvent.Extent);
        var explosionAnimation = new ImageStride(EXPLOSION_LENGTH_MS / 8, explosionStrides);
        enemyExplosions.AddAnimation(explosionShape, EXPLOSION_LENGTH_MS, explosionAnimation);
    }

    public override void KeyHandler(KeyboardAction action, KeyboardKey key) {
        player.KeyHandler(action, key);

        if (action == KeyboardAction.KeyRelease && key == KeyboardKey.Space) {

            Vector2 shotPosition = new Vector2(player.GetPosition().X + 0.045f, player.GetPosition().Y + 0.05f);
            playerShots.AddEntity(new PlayerShot(shotPosition, playerShotImage));
        }
    }
}


public class EnemyExplosion : Entity {
    public EnemyExplosion(StationaryShape shape, ImageStride animation)
        : base(shape, animation) {
    }


}
