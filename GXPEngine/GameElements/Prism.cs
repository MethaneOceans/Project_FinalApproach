using GXPEngine.GameElements;
using GXPEngine.Physics;
using System;
using System.Diagnostics;


namespace GXPEngine
{
	internal class Prism : ALevelObject
	{
		private readonly Stopwatch stopwatch;
		private readonly float TimeToLive = 1000;

		public Prism(Vector2 position, Vector2 velocity) : base("square.png")
		{
			body = new OBCollider(position, new Vector2(sprite.width, sprite.height), 0, this)
			{
				Velocity = velocity,
			};

			body.Velocity = velocity;
			body.Behavior = ACollider.ColliderType.Rigid;

			stopwatch = new Stopwatch();
			stopwatch.Start();
		}
		public Prism(Vector2 position, Vector2 velocity, float timeToLive) : base("square.png")
		{
			body = new OBCollider(position, new Vector2(sprite.width, sprite.height), 0, this)
			{
				Velocity = velocity,
			};

			body.Velocity = velocity;
			body.Behavior = ACollider.ColliderType.Rigid;

			TimeToLive = timeToLive;

			stopwatch = new Stopwatch();
			stopwatch.Start();
		}

		public void Update()
		{
			if (body.Behavior != ACollider.ColliderType.Static)
			{
				if (stopwatch.ElapsedMilliseconds > TimeToLive)
				{
					body.Behavior = ACollider.ColliderType.Static;
					OnPrismFreeze?.Invoke(this, new EventArgs());
				}
			}
		}

		public EventHandler OnPrismFreeze;
	}
}
