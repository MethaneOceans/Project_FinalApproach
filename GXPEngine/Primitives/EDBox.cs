using System.Drawing;

namespace GXPEngine.Primitives
{
	internal class EDBox : Box
	{
		public EasyDraw ED;

		public EDBox(Vector2 position, Vector2 size, float angle) : base(position, size, angle)
		{
			ED = new EasyDraw((int)size.x, (int)size.y);
			ED.SetOrigin(ED.width / 2, ED.height / 2);
			AddChild(ED);

			ED.Position = new Vector2(0, 0);

			ED.NoFill();
			ED.Stroke(Color.White);
			ED.StrokeWeight(3);
			ED.Rect(ED.width / 2f, ED.height / 2f, ED.width - 1, ED.height - 1);
			ED.Line(ED.width / 2f, ED.height / 2f, ED.width, ED.height / 2f);
			ED.SetColor(0, 0, 1);
		}
	}
}
