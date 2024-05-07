using static GXPEngine.Mathf;

namespace GXPEngine.Physics
{
	internal struct CollisionInfo
	{
		public Collider thisCollider;
		public Collider otherCollider;

		public Vector2 Normal;
		public float TimeOfImpact;

		public CollisionInfo(Collider thisCollider, Collider otherCollider, Vector2 Normal, float TimeOfImpact)
		{
			this.thisCollider = thisCollider;
			this.otherCollider = otherCollider;
			this.Normal = Normal;
			this.TimeOfImpact = TimeOfImpact;
		}

		// Used for "empty" collisions
		public static CollisionInfo Empty(Collider thisCollider)
		{
			return new CollisionInfo(thisCollider, null, new Vector2(), 1);
		}
		public bool IsValid()
		{
			if (thisCollider == null) return false;
			if (otherCollider == null) return false;
			if (Abs(Normal.LengthSquared() - 1.0f) > 0.01f) return false;
			else return true;
		}
		public override string ToString()
		{
			return $"ThisCollider: {thisCollider}\nOtherCollider: {otherCollider}\nNormal: {Normal}\nTimeOfImpact: {TimeOfImpact}";
		}
	}
}