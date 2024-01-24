using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Game1
{
    public class Bullet : Sprite
    {
        private float _timer;
        public int damage;

        public Bullet(Texture2D texture) : base(texture)
        {
            // Direction = this.Parent;
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - _rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - _rotation));
            #region Collision
            for (int i = 0; i < sprites.Count; i++)
            {
                if (this.IsTouching(sprites[i]) && (sprites[i] is Missile) && this.IsRemoved == false) //Missile
                {
                    sprites[i].IsRemoved = true;
                    this.IsRemoved = true;
                }
                if (this.IsTouching(sprites[i]) && (sprites[i] is Ship) && !(this.Parent is Ship) && this.IsRemoved == false) //Player
                {
                    var ship = sprites[i] as Ship;
                    ship.Health -= damage;
                    this.IsRemoved = true;
                }

                if (this.IsTouching(sprites[i]) && (sprites[i] is Enemy) && !(this.Parent is Enemy) && this.IsRemoved == false) //Enemy
                {
                    var enemy = sprites[i] as Enemy;
                    enemy.Health -= damage;
                    this.IsRemoved = true;
                }
            }
            #endregion

            if (_timer > LifeSpan)
            {
                IsRemoved = true;
            }
            Position += Direction * LinearVelocity;
        }
    }
}