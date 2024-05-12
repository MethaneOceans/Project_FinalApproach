using System;
using static GXPEngine.Mathf;

namespace GXPEngine.Physics
{
	/// <summary>
	/// Oriented Box collider. Specifically not called OBB because it doesn't have bounding box functionality as of yet (if ever).
	/// </summary>
	internal partial class OBCollider : ICollider
	{
		public Vector2 Velocity;

		private bool valueChanged = true;

		// ===================================
		// Position property
		// ===================================
		public Vector2 Position
		{
			get => _position;
			set
			{
				_position = value;
				Invalidate();
			}
		}
		private Vector2 _position;

		// ===================================
		// Angle property
		// ===================================
		public float Angle
		{
			get => _angle;
			set
			{
				_angle = value;
				Invalidate();
			}
		}
		private float _angle;


		public CollisionInfo LastCollision;

		// Size property can't change, this to prevent unpredictable behavior.
		public readonly Vector2 Size;
		// These are the relative corners and normals to the center. This won't change.
		public readonly Vector2[] baseCorners;
		public readonly Vector2[] baseNormals;

		// ===================================
		// Corner property
		// ===================================
		public Vector2[] Corners
		{
			get
			{
				if (!_cornersValid)
				{
					_corners = rotateVectors(baseCorners, Angle);
					_cornersValid = true;
				}
				return _corners;
			}
		}
		private Vector2[] _corners;
		bool _cornersValid = false;

		// ===================================
		// Normal property
		// ===================================
		public Vector2[] Normals
		{
			get
			{
				if (!_normalsValid)
				{
					_normals = rotateVectors(baseNormals, Angle);
					_normalsValid = true;
				}
				return _normals;
			}
		}
		private Vector2[] _normals;
		private bool _normalsValid = false;

		public OBCollider(Vector2 position, Vector2 size, float angle)
		{
			Position = position;
			Size = size;
			Angle = angle;

			LastCollision = new CollisionInfo();

			// Construct corner offsets
			float hWidth = Size.x / 2f;
			float hHeight = Size.y / 2f;

			Vector2 cornerA = new Vector2( hWidth,  hHeight);
			Vector2 cornerB = new Vector2(-hWidth,  hHeight);
			Vector2 cornerC = new Vector2(-hWidth, -hHeight);
			Vector2 cornerD = new Vector2( hWidth, -hHeight);
			baseCorners = new Vector2[4] { cornerA, cornerB, cornerC, cornerD };

			// Construct normals
			// NOTE: Normals for a box are very easy but for studying purposes the normals are defined by the edges instead. This will be changed later.
			Vector2 normalA = (cornerA - cornerB).Normal();
			Vector2 normalB = (cornerB - cornerC).Normal();
			Vector2 normalC = (cornerC - cornerD).Normal();
			Vector2 normalD = (cornerD - cornerA).Normal();
			baseNormals = new Vector2[4] { normalA, normalB, normalC, normalD };
		}

		public bool Overlapping(ICollider other)
		{
			if (other is OBCollider) return Overlapping(other as OBCollider);
			else return false;
		}

		private bool Overlapping(OBCollider other)
		{
			CollisionInfo bestCol = null;

			int currentCheck = 0;

			foreach (Vector2 norm in Normals)
			{
				currentCheck++;

				bool overlaps = OverlappingOnAxis(norm, other, out CollisionInfo col);
				if (bestCol == null || col.PenetrationDepth < bestCol.PenetrationDepth) bestCol = col;
				if (!overlaps)
				{
					Console.WriteLine("Broke out of intersection check at check #{0}", currentCheck);
					return false;
				}
			}
			foreach (Vector2 norm in other.Normals)
			{
				currentCheck++;

				bool overlaps = other.OverlappingOnAxis(norm, this, out CollisionInfo col);
				if (bestCol == null || col.PenetrationDepth < bestCol.PenetrationDepth) bestCol = col;
				if (!overlaps)
				{
					Console.WriteLine("Broke out of intersection check at check #{0}", currentCheck);
					return false;
				}
			}

			LastCollision = bestCol;
			return true;
		}

		public bool OverlappingOnAxis(Vector2 axis, OBCollider other, out CollisionInfo colInfo)
		{
			float pDepth;
			Vector2 normal;
			bool collides;

			axis.Normal();

			(Vector2 vMinThis, float pMinThis, Vector2 vMaxThis, float pMaxThis) = MinMaxCorner(axis);
			(Vector2 vMinOther, float pMinOther, Vector2 vMaxOther, float pMaxOther) = other.MinMaxCorner(axis);
			
			if (pMinThis < pMinOther && pMaxThis > pMinOther)
			{
				// "this" is overlapping on the left(negative) side of "other"
				normal = axis;
				pDepth = pMaxThis - pMinOther;
				collides = true;
			}
			else if (pMinOther < pMinThis && pMaxOther > pMinThis)
			{
				// "this" is overlapping on the right(positive) side of "other"
				normal = -axis;
				pDepth = pMaxOther - pMinThis;
				collides = true;
			}
			else
			{
				normal = new Vector2();
				pDepth = 0;
				collides = false;
			}
			if (Angle == other.Angle)
			{
				normal = -normal;
			}

			colInfo = new CollisionInfo(normal, pDepth);
			return collides;
		}

		// Axis should be normalized
		public (Vector2 vMin, float pMin, Vector2 vMax, float pMax) MinMaxCorner(Vector2 axis)
		{
			Vector2 vMin = new Vector2();
			float pMin = float.PositiveInfinity;
			Vector2 vMax = new Vector2();
			float pMax = float.NegativeInfinity;

			for (int i = 0; i < Corners.Length; i++)
			{
				Vector2 v = Corners[i] + Position;
				float p = Vector2.Dot(v, axis);

				if (p < pMin)
				{
					pMin = p;
					vMin = v;
				}
				if (p > pMax)
				{
					pMax = p;
					vMax = v;
				}
			}

			return (vMin, pMin, vMax, pMax);
		}

		private Vector2[] rotateVectors(Vector2[] vecs, float angle)
		{
			// Snippet from rotation method
			// float xComp = Cos(angle) * x - Sin(angle) * y;
			// float yComp = Sin(angle) * x + Cos(angle) * y;
			angle = Vector2.Deg2Rad(angle);
			float cosA = Cos(angle);
			float sinA = Sin(angle);

			Vector2[] result = new Vector2[vecs.Length];
			for (int i = 0; i < vecs.Length; i++)
			{
				float x = vecs[i].x;
				float y = vecs[i].y;

				result[i] = new Vector2(cosA * x - sinA * y, sinA * x + cosA * y);
			}

			return result;
		}

		public void DrawNormals(EasyDraw ed)
		{
			for (int i = 0;i < baseNormals.Length; i++)
			{
				Vector2 a = Position + (Corners[i] + Corners[(i + 1) % Corners.Length]) / 2f;
				Vector2 b = a + Normals[i] * 50;
				ed.Line(a.x, a.y, b.x, b.y);
			}
		}
		public void DrawCorners(EasyDraw ed)
		{
			foreach (Vector2 corner in Corners)
			{
				Vector2 a = Position + corner;
				ed.Ellipse(a.x, a.y, 10, 10);
			}
		}
		public void DrawOverlapOnAxis(EasyDraw ed, Vector2 axis)
		{
			axis.Normalize();
			var minmax = MinMaxCorner(axis);

			//axis.y = -axis.y;

			Vector2 a = axis * minmax.pMin;
			Vector2 b = axis * minmax.pMax;
			ed.Line(a.x, a.y, b.x, b.y);
		}

		private void Invalidate()
		{
			_cornersValid = false;
			_normalsValid = false;
		}
	}
}
