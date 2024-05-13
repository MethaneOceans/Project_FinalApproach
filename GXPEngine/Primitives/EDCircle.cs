namespace GXPEngine.Primitives
{
	internal class EDCircle : Circle
	{
		public EasyDraw ED;

		public EDCircle(Vector2 position, float radius) : base(position, radius)
		{
			int radius2 = (int)(radius * 2);
			ED = new EasyDraw(radius2, radius2);
			ED.SetOrigin(radius, radius);

			ED.NoFill();
			ED.Ellipse((int)radius, (int)radius, radius2 - 1, radius2 - 1);
			AddChild(ED);
		}
	}
}
