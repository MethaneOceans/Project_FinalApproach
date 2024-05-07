using GXPEngine.Core;
using GXPEngine.Physics.Colliders;
using System;
using System.Drawing;

namespace GXPEngine.Physics.Primitives
{
	internal class CircleObject : PhysicsED
	{
		public new CircleCollider PCollider
		{
			get => (CircleCollider)base.PCollider;
			set => base.PCollider = value;
		}
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

		private PhysicsManager physicsManager;

		public CircleObject(PhysicsManager manager, int radius) : base(radius * 2, radius * 2, new CircleCollider(radius))
		{
			ED.Ellipse((ED.width - 1) / 2f, (ED.height - 1) / 2f, ED.width - 1, ED.height - 1);

			PCollider = new CircleCollider(radius)
			{
				Owner = this
			};

			SetXY(PCollider.Position.x, PCollider.Position.y);

			physicsManager = manager;
			physicsManager.AddCollider(PCollider);
		}

		public void Update()
		{
			SetXY(PCollider.Position.x, PCollider.Position.y);
			Redraw();
		}
		private void Redraw()
		{
			if (DirtyFlag)
			{
				ED.ClearTransparent();
				ED.Stroke(color);
				ED.Fill(color);
				ED.Ellipse((ED.width - 1) / 2f, (ED.height - 1) / 2f, ED.width - 1, ED.height - 1);

				DirtyFlag = false;
			}
		}
	}
}
