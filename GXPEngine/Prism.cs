using GXPEngine.Physics;
using GXPEngine.Primitives;
using System;
using System.Diagnostics;


namespace GXPEngine
{
	internal class Prism : Circle
	{
		private Stopwatch stopwatch;
		private Sprite sprite;
		private float TimeToLive = 1000;

		public Prism(Vector2 position, Vector2 velocity) : base(position, 50)
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
		public Prism(Vector2 position, Vector2 velocity, float timeToLive) : this(position, velocity)
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
