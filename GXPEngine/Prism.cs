using GXPEngine.Physics;
using GXPEngine.Primitives;
using System;
using System.Diagnostics;


namespace GXPEngine
{
	public class Prism : Sprite
	{
		private readonly Sprite prism;
		private readonly Stopwatch stopWatch = new Stopwatch();
		private bool Frozen; //check if prism is frozen
		private bool Shot; //check if the prism has been launched

		private CircleCollider body;

		//variables
		float SpeedY = 0.0f; //horizontal starting speed. This variable changes when pressing the up or down arrows
		float SpeedX = 0.0f; //vertical starting speed. This variable changes when pressing the left or right arrows
		Vector2 Velocity = new Vector2();
		int freezeTime = 1000; //time between prism being shot and freezing in milliseconds

		public Prism() : base("circle.png")
		{
			SetOrigin(width / 2, height / 2);

			//creates a new sprite and sets the spinning point in the center
			prism = new Sprite("circle.png");
			prism.SetOrigin(prism.width /2, prism.height / 2);
			//prism.SetXY(0, 0);
			AddChild(prism);

			prism.rotation = 0;
		}
		public Prism(Vector2 position, Vector2 velocity) : base("circle.png")
		{
			SetOrigin(width / 2, height / 2);
			prism = new Sprite("circle.png");
			prism.SetOrigin(prism.width / 2, prism.height / 2);
			AddChild(prism);

			Position = position;
			Velocity = velocity;
		}

		private void Update()
		{
			if (!Shot) //This checks if the prism has been fired. If it hasn't you can change the vertical and horizontal speeds
			{
				Vector2 mouse = Input.mousePos;
				Console.WriteLine(mouse - Position);
				Velocity = (mouse - Position).Normalized() * 25;

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
					Velocity = new Vector2();
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
			Velocity.y += 0.5f;
			Position += Velocity;
		}

		private void Rotate()
		{
			//if its frozen rotate with left and right arrow keys
			if (Input.GetKey(Key.LEFT))
			{
				prism.rotation--;
			}
			if (Input.GetKey(Key.RIGHT))
			{
				prism.rotation++;
			}
		}
	}

	internal class Prism2 : Circle
	{
		private Stopwatch stopwatch;
		private Sprite sprite;
		private float TimeToLive = 1000;

		public Prism2(Vector2 position, Vector2 velocity) : base(position, 50)
		{
			body.Velocity = velocity;
			body.IsStatic = false;

			sprite = new Sprite("circle.png");

			sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
			AddChild(sprite);

			float doubleR = 100;
			sprite.scale = doubleR / sprite.width;
			Console.WriteLine(sprite.width / doubleR);

			stopwatch = new Stopwatch();
			stopwatch.Start();
		}
		public Prism2(Vector2 position, Vector2 velocity, float timeToLive) : this(position, velocity)
		{
			TimeToLive = timeToLive;
		}

		public void Update()
		{
			if (!body.IsStatic)
			{
				if (stopwatch.ElapsedMilliseconds > TimeToLive)
				{
					body.IsStatic = true;
				}
				else
				{
					body.Velocity += new Vector2(0, 0.5);
				}
			}
		}
	}
}
