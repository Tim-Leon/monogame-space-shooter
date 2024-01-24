using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Button : UI
    {
        public string Text;
        public Color color; //FIX FIX
        public GameStateEventArgs Args;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="buttonSize"></param>
        /// <param name="offset">offset from center of screen</param>
        /// <param name="args"></param>
        public Button(Texture2D texture, SpriteFont font, string text, Vector2 buttonSize, Vector2 offset, Color color, GameStateEventArgs args = null) : base(texture, font)
        {
            this.Text = text;
            this.Position = new Vector2((Game1.ScreenWidth / 2) - (buttonSize.X / 2),
                (Game1.ScreenHeight / 2) - (buttonSize.Y / 2));
            this.Position += offset;
            this.Args = args;
            this.color = color;
        }
        /// <summary>
        /// dose the same as the other button constructor but uses a struck to make it easier to implement many button without being confused
        /// </summary>
        /// <param name="buttonValues"></param>
        /// <param name="text"></param>
        /// <param name="offset">offset from center of screen</param>
        /// <param name="args"></param>
        public Button(ButtonValues buttonValues, string text, Vector2 offset, GameStateEventArgs args = null) : base(buttonValues.texture2D, buttonValues.font)
        {
            this.Text = text;
            this.Position += offset;
            this.Args = args;
            this.color = buttonValues.color;
            this.Position = new Vector2((Game1.ScreenWidth / 2) - (buttonValues.buttonSize.X / 2),
                (Game1.ScreenHeight / 2) - (buttonValues.buttonSize.Y / 2));
        }

        private MouseState _lastState;
        private MouseState _currentState;
        public override void Clicked(MouseState MouseState)
        {
            _lastState = _currentState;
            _currentState = MouseState;
            if (MouseState.Y < Position.Y + _texture.Height &&
                MouseState.Y > Position.Y &&
                MouseState.X < Position.X + _texture.Width &&
                MouseState.X > Position.X)
            {
                if (MouseState.LeftButton == ButtonState.Pressed)
                {
                    color = Color.Yellow;
                    if ((MouseClick != null) && (_currentState.LeftButton == ButtonState.Pressed) && (_lastState.LeftButton == ButtonState.Released))
                    {
                        Debug.WriteLine("CLICKED");
                        MouseClick(this, Args);
                    }
                }
                color = Color.LightBlue;
            }
            else
            {
                color = Color.White;
            }
        }
        public void AssignEvent(MouseClickEvent eClickEvent)
        {
            MouseClick += eClickEvent;
        }

        public void UnsubscribeEvent(MouseClickEvent eClickEvent)
        {
            MouseClick -= eClickEvent;
        }

        public void Show()
        {
            IsVisible = true;
        }
        public void Hide()
        {
            IsVisible = false;
        }

        public delegate void MouseClickEvent(object sender, GameStateEventArgs e);

        public event MouseClickEvent MouseClick;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible == true)
            {
                Vector2 fontSize = Font.MeasureString(Text);
                spriteBatch.Draw(_texture, Position, null, color, _rotation, Origin, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(Font, Text, new Vector2(Position.X + (_texture.Width / 2) - (fontSize.X / 2), Position.Y + (_texture.Height / 2) - (fontSize.Y / 2)), Color.Blue);
            }
        }
        public class GameStateEventArgs : EventArgs
        {
            public Game1.GameState state;
            public GameStateEventArgs(Game1.GameState gameState)
            {
                state = gameState;
            }

        }
        public struct ButtonValues
        {
            public Texture2D texture2D;
            public SpriteFont font;
            public Color color;
            public Vector2 buttonSize;
        }
    }


}
