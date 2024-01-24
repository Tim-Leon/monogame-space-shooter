using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class Bar : UI
    {
        public Texture2D Frame;
        public Sprite player;
        public int TextureWidth;
        public float amount;
        public float max;
        public Vector2 offset;
        public Bar(Texture2D texture, Texture2D frame, Sprite player, Vector2 offset, int max) : base(texture)
        {
            this.Frame = frame;
            this.player = player;
            TextureWidth = texture.Width;
            this.max = max;
            this.offset = offset;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Frame, new Vector2(player.Position.X - Frame.Width/2 + offset.X, player.Position.Y  - Frame.Height/2 + offset.Y), Color.White);
            spriteBatch.Draw(_texture, new Rectangle((int)(player.Position.X  - TextureWidth/2 + offset.X), (int)(player.Position.Y - _texture.Height/2 + offset.Y), width: (int)((amount / max) * (float)TextureWidth), height: _texture.Height), Color.White);
        }
    }
}
