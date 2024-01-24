using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Bomb : Sprite
    {
        public Bullet Bullet;
        public Bomb (Texture2D explotionTex) : base(explotionTex)
        {
        }
        float timer;
        float colorTimer;
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            base.Update(gameTime, sprites);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            colorTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Color = new Color(colorTimer/100, 0, 0, 1);
            if(colorTimer >= 255)
            {
                colorTimer = 0;
            }
            if (timer > 5)
            {
                for (int i = 0; i < 30; i++)
                {
                    Bullet.Position = this.Position;
                    Bullet._rotation = i * 30;
                    Bullet.LifeSpan = 3f;
                    Bullet.Parent = this.Parent;
                    sprites.Add((Bullet)Bullet.Clone());
                }
                this.IsRemoved = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        { 
            base.Draw(spriteBatch);
        }
    }
}
