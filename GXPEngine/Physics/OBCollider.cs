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
		public CollisionInfo LastCollision;
		// Size field can't change, this to prevent unpredictable behavior.
		public readonly Vector2 Size;

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

		// ===================================
		// Corners property
		// ===================================
		public Vector2[] Corners
		{
			get
			{
				if (!_cornersValid)
				{
					_corners = Vector2.RotateVectorsDeg(baseCorners, Angle);
					_cornersValid = true;
				}
				return _corners;
			}
		}
		private readonly Vector2[] baseCorners;
		private Vector2[] _corners;
		bool _cornersValid = false;
		// ===================================
		// Normals property
		// ===================================
		public Vector2[] Normals
		{
			get
			{
				if (!_normalsValid)
				{
					_normals = Vector2.RotateVectorsDeg(baseNormals, Angle);
					_normalsValid = true;
				}
				return _normals;
			}
		}
		private readonly Vector2[] baseNormals;
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

		// Overlap test specifically for other boxes
		private bool Overlapping(OBCollider other)
		{
			CollisionInfo bestCol = null;
			int currentCheck = 0;

			// Axis checks for this box
			for (int i = 0; i < Normals.Length; i++)
			{
				currentCheck++;
				Vector2 norm = Normals[i];

				bool overlaps = OverlappingOnAxis(norm, other, out CollisionInfo col);
				if (bestCol == null || col.PenetrationDepth < bestCol.PenetrationDepth) bestCol = col;
				if (!overlaps) return false;
			}
			// Axis checks for the other box
			for (int i = 0; i < other.Normals.Length; i++)
			{
				currentCheck++;
				Vector2 norm = other.Normals[i];

				bool overlaps = other.OverlappingOnAxis(norm, this, out CollisionInfo col);
				if (bestCol == null || col.PenetrationDepth < bestCol.PenetrationDepth) bestCol = col;
				if (!overlaps) return false;
			}

			LastCollision = bestCol;
			return true;
		}

		// Checks if the box overlaps with other box along a given axis
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

		/// <summary>
		/// Gets the minimum and maximum points along a given axis
		/// </summary>
		/// <param name="axis">Axis to compare the points on</param>
		/// <returns>A tuple containing the min and max vectors and projected values</returns>
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

		// =====================================================
		// Property management methods
		// =====================================================
		private void Invalidate()
		{
			_cornersValid = false;
			_normalsValid = false;
		}

		// =====================================================
		// Debugging methods
		// =====================================================
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
	}
}
