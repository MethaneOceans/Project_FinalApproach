using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Physics.Colliders
{
	internal class LineSegmentCollider : Collider
	{
		public Vector2 Begin;
		public Vector2 End;
		public Vector2 AsVec
		{
			get => End - Begin;
		}

		private bool lengthChanged = true;
		private float length;
		public float Length
		{
			get
			{
				if (lengthChanged)
				{
					length = AsVec.Length();
				}
				return length;
			}
		}

		public readonly new bool Static = true;

		public LineSegmentCollider(Vector2 begin, Vector2 end)
		{
			Begin = begin;
			End = end;

			base.Static = true;
		}

		public override CollisionInfo GetCollisionInfo(Collider other)
		{
			throw new NotImplementedException();
		}

		public override bool Overlaps(Collider other)
		{
			throw new NotImplementedException();
		}
	}
}
