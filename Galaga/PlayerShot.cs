namespace Galaga;

using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using System.Numerics;

public class PlayerShot : Entity {


    private static readonly Vector2 extent = new Vector2(0.008f, 0.021f);
    private static readonly Vector2 velocity = new Vector2(0.0f, 0.1f);

    
    public PlayerShot(Vector2 position, IBaseImage image) : base(new DynamicShape(position, extent, velocity), image) { }

    
    public static Vector2 GetExtent() => extent;
    public static Vector2 GetVelocity() => velocity;
}
