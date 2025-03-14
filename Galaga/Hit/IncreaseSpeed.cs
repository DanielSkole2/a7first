namespace Galaga.Hit;

using Galaga;

public class IncreaseSpeed : IHitStrategy {
    public void Hit(Enemy enemy) {
        enemy.DecreaseHitpoints();
        enemy.IncreaseSpeed(1.2f);
    }
}