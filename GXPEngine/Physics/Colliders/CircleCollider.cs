using GXPEngine.Physics.Primitives;
using System;
using static GXPEngine.Mathf;

namespace GXPEngine.Physics.Colliders
{
	internal class CircleCollider : Collider
	{
		public float Radius
		{
			get => radius;
			set
			{
				radius = value;
				Mass = radius * radius;
			}
		}
		private float radius;

		public CircleCollider(int radius, int? mass = null)
		{
			Radius = radius;
			if (mass != null)
			{
				Mass = (int)mass;
			}
			else
			{
				Mass = radius * radius;
			}
		}

		public override bool Overlaps(Collider other)
		{
			bool overlapping = false;

			if (other is CircleCollider otherCircle)
			{
				// Check for discrete simulation. Check the new position of this against the current position of other
				float distance = (Position - otherCircle.Position).Length() - (Radius + otherCircle.Radius);
				if (distance < 0)
				{
					overlapping = true;
				}
			}
			else if (other is LineSegmentCollider otherLine)
			{
				float distSquared;

				Vector2 relativePosition = Position - otherLine.Begin;
				Vector2 lineVec = otherLine.AsVec;
				Vector2 lineUnit = lineVec.Normalized();
				float lineLen = lineVec.Length();

				// Calculate which point on the line is closest
				float distOnLine = Clamp(Vector2.Dot(relativePosition, lineUnit), 0, lineLen);
				Vector2 closestPoint = lineUnit * distOnLine;

				distSquared = (closestPoint - relativePosition).LengthSquared();
				if (distSquared < Radius * Radius)
				{
					overlapping = true;
				}
			}
			else if (other is AABBCollider otherAABB)
			{

			}

			return overlapping;
		}

		public override CollisionInfo GetCollisionInfo(Collider other)
		{
			float TOI = float.PositiveInfinity;
			Vector2 normal = new Vector2();

			// Match the other collider to a known collider type
			if (other is CircleCollider otherCircle)
			{
				Vector2 relativePosition = OldPosition - other.Position;

				float a = Velocity.LengthSquared();
				if (a == 0)
				{
					return CollisionInfo.Empty(this);
				}

				float b = 2 * Vector2.Dot(relativePosition, Velocity);
				float c = relativePosition.LengthSquared() - (Radius + otherCircle.Radius) * (Radius + otherCircle.Radius);
				float discriminant = QuadraticSolver.GetDiscriminant(a, b, c);

				if (discriminant < 0)
				{
					return CollisionInfo.Empty(this);
				}

				(float rootA, float rootB) = QuadraticSolver.GetRoots(discriminant, a, b);
				TOI = Min(rootA, rootB);

				normal = (otherCircle.Position - (OldPosition + TOI * Velocity)).Normalized();
			}
			else if (other is LineSegmentCollider otherLine)
			{
				Vector2 relativePosition = OldPosition - otherLine.Begin;
				Vector2 lineVec = otherLine.AsVec;
				Vector2 lineUnit = lineVec.Normalized();
				Vector2 lineNormal = lineVec.Normal();
				if (Vector2.Dot(relativePosition, lineNormal) < 0) lineNormal *= -1;
				float lineLen = lineVec.Length();

				// Calculate which point on the line is closest
				float distOnLine = Clamp(Vector2.Dot(relativePosition, lineUnit), 0, lineLen);
				Vector2 closestPoint = lineUnit * distOnLine;

				if ((distOnLine > 0) && (distOnLine < lineLen))
				{
					// Line and ball overlap
					// Now it's just point - circle collision?
					// TOI = a / b
					// a is the distance from the old position to the point of impact (along the line normal)
					// b is the distance from the old position to the next position (along the line normal)
					float a = Vector2.Dot(relativePosition, lineNormal) - Radius;
					a = Abs(a);
					float b = Vector2.Dot(relativePosition + Velocity, lineNormal);
					b = Abs(b);
					TOI = a / b;
					normal = lineNormal;
				}
				else
				{
					// Now it's just point - circle collision?
					float a = Velocity.LengthSquared();
					float b = 2 * Vector2.Dot(relativePosition - closestPoint, Velocity);
					float c = (relativePosition - closestPoint).LengthSquared() - Radius * Radius;
					float Discriminant = QuadraticSolver.GetDiscriminant(a, b, c);

					(float rootA, float rootB) = QuadraticSolver.GetRoots(Discriminant, a, b);
					TOI = Min(rootA, rootB);
					normal = (relativePosition + TOI * Velocity - closestPoint).Normalized();
				}
				//normal = (closestPoint - relativePosition).Normalized();
			}

			//if (TOI == float.PositiveInfinity) return CollisionInfo.Empty(this);
			return new CollisionInfo(this, other, normal, TOI);
		}
	}
}
