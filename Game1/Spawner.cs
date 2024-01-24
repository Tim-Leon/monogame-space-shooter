using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Spawner
    {
        public Asteroid Asteroid;
        public Enemy Enemy;
        public List<UI> UI;
        public float _time;
        public float waitTime = 3f;
        public int times = 0; // amount of enemy spawned
        private int MaxAmount = 10;
        private int EnemyAmount;
        public Texture2D EnemyHealthBar;
        public Texture2D EnemyHealthBarFrame;

        private ContentManager ContentManager;
        public Spawner(Enemy enemy, List<UI> _ui, Texture2D EnemyHealthBar, Texture2D EnemyHealthBarFrame, ContentManager contentManager)
        {
            this.ContentManager = contentManager;
            this.Enemy = enemy;
            this.UI = _ui;
            this.EnemyHealthBar = EnemyHealthBar;
            this.EnemyHealthBarFrame = EnemyHealthBarFrame;
        }

        public Enemy.EnemyDead enemyDead;
        public void Uppdate(GameTime gameTime, List<Sprite> sprites)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            EnemyAmount = 0;
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Enemy)
                {
                    EnemyAmount++;
                }
            }
            if(Game1.RNG.Next(0, 101) < 2)
            {
                #region Asteroid Spawner
                Asteroid asteroid = Asteroid.Clone() as Asteroid;
                asteroid._rotation = Game1.RNG.Next(0, 361);
                switch (Game1.RNG.Next(0, 4))
                {
                    case 0:
                        asteroid.Position = new Vector2(0, Game1.RNG.Next(0, Game1.GameHeight_Max_Y)); //left
                        break;
                    case 1:
                        asteroid.Position = new Vector2(Game1.RNG.Next(0, Game1.GameWidth_Max_X), 0); //top
                        break;
                    case 2:
                        asteroid.Position = new Vector2(Game1.GameWidth_Max_X, Game1.RNG.Next(0, Game1.GameHeight_Max_Y)); //right
                        break;
                    case 3:
                        asteroid.Position = new Vector2(Game1.RNG.Next(0, Game1.GameWidth_Max_X), Game1.GameHeight_Max_Y); //down 
                        break;
                }

                Vector2 diff = asteroid.Position - new Vector2(Game1.GameWidth_Max_X / 2, Game1.GameHeight_Max_Y / 2);
                asteroid._rotation = (float)Math.Atan2(diff.Y, diff.X) - (float)(Math.PI / 2); // rotate all asteroid to the center
                //asteroid.Position = new Vector2(Game1.RNG.Next(Game1.GameWidth_Min_X, Game1.GameWidth_Max_X), Game1.RNG.Next(Game1.GameHeight_Min_Y, Game1.GameHeight_Max_Y));
                sprites.Add(asteroid);
                #endregion
            }
            
            if (_time >= waitTime && EnemyAmount <= 10)
            {
                var ene = Enemy.Clone() as Enemy;
                ene.Position = new Vector2(Game1.RNG.Next(0, Game1.GameWidth_Max_X), Game1.RNG.Next(0, Game1.GameHeight_Max_Y));
                ene.EnemyDeadEvent += enemyDead;
                sprites.Add(ene);



                #region Add UI to added enemy
                var enemy = sprites[sprites.Count - 1] as Enemy;
                UI.Add(new EnemyHealthBar(EnemyHealthBar, EnemyHealthBarFrame, enemy, new Vector2(-EnemyHealthBar.Width / 2, enemy.Origin.Y), enemy.MaxHealth));
                #endregion
                _time = 0;
                times++;
            }
        }

        
    }
}