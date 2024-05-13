namespace GXPEngine.Physics
{
	public class CollisionInfo
	{
		public Vector2 Normal;
		public float PenetrationDepth;

		public CollisionInfo(Vector2 normal, float penetrationDepth)
		{
			Normal = normal;
			PenetrationDepth = penetrationDepth;
		}
		public CollisionInfo() : this(new Vector2(), -1000) { }
	}
}
