using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Player : Sprite
    {
        public Player() : base("square.png")
        {

        }

        private void Update()
        {
            Movement();
        }

        private void Movement()
        {
            if (Input.GetKey(Key.A))
            {
                x = x - 2.0f;
            }
            if (Input.GetKey(Key.D))
            {
                x = x + 2.0f;
            }
            if (Input.GetKey(Key.W))
            {
                y = y - 2.0f;
            }
            if (Input.GetKey(Key.S))
            {
                y = y + 2.0f;
            }
        }
    }
}
