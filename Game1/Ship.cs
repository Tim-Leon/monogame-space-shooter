using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.UI_Content;
using Microsoft.Xna.Framework.Audio;

namespace Game1
{
    public class Ship : Sprite
    {
        public int Ammo
        {
            get
            {
                return ammo;
            }
            set
            {
                if (value <= maxAmmo)
                {
                    ammo = value;
                }
                else
                {
                    ammo = maxAmmo;
                }
            }
        }
        private int maxAmmo = 100;
        private int ammo = 50;
        private int _Helath;
        public int MaxHealth = 200;
        public int Health
        {
            get { return _Helath; }
            set
            {
                if (value >= MaxHealth)
                {
                    _Helath = MaxHealth;
                }
                else if (value <= 0)
                {
                    Death();
                    _Helath = 0;
                }
                else
                {
                    _Helath = value;
                }
            }
        }
        private void Death()
        {
            PlayerDeadEvent(this, new Button.GameStateEventArgs(Game1.GameState.Dead));
            this.IsRemoved = true;
        }

        public delegate void PlayerDead(object sender, Button.GameStateEventArgs e);
        public event PlayerDead PlayerDeadEvent;

        public float MaxBoost = 5000;
        public float Boost
        {
            get { return _Boost; }
            set
            {
                if (value <= 0)
                {
                    _Boost = 0;
                }
                else if (value >= MaxBoost)
                {
                    _Boost = MaxBoost;
                }
                else
                {
                    _Boost = value;
                }
            }
        }
        private float _Boost;
        public Bullet Bullet;
        public SoundEffect BulletSound;
        public float Rotation { get { return _rotation; } }

        public Bomb Bomb;
        public Vector2 position // remove
        {
            get { return Position; }
        }

        public Ship(Texture2D texture, SoundEffect BulletSound) : base(texture)
        {
            this.BulletSound = BulletSound;
            _Boost = MaxBoost;
            _Helath = MaxHealth;
        }
        internal float timer = 0;
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Position += Direction * LinearVelocity;
            /*
            #region Ship Collision
            for (int i = 0; i < sprites.Count; i++)
            {
                if (IsTouching(sprites[i]) && sprites[i] is Enemy)
                {
                    this.Health -= 40;
                    var enemy = sprites[i] as Enemy;
                    enemy.Health -= 100;
                }
            }
            #endregion
            */
            
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(timer >= 2)
            {
                this.Ammo++;
                timer = 0;
            }

            #region Controls
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            if (_currentKey.IsKeyDown(Keys.A))
            {
                _rotation -= MathHelper.ToRadians(RotationVelocity);
            }
            else if (_currentKey.IsKeyDown(Keys.D))
            {
                _rotation += MathHelper.ToRadians(RotationVelocity);
            }

            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - _rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - _rotation));

            if (_currentKey.IsKeyDown(Keys.W))
            {
                if (Boost > 0)
                {
                    Boost -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Position += Direction * (LinearVelocity * 0.5f);
                }
            }
            else
            {
                Boost += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            //Debug.WriteLine(Boost);
            if (_currentKey.IsKeyDown(Keys.Space) && _previousKey.IsKeyUp(Keys.Space) && (Ammo > 0))
            {
                BulletSound.Play();
                AddBullet(sprites);
                Ammo--;
            }
            else if (_currentKey.IsKeyDown(Keys.G) && _previousKey.IsKeyUp(Keys.G) && (Ammo > 10))
            {
                Bomb.Parent = this;
                Bomb.Bullet.Parent = this;
                Bomb.Bullet.damage = 20;
                Bomb.Position = this.Position;
                sprites.Add((Bomb)Bomb.Clone());
                ammo -= 10;
            }

            if (_currentKey.IsKeyDown(Keys.Tab) && _previousKey.IsKeyUp(Keys.Tab))
            {
                if(!BuyMenu.IsVisible)
                BuyMenu.Show();
                else
                {
                    BuyMenu.Hide();
                }
            }


            #endregion
            Position.X = MathHelper.Clamp(Position.X, Game1.GameWidth_Min_X, Game1.GameWidth_Max_X);
            Position.Y = MathHelper.Clamp(Position.Y, Game1.GameHeight_Min_Y, Game1.GameHeight_Max_Y);

        }
        private void MouseControl() //DO NOT WORK
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 diff = mouseState.Position.ToVector2() - this.Position;
            _rotation = (float)Math.Atan2(diff.Y, diff.X);
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Direction = this.Direction;
            bullet.Position = this.Position + Direction * 100;
            bullet.LinearVelocity = this.LinearVelocity * 2;
            bullet._rotation = this.Rotation;
            bullet.LifeSpan = 2f;
            bullet.Parent = this;
            bullet.damage = 20;
            sprites.Add(bullet);
        }
    }
}