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
    public class Sprite : ICloneable
    {
        protected Texture2D _texture;
        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public float _rotation;
        public Vector2 Position;
        public SpriteEffects Flip;
        public Vector2 Origin;

        public Vector2 Direction;
        public float RotationVelocity = 3f;
        public float LinearVelocity = 10f;

        public Sprite Parent;

        public float LifeSpan = 0f;
        public bool IsRemoved = false;

        public Color Color;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X -  (_texture.Width / 2), (int)Position.Y - (_texture.Height / 2), _texture.Width, _texture.Height);
            }
        }

        public bool IsMoving;

        public Sprite(Texture2D texture)
        {
            _texture = texture;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            Flip = SpriteEffects.None;
            Color = Color.White;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color, _rotation, Origin, 1, Flip, 0);
        }
        #region Collision
        protected bool IsTouching(Sprite sprite)
        {
            if (sprite != null)
            {
                return Rectangle.Intersects(sprite.Rectangle);
            }
            return false;
        }

        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Left &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left < sprite.Rectangle.Right &&
              this.Rectangle.Right > sprite.Rectangle.Right &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Top &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top < sprite.Rectangle.Bottom &&
              this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }
        #endregion


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
