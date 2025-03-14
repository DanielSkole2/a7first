namespace Galaga.Hit;

using Galaga;

public class Enrage : IHitStrategy {
    public void Hit(Enemy enemy) {
        enemy.DecreaseHitpoints();
        if (enemy.GetHitpoints() <= 1) {
            enemy.Enrage();
        }
    }
}