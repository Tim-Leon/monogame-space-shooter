using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1.UI_Content
{
    static class BuyMenu
    {
        public static List<UI> BuyUis = new List<UI>();
        public static Ship Target;
        public static void CreateBuyMenu(ContentManager content, GraphicsDevice graphicsDevice)
        {
            var DefultButton = new Button.ButtonValues()
            {
                buttonSize = new Vector2(200, 200),
                color = Color.White,
                font = content.Load<SpriteFont>("basicfont"),
                texture2D = Game1.CreateTexture(graphicsDevice, 100, 100, Color.White)
            };
            BuyUis.Add(new Button(DefultButton, "+20 DMG", new Vector2(-Game1.ScreenWidth/2, -Game1.ScreenHeight/2)));
        }

        public static bool IsVisible
        {
            get;
            private set;
        }

        public static void update(GameTime gameTime)
        {
            if (IsVisible)
            {
                for (int i = 0; i < BuyUis.Count; i++)
                {
                    BuyUis[i].Update(gameTime, Mouse.GetState());
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < BuyUis.Count; i++)
            {
                BuyUis[i].Draw(spriteBatch);
            }
        }

        public static void Show()
        {
            IsVisible = true;
            for (int i = 0; i < BuyUis.Count; i++)
            {
                BuyUis[i].IsVisible = true;
            }
        }

        public static void Hide()
        {
            IsVisible = false;
            for (int i = 0; i < BuyUis.Count; i++)
            {
                BuyUis[i].IsVisible = false;
            }
        }
    }
}
