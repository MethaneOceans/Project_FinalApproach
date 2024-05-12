using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace GXPEngine.Physics
{
	/// <summary>
	/// Oriented Box collider. Specifically not called OBB because it doesn't have bounding box functionality as of yet (if ever).
	/// </summary>
	internal class OBCollider : ICollider
	{
		public Vector2 Position;
		public Vector2 Size;
		public float Angle;

		public CollisionInfo LastCollision;

		public Vector2 NormalP => Vector2.GetUnitVectorDeg(Angle);
		public Vector2 NormalQ => Vector2.GetUnitVectorDeg(Angle + 90);

		public OBCollider(Vector2 position, Vector2 size, float angle)
		{
			Position = position;
			Size = size;
			Angle = angle;

			LastCollision = new CollisionInfo();
		}

		public bool Overlapping(ICollider other)
		{
			if (other is OBCollider) return Overlapping(other as OBCollider);
			else return false;
		}

		public bool Overlapping(OBCollider other)
		{
			bool thisOverlapP = OverlappingOnAxis(NormalP, other, out CollisionInfo bestCol);
			if (!thisOverlapP) return false;

			bool thisOverlapQ = OverlappingOnAxis(NormalQ, other, out CollisionInfo currentCol);
			if (!thisOverlapQ) return false;
			if (currentCol.PenetrationDepth >= bestCol.PenetrationDepth) bestCol = currentCol;

			bool otherOverlapP = other.OverlappingOnAxis(other.NormalP, this, out currentCol);
			if (!otherOverlapP) return false;
			if (currentCol.PenetrationDepth >= bestCol.PenetrationDepth) bestCol = currentCol;

			bool otherOverlapQ = other.OverlappingOnAxis(other.NormalQ, this, out currentCol);
			if (!otherOverlapQ) return false;
			if (currentCol.PenetrationDepth >= bestCol.PenetrationDepth) bestCol = currentCol;

			LastCollision = bestCol;
			Console.WriteLine(LastCollision.PenetrationDepth);

			return thisOverlapP && thisOverlapQ && otherOverlapP && otherOverlapQ;
		}

		public bool OverlappingOnAxis(Vector2 axis, OBCollider other, out CollisionInfo colInfo)
		{
			OBCollider BoxA = this;
			OBCollider BoxB = other;
			colInfo = new CollisionInfo();

			// Get min and max vertices along a given axis
			(Vector2 vecMinA, Vector2 vecMaxA) = BoxA.MinMaxCorner(axis);
			(Vector2 vecMinB, Vector2 vecMaxB) = BoxB.MinMaxCorner(axis);

			// Normalize the axis because longer vectors would make the next operations inaccurate
			Vector2 unitAxis = axis.Normalized();

			// Get the projected values of minmax on the unitAxis
			float projectedMinA = Vector2.Dot(unitAxis, vecMinA + BoxA.Position);
			float projectedMaxA = Vector2.Dot(unitAxis, vecMaxA + BoxA.Position);
			float projectedMinB = Vector2.Dot(unitAxis, vecMinB + BoxB.Position);
			float projectedMaxB = Vector2.Dot(unitAxis, vecMaxB + BoxB.Position);

			// A first then B
			if (projectedMinA <= projectedMinB && projectedMaxA >= projectedMinB)
			{
				colInfo.Normal = unitAxis;
				colInfo.PenetrationDepth = projectedMinB - projectedMaxA;

				return true;
			}
			if (projectedMinB <= projectedMinA && projectedMaxB >= projectedMinA)
			{
				colInfo.Normal = -unitAxis;
				colInfo.PenetrationDepth = projectedMinB - projectedMaxA;

				return true;
			}
			return false;
		}

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

		// Get a corner (or center) of the box
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
