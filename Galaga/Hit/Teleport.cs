namespace Galaga.Hit;

using Galaga;

public class Teleport : IHitStrategy {
    public void Hit(Enemy enemy) {
        enemy.DecreaseHitpoints();
        enemy.Teleport(0.1f, 0.9f, 0.33f, 1.0f);
    }
}