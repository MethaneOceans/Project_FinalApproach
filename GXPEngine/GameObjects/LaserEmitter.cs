using System.Drawing;
using static GXPEngine.Mathf;

namespace GXPEngine.GameObjects
{
	internal class LaserEmitter : GameObject
	{
		private EasyDraw body;
		public Ray ray;

		private bool moved;

		public new Vector2 Position
		{
			get => base.Position;
			set
			{
				base.Position = value;
				moved = true;
			}
		}
		public new float rotation
		{
			get => base.rotation;
			set
			{
				base.rotation = value;
				moved = true;
			}
		}

		public LaserEmitter()
		{
			// Draw body
			body = new EasyDraw(50, 50);
			body.SetOrigin(body.width / 2, body.height / 2);
			body.Clear(Color.Gray);
			body.Fill(Color.DarkRed);
			body.NoStroke();

			float leftOfSpot = body.width * 2 / 3;
			body.Rect((leftOfSpot + body.width) / 2, body.height / 2, Abs(leftOfSpot - body.width), 10);

			AddChild(body);

			// Create ray
			ray = new Ray();
		}

		public void Update()
		{
			if (moved)
			{
				ray.Origin = Position;
				ray.Direction = Vector2.GetUnitVectorDeg(rotation);
				moved = false;
			}
		}
	}
}
