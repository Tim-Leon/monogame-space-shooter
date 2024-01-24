using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.CodeDom;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using Game1.UI_Content;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static readonly Random RNG = new Random(Guid.NewGuid().GetHashCode());
        public static int ScreenWidth;
        public static int ScreenHeight;
        private Camera _camera;
        private Ship player;
        private KeyboardState _previousKey;
        private KeyboardState _currentKey;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D _background;
        private Texture2D _gameMenuBackground;
        private Spawner spawner;
        public SoundEffect soundEffect;
        public enum GameState { StartMenu, Scoreboard, GameRunning, Dead };
        public static GameState gameState = GameState.StartMenu;
        //min/max map size
        public static int GameWidth_Max_X = 10000, GameHeight_Max_Y = 10000, GameWidth_Min_X = 0, GameHeight_Min_Y = 0;
        public List<Sprite> _sprites;
        private List<UI> _UIs;
        private List<UI> StartMenu;
        private List<UI> EndScreen;
        public SpriteFont basicFont;
        public static int Score;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        public void GameStateEvent(object sender, Button.GameStateEventArgs e)
        {
            Debug.WriteLine("Click");
            Debug.WriteLine(e.state);
            Debug.WriteLine(e.GetType().FullName.ToString());
            if (e != null)
            {
                if (e.state != gameState && e.state != null)
                {
                    gameState = e.state;
                    this.LoadContent();
                }
            }
        }

        AmmoCrate AmmoCrate;
        HealthKit HealthKit;

        public void EnemyDead(object sender, EventArgs e)
        {
            int r = RNG.Next(0, 101);
            var enemy = (Enemy)sender;
            if (r < 20)
            {
                var he = HealthKit.Clone() as HealthKit;
                he.Position = enemy.Position;
                Debug.WriteLine("HP");
                _sprites.Add(he);
            }
            else if (r  > 80)
            {
                var am = AmmoCrate.Clone() as AmmoCrate;
                am.Position = enemy.Position;
                Debug.WriteLine("AMMO");
                _sprites.Add(am);
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = false;
            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
        }

        private Texture2D EndScene;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new Camera();
            Score = 0;
            basicFont = Content.Load<SpriteFont>("basicFont");
            SoundManager.SoundEffects = new Dictionary<string, SoundEffect>();
            Animator.Constructor();
            AmmoCrate = new AmmoCrate(Content.Load<Texture2D>("AmmoCrate"), Vector2.One);
            HealthKit = new HealthKit(Content.Load<Texture2D>("Healthkit"), Vector2.One);
            switch (gameState)
            {
                case GameState.StartMenu:
                    #region Start Menu
                    _gameMenuBackground = Content.Load<Texture2D>("GameMenuBackground");
                    Vector2 buttonSize = new Vector2(400, 150);
                    StartMenu = new List<UI>()
                    {
                new Button(CreateTexture(GraphicsDevice, (int)buttonSize.X, (int)buttonSize.Y, Color.White), Content.Load<SpriteFont>("basicFont"), "START", buttonSize, new Vector2(0,-300), Color.White, new Button.GameStateEventArgs(GameState.GameRunning)),
                new Button(CreateTexture(GraphicsDevice, (int)buttonSize.X, (int)buttonSize.Y, Color.White), Content.Load<SpriteFont>("basicFont"),"Scoreboard", buttonSize, new Vector2(0,300), Color.White, new Button.GameStateEventArgs(GameState.Scoreboard)),
            };
                    for (int i = 0; i < StartMenu.Count; i++)
                    {
                        if (StartMenu[i] is Button)
                        {
                            var btn = StartMenu[i] as Button;
                            btn.AssignEvent(GameStateEvent);
                        }
                    }
                    #endregion
                    break;
                case GameState.GameRunning:
                    #region Load Content
                    var shipTexture = Content.Load<Texture2D>("Ship");
                    var bulletSound = Content.Load<SoundEffect>("PewPewSound");
                    var missileExplosion = Content.Load<SoundEffect>("MissileExplosion");
                    var missleLaunch = Content.Load<SoundEffect>("MissleLaunch");
                    var barFrame = Content.Load<Texture2D>("BoostBarFrame");
                    var boostBar = Content.Load<Texture2D>("BoostBar");
                    var healthBar = Content.Load<Texture2D>("HealthBar");
                    _background = Content.Load<Texture2D>("Background");
                    #endregion

                    #region Create Player
                    player = new Ship(shipTexture, bulletSound)
                    {
                        Position = new Vector2(100, 100),
                        Bullet = new Bullet(Content.Load<Texture2D>("Bullet")),
                        Bomb = new Bomb(Content.Load<Texture2D>("Dynamite_SU")) { Bullet = new Bullet(Content.Load<Texture2D>("Bullet")) }
                    };
                    player.PlayerDeadEvent += GameStateEvent;
                    _sprites = new List<Sprite>()
            {
                player
            };
                    #endregion
                    #region SoundManager

                    SoundManager.Target = player;
                    SoundManager.Add(Content.Load<SoundEffect>("MissleLaunch"));
                    SoundManager.Add(Content.Load<SoundEffect>("MissileExplosion"));
                    SoundManager.Add(Content.Load<SoundEffect>("PewPewSound"));
                    #endregion

                    #region Animator
                    Content.RootDirectory = @"Content\Explosion";

                    List < Texture2D > tempTexture = new List<Texture2D>();
                    for (int i = 1; i < 48; i++)
                    {
                    
                        tempTexture.Add(Content.Load<Texture2D>(i < 10 ? $"000{i}" : $"00{i}"));
                    }
                    Animator.Add(new Animation(tempTexture, "Explosion", 0.1f));
                    Animator.Play("Explosion", new Rectangle(0,0,100,100));

                    Content.RootDirectory = "Content";
                    #endregion
                    #region UI
                    _UIs = new List<UI>();
                    _UIs.Add(new BoostBar(boostBar, barFrame, player, new Vector2(3300 , 2000), (int)player.MaxBoost));
                    _UIs.Add(new HealtBar(healthBar, barFrame, player, new Vector2(3300, 1900), (int)player.MaxHealth));
                    #endregion
                    BuyMenu.Target = player;
                    BuyMenu.CreateBuyMenu(Content, GraphicsDevice);
                    var FPS_Counter = new FPS_Counter(null, basicFont, player);
                    _UIs.Add(FPS_Counter);
                    Enemy enemy = new Enemy(Content.Load<Texture2D>("Enemy"), new Vector2(0, 0), 1, player,
                        bulletSound, missileExplosion, missleLaunch)
                    {
                        LinearVelocity = 3,
                        Missile = new Missile(Content.Load<Texture2D>("Missile")),
                        Bullet = new Bullet(Content.Load<Texture2D>("Bullet"))

                    };
                    enemy.EnemyDeadEvent += EnemyDead;
                    #region Spawner
                        spawner = new Spawner(new Enemy(Content.Load<Texture2D>("Enemy"), new Vector2(0, 0), 1, player,
                            bulletSound, missileExplosion, missleLaunch)
                        {
                            
                            LinearVelocity = 3,
                            Missile = new Missile(Content.Load<Texture2D>("Missile")),
                            Bullet = new Bullet(Content.Load<Texture2D>("Bullet")),
                            

                        }, _UIs, healthBar, barFrame, Content)
                        {
                            Asteroid = new Asteroid(Content.Load<Texture2D>("Asteroid")),
                            enemyDead = EnemyDead,
                        };
                        #endregion

                    
                    break;
                case GameState.Dead:
                    EndScene = Content.Load<Texture2D>("deathScene");
                    buttonSize = new Vector2(200, 200);
                    var dbtn = new Button(
                        CreateTexture(GraphicsDevice, (int) buttonSize.X, (int) buttonSize.Y, Color.White),
                        Content.Load<SpriteFont>("basicFont"), "Restart", buttonSize, new Vector2(0, 400), Color.White,
                        new Button.GameStateEventArgs(GameState.StartMenu));
                    dbtn.AssignEvent(GameStateEvent);
                    EndScreen = new List<UI>()
                    {
                        dbtn
                    };
                    break;
                case GameState.Scoreboard: 
                    break;
            }

            Debug.WriteLine("Done Loading");
            // TODO: use this.Content to load your game content here
        }

        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Color paint)
        {
            Texture2D texture = new Texture2D(device, width, height);
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Length; pixel++)
            {
                data[pixel] = paint;
            }
            texture.SetData(data);
            return texture;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            StartMenu = null;
            //Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            for (int i = 0; i < Animator.PlayingAnimations.Count; i++)
            {
                Animator.PlayingAnimations[i].Uppdate(gameTime);
            }
            switch (gameState)
            {
                case GameState.StartMenu:
                    #region StartMenu
                    for (int i = 0; i < StartMenu.Count; i++)
                    {
                        StartMenu[i].Clicked(mouse);
                    }
                    #endregion
                    break;
                case GameState.GameRunning:
                    #region GameRunning
                    foreach (var sprite in _sprites.ToArray())
                    {
                        sprite.Update(gameTime, _sprites);
                        base.Update(gameTime);
                    }
                    PostUpdate();
                    if (BuyMenu.IsVisible)
                        BuyMenu.update(gameTime);
                    _camera.Follow(player);
                    spawner.Uppdate(gameTime, _sprites);
                    //UI
                    for (int i = 0; i < _UIs.Count; i++)
                    {
                        _UIs[i].Update(gameTime);
                        base.Update(gameTime);
                        if (_UIs[i].IsRemoved)
                        {
                            _UIs.RemoveAt(i);
                            i--;
                        }
                    }
                    base.Update(gameTime);
                    #endregion
                    break;
                case GameState.Dead:
                    #region DeadSceen
                    
                    for (int i = 0; i < EndScreen.Count; i++)
                    {
                        EndScreen[i].Clicked(mouse);
                        base.Update(gameTime);
                    }
                    #endregion
                    break;
            }
            #region ESCAPE GAME
            if (_currentKey.IsKeyDown(Keys.Escape) && _previousKey.IsKeyUp(Keys.Escape))
            {
                if (gameState != GameState.StartMenu)
                {
                    gameState = GameState.StartMenu;
                    LoadContent();
                }
                else
                {
                    this.Exit();
                }

            }
            #endregion
        }

        private void PostUpdate()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            switch (gameState)
            {
                case GameState.StartMenu:
                    #region StartMenu
                    spriteBatch.Begin();
                    spriteBatch.Draw(_gameMenuBackground, new Rectangle(0, 0, ScreenWidth, ScreenHeight), new Rectangle(0, 0, _gameMenuBackground.Width, _gameMenuBackground.Height), Color.White);

                    for (int i = 0; i < StartMenu.Count; i++)
                    {
                        StartMenu[i].Draw(spriteBatch);
                    }
                    spriteBatch.End();
                    #endregion
                    break;
                case GameState.GameRunning:
                    #region GameRunning
                    spriteBatch.Begin(transformMatrix: _camera.Transform);
                    //Background
                    spriteBatch.Draw(_background, new Rectangle(GameWidth_Min_X, GameHeight_Min_Y, GameWidth_Max_X, GameHeight_Max_Y), new Rectangle(0, 0, _background.Width, _background.Height), Color.White);

                    if (Network.Players.Count > 0)
                    {
                        for (int p = 0; p < Network.AllClients.Count; p++)
                        {
                            for (int i = 0; i < Network.Players.Count; i++)
                            {
                                Network.Players[Network.AllClients[p]].Draw(spriteBatch);
                            }
                        }
                    }
                    //spriteBatch.Draw(boostBarFrame, new Vector2(player.Position.X + (ScreenWidth/2) - (boostBarFrame.Width),player.Position.Y + (ScreenHeight/2)), Color.White);
                    //objects
                    foreach (var sprite in _sprites)
                    {
                        sprite.Draw(spriteBatch);
                    }

                    for (int i = 0; i < Animator.PlayingAnimations.Count; i++)
                    {
                        Animator.PlayingAnimations[i].Draw(spriteBatch);
                    }
                    //UI
                    foreach (var ui in _UIs)
                    {
                        ui.Draw(spriteBatch);
                    }
                    if (BuyMenu.IsVisible)
                        BuyMenu.Draw(spriteBatch);
                    spriteBatch.DrawString(basicFont, $"Bullets {player.Ammo}", new Vector2(player.Position.X - (ScreenWidth / 2), player.Position.Y - (ScreenHeight / 2) + 100), Color.White);
                    spriteBatch.DrawString(basicFont, $"Score {Score}", new Vector2(player.Position.X - (ScreenWidth / 2), player.Position.Y - (ScreenHeight / 2)), Color.White);
                    spriteBatch.End();
                    base.Draw(gameTime);
                    #endregion
                    break;
                case GameState.Dead:
                    #region Dead
                    spriteBatch.Begin();
                    spriteBatch.Draw(EndScene, new Rectangle(0,0, Game1.ScreenWidth, Game1.ScreenHeight), Color.White);
                    if (EndScreen != null)
                    {
                        for (int i = 0; i < EndScreen.Count; i++)
                        {
                            EndScreen[i].Draw(spriteBatch);
                        }
                    }

                    spriteBatch.End();
                    #endregion
                    break;
            }

        }
    }
}