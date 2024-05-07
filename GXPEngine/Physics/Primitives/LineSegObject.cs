using GXPEngine.Physics.Colliders;
using System.Drawing;
using static GXPEngine.Mathf;

namespace GXPEngine.Physics.Primitives
{
	internal class LineSegObject : PhysicsED
	{
		public Color PColor
		{
			get => color;
			set
			{
				color = value;
				DirtyFlag = true;
			}
		}
		private Color color;

		public LineSegObject(PhysicsManager manager, Vector2 start, Vector2 end) : base(
			(int)Abs(start.x - end.x), 
			(int)Abs(start.y - end.y), 
			new LineSegmentCollider(start, end))
		{
			ED.SetOrigin(0, 0);
			ED.Mirror(start.x < end.x, start.y < end.y);
			Vector2 topLeft = new Vector2(Min(start.x, end.x), Min(start.y, end.y));
			Position = topLeft;

			manager.AddCollider(PCollider);
		}

		private void Redraw()
		{
			if (DirtyFlag)
			{
				ED.ClearTransparent();
				ED.Stroke(color);
				ED.StrokeWeight(1);
				ED.Fill(color);
				ED.Line(0, 0, ED.width - 1, ED.height - 1);

				//ED.Clear(color);
			}
		}
		public void Update()
		{
			Redraw();
		}
	}
}
