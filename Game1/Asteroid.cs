using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Asteroid : Sprite
    {
        public int Damage = 20;
        public int Health
        {
            get
            {
                return _helath;
            }
            set
            {
                if (value <= 0)
                {
                    this.IsRemoved = true;
                }
                _helath = value;
            }
        }
        private int _helath = 30;
        public Asteroid(Texture2D texture) : base(texture)
        {

        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - _rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - _rotation));
            Position += Direction * LinearVelocity;
            for (int i = 0; i < sprites.Count; i++)
            {
                if (this.IsTouching(sprites[i]))
                {
                    if (sprites[i] is Enemy && this.IsRemoved == false)
                    {
                        var obj = sprites[i] as Enemy;
                        obj.Health -= Damage;
                        this.IsRemoved = true;
                    }
                    else if (sprites[i] is Ship && this.IsRemoved == false)
                    {
                        var obj = sprites[i] as Ship;
                        obj.Health -= Damage;
                        this.IsRemoved = true;
                    }
                    else if (sprites[i] is Bullet && this.IsRemoved == false)
                    {
                        var obj = sprites[i] as Bullet;
                        this.Health -= obj.damage;
                        sprites[i].IsRemoved = true;
                    }
                    else if (sprites[i] is Missile && this.IsRemoved == false)
                    {
                        var obj = sprites[i] as Missile;
                        this.Health -= obj.damage;
                        sprites[i].IsRemoved = true;
                    }
                    else if(sprites[i] is Asteroid && this.IsRemoved == false)
                    {
                        this.IsRemoved = true;
                    }
                }
            }

            if (this.Position.Y - _texture.Height > Game1.GameHeight_Max_Y || //remove if it is outside the game
                this.Position.Y + _texture.Height < Game1.GameHeight_Min_Y ||
                this.Position.X - _texture.Width > Game1.GameWidth_Max_X ||
                this.Position.X + _texture.Width < Game1.GameWidth_Min_X)
                this.IsRemoved = true;


                base.Update(gameTime, sprites);
        }
    }
}
