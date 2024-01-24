using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{

    class EnemyHealthBar : Bar
    {
        public Enemy enemy;
        public EnemyHealthBar(Texture2D texture, Texture2D frame, Sprite player, Vector2 offset, int max) : base(texture, frame, player, offset, max)
        {
            enemy = player as Enemy;
        }

        public override void Update(GameTime gameTime)
        {
            amount = enemy.Health;
            if (amount <= 0)
            {
                IsRemoved = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Frame, new Vector2(enemy.Position.X + offset.X, enemy.Position.Y + offset.Y), Color.White);
            spriteBatch.Draw(_texture, new Rectangle((int)(player.Position.X + offset.X), (int)(player.Position.Y +  offset.Y), width: (int)((amount / max) * (float)TextureWidth), height: _texture.Height), Color.White);
        }
    }
}
