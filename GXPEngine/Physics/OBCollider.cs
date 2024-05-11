using System;
using System.Collections.Generic;

namespace GXPEngine.Physics
{
	/// <summary>
	/// Oriented Box collider. Specifically not called OBB because it doesn't have bounding box functionality as of yet (if ever).
	/// </summary>
	internal class OBCollider : ICollider<OBCollider>
	{
		public Vector2 Position;
		public Vector2 Size;
		public float Angle;

		public Vector2 normalP
		{
			get => Vector2.GetUnitVectorDeg(Angle);
		}
		public Vector2 normalQ
		{
			get => Vector2.GetUnitVectorDeg(Angle + 90);
		}

		// TODO: Add angle
		public OBCollider(Vector2 position, Vector2 size, float angle)
		{
			Position = position;
			Size = size;
			Angle = angle;
		}

		public bool Overlapping(OBCollider other)
		{
			bool thisOverlapP = OverlappingOnAxis(normalP, other);
			bool thisOverlapQ = OverlappingOnAxis(normalQ, other);
			bool otherOverlapP = other.OverlappingOnAxis(other.normalP, this);
			bool otherOverlapQ = other.OverlappingOnAxis(other.normalQ, this);

			return thisOverlapP && thisOverlapQ && otherOverlapP && otherOverlapQ;
		}

		public bool OverlappingOnAxis(Vector2 axis, OBCollider other)
		{
			OBCollider BoxA = this;
			OBCollider BoxB = other;

			var LimitsA = BoxA.MinMaxCorner(axis);
			var LimitsB = BoxB.MinMaxCorner(axis);

			Vector2 unitAxis = axis.Normalized();
			float minA = Vector2.Dot(unitAxis, LimitsA.min + BoxA.Position);
			float maxA = Vector2.Dot(unitAxis, LimitsA.max + BoxA.Position);
			float minB = Vector2.Dot(unitAxis, LimitsB.min + BoxB.Position);
			float maxB = Vector2.Dot(unitAxis, LimitsB.max + BoxB.Position);

			if (minA < maxB && maxA > minB) return true;
			if (minB < maxA && maxB > minA) return true;
			return false;
		}

		public (Vector2 min, Vector2 max) MinMaxCorner(Vector2 axis)
		{
			Vector2 min = new Vector2();
			Vector2 max = new Vector2();

			float minProjection = 0;
			float maxProjection = 0;

			for (int i = 1; i < 5; i++)
			{
				Vector2 corner = GetCornerOffset(i);
				float projection = Vector2.Dot(corner, axis);

				if (projection < minProjection)
				{
					min = corner;
					minProjection = projection;
				}
				else if (projection > maxProjection)
				{
					max = corner;
					maxProjection = projection;
				}
			}
			
			return (min, max);
		}

		public Vector2 GetCornerOffset(int i)
		{
			Vector2 corner;

			switch (i)
			{
				case 0:
					return Position;
				case 1:
					corner = new Vector2(Size.x * 0.5, Size.y * 0.5);
					break;
				case 2:
					corner = new Vector2(Size.x * -0.5, Size.y * 0.5);
					break;
				case 3:
					corner = new Vector2(Size.x * -0.5, Size.y * -0.5);
					break;
				case 4:
					corner = new Vector2(Size.x * 0.5, Size.y * -0.5);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return corner.RotatedDeg(Angle);
		}
	}
}
