using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Camera
    {
        public Matrix Transform;
        public float Zoom = 0.5f;
        public void Follow(Sprite target)
        {
            var position = Matrix.CreateTranslation(-target.Position.X  ,
                -target.Position.Y , 0);
             var offset =  Matrix.CreateTranslation((Game1.ScreenWidth / 2) * Zoom, (Game1.ScreenHeight / 2) * Zoom, 0);
             Transform = position * Matrix.CreateScale(Zoom, Zoom, Zoom) * offset;
        }
    }
}