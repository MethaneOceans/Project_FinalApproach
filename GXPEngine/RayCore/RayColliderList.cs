using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.RayCore
{
	internal class RayColliderList : IRayCollider
	{
		public List<IRayCollider> Colliders;

		public RayColliderList()
		{
			Colliders = new List<IRayCollider>();
		}

		public HitRecord RayCast(Ray ray)
		{
			HitRecord rec = new HitRecord()
			{
				t = float.PositiveInfinity,
			};

			foreach (IRayCollider collider in Colliders)
			{
				HitRecord tempRec = collider.RayCast(ray);
				if (!tempRec.Hit) continue;
				if (tempRec.t < rec.t) rec = tempRec;
			}

			return rec;
		}
	}
}
