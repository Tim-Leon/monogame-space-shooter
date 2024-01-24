using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Audio;

namespace Game1
{
    public enum TurningDirection { Right, Left }
    public class Enemy : Sprite
    {
        public TurningDirection TurningDirection;
        public Missile Missile;
        public Bullet Bullet;
        private int Seed = 0;
        public int Health
        {
            get { return _Health; }
            set
            {
                if (value <= 0)
                {
                    _Health = value;
                    Dead();
                }
                else if (value > MaxHealth)
                {
                    _Health = MaxHealth;
                }
                else
                {
                    _Health = value;
                }
            }
        }
        public int MaxHealth = 100;
        private int _Health = 100;
        public int Level;
        public Ship Player;
        float _time;


        private SoundEffect BulletSound;
        private SoundEffect MissleLaunch;
        private SoundEffect MissileExplosion;


        public delegate void EnemyDead(object sender, EventArgs e);
        public event EnemyDead EnemyDeadEvent;
        private void Dead()
        {
            Game1.Score += 20;
            //Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            //Animator.Play("Explosion", rec, _rotation);
           
            EnemyDeadEvent(this, EventArgs.Empty);
            Animator.Play("Explosion", this);
            this.IsRemoved = true;
        }

        public Enemy(Texture2D texture, Vector2 position, int Level, Ship Player, SoundEffect BulletSound, SoundEffect MissileExplosion, SoundEffect MissleLaunch) : base(texture)
        {
            this.Position = position;
            this.BulletSound = BulletSound;
            this.Player = Player;
            this.Level = Level;
            this.Health = (int)(Math.Pow(Convert.ToDouble(Level), 1.5) + 100);
            this.MissileExplosion = MissileExplosion;
            this.MissleLaunch = MissleLaunch;

        }
        public void AssignEvent(EnemyDead eEvent)
        {
            EnemyDeadEvent += eEvent;
        }
        private int rn = Game1.RNG.Next(3, 5);
        private Vector2 LastPosition;
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - _rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - _rotation));
            if (Vector2.Distance(Position, Player.Position) > 400)
            {
                Position += Direction * LinearVelocity;
            }

            Vector2 diff = Position - Player.Position;
            _rotation = (float)Math.Atan2(diff.Y, diff.X) - (float)(Math.PI/2);
            if(_rotation < 0)
            {

            }

            /*
            if (Vector2.Distance(Position, Player.Position) > 75)
            {
                Position.X += FindDirection(Player.Position).X * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 20) * LinearVelocity;
                Position.Y += FindDirection(Player.Position).Y * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 20) * LinearVelocity;
            }
            */
            #region Shoot    
            Random RNG = new Random(Seed);
            if (_time >= rn)
            {

                if (RNG.Next(0, 10) <= 3)
                {
                    //MissleLaunch.Play();
                    SoundManager.Play("MissleLaunch", this);
                    AddMissile(sprites);
                }
                else
                {
                    //BulletSound.Play();
                    SoundManager.Play("PewPewSound", this);
                    AddBullet(sprites);
                }
                rn = RNG.Next(3, 5);
                Seed++;
                _time = 0;
            }
            #endregion

            

            if (LastPosition == Position)
            {
                IsMoving = false;
            }
            else
            {
                IsMoving = true;
            }

            LastPosition = Position;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        private Vector2 FindDirection(Vector2 target)
        {
            Vector2 direction = target - Position;
            direction.Normalize();
            return direction;
        }
        private void AddMissile(List<Sprite> sprites)
        {
            var missile = Missile.Clone() as Missile;
            missile.Direction = this.Direction;
            missile.Position = this.Position;
            missile.LinearVelocity = this.LinearVelocity * 2;
            missile._rotation = this._rotation;
            missile.LifeSpan = 10;
            missile.Parent = this;
            missile.Player = Player;
            missile.damage = 30;
            sprites.Add(missile);
        }



        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Direction = this.Direction;
            bullet.Position = this.Position;
            bullet._rotation = this._rotation;
            bullet.LinearVelocity = this.LinearVelocity * 2;
            bullet.LifeSpan = 5f;
            bullet.Parent = this;
            bullet.damage = 10;
            sprites.Add(bullet);
        }
    }
}