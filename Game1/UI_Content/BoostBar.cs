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
    class BoostBar : Bar
    {
        public Ship _player;
        public BoostBar(Texture2D texture, Texture2D frame, Sprite player, Vector2 offset, int max) : base(texture, frame, player, offset, max)
        {
            _player = this.player as Ship;
        }
        public override void Update(GameTime gameTime)
        {
            amount = _player.Boost;
        }
    }
}
