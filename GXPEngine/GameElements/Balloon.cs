using GXPEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.GameElements
{
    internal class Balloon : ALevelObject
    {
        public Balloon(Vector2 position, Vector2 size, float angle) : base("Textures/balloon.png")
        {
            body = new OBCollider(position, size, 0, this);
            Rotation = angle;
            Position = position;
            sprite.width = (int)size.x;
            sprite.height = (int)size.y;
        }
    }
}
