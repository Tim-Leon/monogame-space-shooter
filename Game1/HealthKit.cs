using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class HealthKit : Sprite
    {
        
        public HealthKit(Texture2D texture, Vector2 position) : base(texture)
        {
            this.Position = position;
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _rotation += MathHelper.ToRadians(2);
            for (int i = 0; i < sprites.Count; i++)
            {
                if(IsTouching(sprites[i]) && sprites[i] is Ship)
                {
                    var player = sprites[i] as Ship;
                    player.Health += 20;
                    this.IsRemoved = true;
                }
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color, _rotation, Origin, 1, Flip, 0);
        }
    }
}
