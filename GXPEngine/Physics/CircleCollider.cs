using System;
using static GXPEngine.Mathf;

namespace GXPEngine.Physics
{
	internal class CircleCollider : ACollider
	{
		public float Radius;

		public CircleCollider(Vector2 center, float radius)
		{
			Position = center;
			Radius = radius;
		}

		public override (float min, float max) MinMaxBounds(Vector2 axis)
		{
			float a = Vector2.Dot(Position, axis);
			return (a - Radius, a + Radius);
		}

		public override Vector2 NormalAt(Vector2 point)
		{
			return (point - Position).Normalized();
		}

		public override bool Overlapping(ACollider other)
		{
			throw new NotImplementedException();
		}
		private bool Overlapping(OBCollider other)
		{
			Ray r = new Ray(Position, other.Position);
			Vector2 closestPoint = r.At(other.RayCast(r));

			float distanceSquared = (Position - closestPoint).LengthSquared();
			if (distanceSquared <= Radius * Radius)
			{
				CollisionInfo col = new CollisionInfo();
				col.Normal = NormalAt(closestPoint);
				col.PenetrationDepth = Radius - Sqrt(distanceSquared);
				return true;
			}
			else return false;
		}
		private bool Overlapping(CircleCollider other)
		{
			float distanceSquared = (Position - other.Position).LengthSquared() - Radius * other.Radius;
			if (distanceSquared <= 0)
			{
				CollisionInfo col = new CollisionInfo();
				col.Normal = (other.Position - Position).Normalized();
				col.PenetrationDepth = -Sqrt(distanceSquared);
				return true;
			}
			else return false;
		}

		public override float RayCast(Ray ray)
		{
			throw new NotImplementedException();
		}
	}
}
