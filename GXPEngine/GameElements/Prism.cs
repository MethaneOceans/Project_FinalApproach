using GXPEngine.GameElements;
using GXPEngine.Physics;
using System.Diagnostics;


namespace GXPEngine
{
	internal class Prism : ALevelObject
	{
		private readonly Stopwatch stopwatch;
		//private Sprite sprite;
		private readonly float TimeToLive = 1000;

		public Prism(Vector2 position, Vector2 velocity) : base("square.png")
		{
			body = new OBCollider(position, new Vector2(sprite.width, sprite.height), 0, this)
			{
				Velocity = velocity,
			};

			body.Velocity = velocity;
			body.Behavior = ACollider.ColliderType.Rigid;

			//sprite = new Sprite("circle.png");

			//sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
			//AddChild(sprite);

			//float doubleR = 100;
			//sprite.scale = doubleR / sprite.width;
			//Console.WriteLine(sprite.width / doubleR);

			stopwatch = new Stopwatch();
			stopwatch.Start();
		}
		public Prism(Vector2 position, Vector2 velocity, float timeToLive) : this(position, velocity)
		{
			TimeToLive = timeToLive;
		}

		public void Update()
		{
			if (body.Behavior != ACollider.ColliderType.Static)
			{
				if (stopwatch.ElapsedMilliseconds > TimeToLive)
				{
					body.Behavior = ACollider.ColliderType.Static;
				}
			}
		}
	}
}
