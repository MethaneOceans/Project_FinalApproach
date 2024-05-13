using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Prism : Sprite
    {
        Sprite prism;
        Stopwatch stopWatch = new Stopwatch();
        bool Frozen; //check if prism is frozen
        bool Shot; //check if the prism has been launched

        //variables
        float SpeedY = 0.0f; //horizontal starting speed. This variable changes when pressing the up or down arrows
        float SpeedX = 0.0f; //vertical starting speed. This variable changes when pressing the left or right arrows
        int freezeTime = 1000; //time between prism being shot and freezing in milliseconds

        public Prism() : base("circle.png")
        {
            //creates a new sprite and sets the spinning point in the center
            prism = new Sprite("circle.png");
            prism.SetOrigin(prism.width /2, prism.height / 2);
            prism.SetXY(width /2, height /2);
            AddChild(prism);

            prism.rotation = 0;

            y = 536;

        }

        private void Update()
        {
            if (!Shot) //This checks if the prism has been fired. If it hasn't you can change the vertical and horizontal speeds
            {
                if (Input.GetKeyDown(Key.UP))
                {
                    SpeedY--;
                    Console.WriteLine("Vertical speed is: {0}", SpeedY);
                }
                if (Input.GetKeyDown(Key.DOWN))
                {
                    SpeedY++;
                    Console.WriteLine("Vertical speed is: {0}", SpeedY);
                }
                if (Input.GetKeyDown(Key.RIGHT))
                {
                    SpeedX++;
                    Console.WriteLine("horizontal speed is: {0}", SpeedX);
                }
                if (Input.GetKeyDown(Key.LEFT))
                {
                    SpeedX--;
                    Console.WriteLine("horizontal speed is: {0}", SpeedX);
                }
                if (Input.GetKeyDown(Key.SPACE))
                {
                    Shot = true;
                    stopWatch.Start();
                    Movement();
                }
            }
            else //if it has been fired it will check if the freeze time has passed.
            {
                if (stopWatch.Elapsed.TotalMilliseconds > freezeTime) //if the freeze time is over, the prism will be frozen in place and you will be able to rotate it
                {
                    SpeedY = 0.0f;
                    Frozen = true;
                    //stopWatch.Reset();
                    Rotate();
                }
                else //if freeze time is not over the prism will continue moving like normal
                {
                    Movement();
                }
            }

            //Freeze(); //checks if spacebar is pressed
            //if (Frozen) //if the prism is frozen you can rotate
            //{
            //    Rotate();
            //}
            //else //if it isn't frozen it will move
            //{
            //    Movement();
            //}
        }

        public void Movement()
        {
            //gravity
            SpeedY = SpeedY + 0.5f;
            y = y + SpeedY;

            //shooting force
            x = x + SpeedX;
        }

        private void Rotate()
        {
            //if its frozen rotate with left and right arrow keys
            if (Input.GetKey(Key.LEFT))
            {
                prism.rotation = prism.rotation - 1;
            }
            if (Input.GetKey(Key.RIGHT))
            {
                prism.rotation = prism.rotation + 1;
            }
        }
    }
}
