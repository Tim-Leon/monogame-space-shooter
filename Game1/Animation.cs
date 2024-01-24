using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    public static class Animator
    {
        public static Dictionary<string, Animation> Animations;
        public static List<Animation> PlayingAnimations;

        public static void Constructor()
        {
            Animations = new Dictionary<string, Animation>();
            PlayingAnimations = new List<Animation>();
        }

        public static void Add(Animation animation)
        {
            Animations.Add(animation.Name, animation);
        }


        public static void Play(string name, Rectangle rectangle)
        {
            var animation = Animations[name].Clone() as Animation;
            animation.Play(rectangle, null);
            PlayingAnimations.Add(animation);
        }

        public static void Play(string name, Rectangle rectangle, float rotation)
        {
            var animation = Animations[name].Clone() as Animation;
            animation.Play(rectangle, rotation);
            PlayingAnimations.Add(animation);
        }

        public static void Play(string name, Sprite sprite)
        {
            var animation = Animations[name].Clone() as Animation;
            if(sprite.IsMoving)
            animation.Velocity = sprite.LinearVelocity;
            Rectangle rec = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y, sprite.Rectangle.Width, sprite.Rectangle.Height);
            animation.Play(rec, sprite._rotation);
            PlayingAnimations.Add(animation);
        }


    }

    public class Animation
    {
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(Position.ToPoint(), Size.ToPoint());
            }
        }
        public Vector2 Position;
        public Vector2 Size;
        public string Name;
        public int CurrentTexture
        {
            get { return _currentTexture; }
            set
            {
                if (value > Textures.Count- 1)
                {
                    _currentTexture = 0;
                }
                else
                {
                    _currentTexture = value;
                }
            }
        }

        private int _currentTexture;
        public List<Texture2D> Textures;
        public float FrameTime;
        private float Time = 0;
        public bool IsPlaying = false;
        public float Rotation = 0;
        public Vector2 Direction;
        public float Velocity;
        public bool IsVisible = true;
        public Vector2 Origin;
        public Animation(List<Texture2D> textures,string name, float Frametime)
        {
            this.Name = name;
            this.FrameTime = Frametime;
            Textures = textures;
        }
        public void Play(Rectangle rectangle,float? rotation)
        {
            Position = rectangle.Location.ToVector2();
            Size = rectangle.Size.ToVector2();
            if (rotation != null)
                Rotation = (float)rotation;
            IsPlaying = true;
        }
        public void Add(Texture2D texture)
        {
            Textures.Add(texture);
        }

        public void Show()
        {
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Uppdate(GameTime gameTime)
        {
            if (IsPlaying)
            {
                Origin = new Vector2(Textures[CurrentTexture].Width / 2, Textures[CurrentTexture].Height / 2);
                Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - Rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - Rotation));
                Position += Direction * Velocity;
                if (CurrentTexture == Textures.Count - 1)
                {
                    IsPlaying = false;
                }
                Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Time >= FrameTime)
                {
                    CurrentTexture++;
                    Time = 0;
                }
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(Textures[CurrentTexture], Rectangle, null, Color.White, Rotation, Origin, SpriteEffects.None, 0);
            }
        }
    }
}
