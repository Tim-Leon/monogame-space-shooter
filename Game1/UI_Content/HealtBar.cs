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
    class HealtBar : Bar
    {
        public Ship _player;
        public HealtBar(Texture2D texture, Texture2D frame, Sprite player, Vector2 offset, int max) : base(texture, frame, player, offset, max)
        {
            _player = player as Ship;
        }

        public override void Update(GameTime gameTime)
        {
            amount = _player.Health;
        }

    }
}
