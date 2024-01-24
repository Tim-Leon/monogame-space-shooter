using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class FPS_Counter : UI
    {
        int Frames;
        int FramesTotal;
        SpriteFont font;
        private Sprite target;
        public FPS_Counter(Texture2D texture, SpriteFont spriteFont, Sprite target) : base(texture)
        {
            font = spriteFont;
        }
        float timer;
        public override void Update(GameTime gameTime)
        {
            timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timer >= 1000)
            {
                FramesTotal = Frames;
                Frames = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Frames++;

           // spriteBatch.DrawString(font, $"FPS{FramesTotal}", target.Position + new Vector2(Game1.ScreenWidth, Game1.ScreenHeight), Color.White); //FIX 
        }

    }
}
