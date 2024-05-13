using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Mirror : Sprite
    {
        public Mirror() : base("mirror.png")
        {
            x = 200;
            y = 200;
        }

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (Input.GetKey(Key.LEFT)) 
            {
                rotation = rotation + 1;
            }
            if (Input.GetKey(Key.RIGHT))
            {
                rotation = rotation - 1;
            }
        }
    }
}
