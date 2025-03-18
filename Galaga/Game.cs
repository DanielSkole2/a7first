namespace Galaga {

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
    using Galaga.Squadron;

    public class Game : DIKUGame {

        private Player player;
        private EntityContainer<Enemy> enemies;
        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;
        private GameEventBus gameEventBus;
        private AnimationContainer enemyExplosions;
        private List<Image> explosionStrides;
        private const int EXPLOSION_LENGTH_MS = 500;

        private ISquadron currentSquadron;  
        private IMovementStrategy currentMovement;  

        public Game(WindowArgs windowArgs) : base(windowArgs) {

            player = new Player(
                new DynamicShape(new Vector2(0.45f, 0.1f), new Vector2(0.1f, 0.1f)),
                new Image("Galaga.Assets.Images.Player.png")
            );

            
            List<Image> images = ImageStride.CreateStrides(4, "Galaga.Assets.Images.BlueMonster.png");
            List<Image> enragedImages = ImageStride.CreateStrides(4, "Galaga.Assets.Images.RedMonster.png");

            
            currentSquadron = new SimpleGridSquadron();  
            currentMovement = new Down(0.0003f);         

            
            enemies = currentSquadron.CreateEnemies(images, enragedImages, 
                () => currentMovement,  
                () => new Enrage()       
            );

            playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image("Galaga.Assets.Images.BulletRed2.png");

            gameEventBus = new GameEventBus();
            gameEventBus.Subscribe<AddExplosionEvent>(AddExplosion);

            enemyExplosions = new AnimationContainer(10);
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
            
            
            enemies.Iterate(enemy => {
                if (enemy is Enemy e) {
                    
                    currentMovement.Move(e);  
                }
            });

            IterateShots();
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

                                e.TakeHit();
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

            
            if (action == KeyboardAction.KeyPress) {
                if (key == KeyboardKey.F1) {
                    
                    currentSquadron = new SimpleGridSquadron();
                    enemies = currentSquadron.CreateEnemies(
                        ImageStride.CreateStrides(4, "Galaga.Assets.Images.BlueMonster.png"),
                        ImageStride.CreateStrides(4, "Galaga.Assets.Images.RedMonster.png"),
                        () => currentMovement,
                        () => new Enrage()
                    );
                }
                else if (key == KeyboardKey.F2) {
                    
                    currentSquadron = new VerticalLineSquadron();
                    enemies = currentSquadron.CreateEnemies(
                        ImageStride.CreateStrides(4, "Galaga.Assets.Images.BlueMonster.png"),
                        ImageStride.CreateStrides(4, "Galaga.Assets.Images.RedMonster.png"),
                        () => currentMovement,
                        () => new Enrage()
                    );
                }
                else if (key == KeyboardKey.F3) {
                    
                    currentSquadron = new RandomSquadron();
                    enemies = currentSquadron.CreateEnemies(
                        ImageStride.CreateStrides(4, "Galaga.Assets.Images.BlueMonster.png"),
                        ImageStride.CreateStrides(4, "Galaga.Assets.Images.RedMonster.png"),
                        () => currentMovement,
                        () => new Enrage()
                    );
                }

                
                if (key == KeyboardKey.F4) {
                    currentMovement = new Down(0.0003f);
                }
                else if (key == KeyboardKey.F5) {
                    currentMovement = new ZigZagDown(); 
                }
                else if (key == KeyboardKey.F6) {
                    currentMovement = new NoMove();
                }
            }
        }
    }

    public class EnemyExplosion : Entity {
        public EnemyExplosion(StationaryShape shape, ImageStride animation)
            : base(shape, animation) {
        }
    }
}