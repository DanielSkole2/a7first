namespace Galaga.Movement {
    using System;
    using System.Numerics;
    using DIKUArcade.Entities;
    using DIKUArcade.Graphics;
    using Galaga.Hit;
    using Galaga;

    public class ZigZagDown : IMovementStrategy {
        private float speed = 0.0003f; 
        private float amplitude = 0.05f;  
        private float period = 0.045f;    
        private float initialY;

        public ZigZagDown() {
            this.initialY = 1.0f;  
        }

        public void Scale(float factor) {
            speed *= factor;
        }

        public void Move(Enemy enemy) {
            DynamicShape shape = enemy.Shape as DynamicShape;
            if (shape != null) {
               
                float deltaY = shape.Position.Y - initialY;

                
                float xOffset = amplitude * (float)Math.Sin((2 * Math.PI * deltaY) / period);

                
                xOffset = Math.Clamp(shape.Position.X + xOffset, 0.0f, 0.9f) - shape.Position.X;

                
                shape.Position = new Vector2(shape.Position.X + xOffset, shape.Position.Y - speed);
            }
        }
    }
}
