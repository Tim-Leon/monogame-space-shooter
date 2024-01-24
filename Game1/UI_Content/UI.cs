using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class UI
    {
        protected Texture2D _texture;
        protected float _rotation;
        public Vector2 Position;
        public Vector2 Origin;
        public bool IsRemoved;
        public SpriteFont Font;
        public bool IsVisible = true;
        public UI(Texture2D texture)
        {
            this._texture = texture;
        }
        public UI(SpriteFont font)
        {
            this.Font = font;
        }

        public UI(Texture2D texture, SpriteFont spriteFont)
        {
            this._texture = texture;
            this.Font = spriteFont;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Update(GameTime gameTime, MouseState mouse)
        {

        }
        public virtual void Clicked(MouseState MouseState)
        {
            
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(IsVisible)
            spriteBatch.Draw(_texture, Position, null, Color.White, _rotation, Origin, 1, SpriteEffects.None, 0);
        }
        public virtual void Draw(SpriteBatch spriteBatch, string text)
        {
            if(IsVisible)
            spriteBatch.DrawString(Font, text, Position, Color.White, _rotation, Origin, 1, SpriteEffects.None, 0);
        }

    }
}
