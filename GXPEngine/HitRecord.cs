namespace GXPEngine
{
	internal class HitRecord
	{
		public bool Hit;
		public float t;
		public Vector2 Normal;

		public HitRecord()
		{
			Hit = false;
			t = 0;
			Normal = new Vector2();
		}
	}
}
