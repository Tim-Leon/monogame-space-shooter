using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Missile : Sprite
    {
        private float _timer;
        public Ship Player;
        public int damage;
        public Missile(Texture2D texture) : base(texture)
        {

        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < sprites.Count; i++)
            {
                if (this.IsTouching(sprites[i]) && (sprites[i] is Ship) && !(this.Parent is Ship) && this.IsRemoved == false)
                {
                    this.IsRemoved = true;
                    Player.Health -= damage;
                }
            }
            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - _rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - _rotation));
            Position += Direction * LinearVelocity;
            if (_timer > LifeSpan)
            {
                IsRemoved = true;
            }
            /*
            Position.X += FindDirection(Player.Position).X * (float)(gAameTime.ElapsedGameTime.TotalMilliseconds / 20) * LinearVelocity;
            Position.Y += FindDirection(Player.Position).Y * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 20) * LinearVelocity;
            */
            Vector2 diff = Position - Player.Position;
            _rotation = (float)Math.Atan2(diff.Y, diff.X) - (float)(Math.PI / 2);
        }
        private Vector2 FindDirection(Vector2 target)
        {
            Vector2 direction = target - Position;
            direction.Normalize();
            return direction;
        }
    }
}
