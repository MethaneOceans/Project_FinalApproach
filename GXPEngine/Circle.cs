using System.Drawing;

namespace GXPEngine
{
	internal class Circle : EasyDraw
	{
		public Vector2 Position
		{
			get => new Vector2(x, y);
			set
			{
				x = value.x;
				y = value.y;
			}
		}
		public float Radius
		{
			get => radius;
			set
			{
				radius = value;
				Redraw();
			}
		}
		private float radius;

		public Circle(float radius) : base((int)(radius * 2), (int)(radius * 2))
		{
			this.radius = radius;
			SetOrigin(radius, radius);
			Redraw();
		}
		public Circle(Vector2 position, float radius) : this(radius)
		{
			Position = position;
		}

		private void Redraw()
		{
			Fill(Color.Blue);
			Ellipse((width - 1) / 2f, (height - 1) / 2f, radius * 2, radius * 2);
		}
		//public float RayIntersect(Ray ray)
		//{

		//}
	}
}
