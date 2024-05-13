using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Physics
{
	internal class CircleCollider : ICollider
	{
		public Vector2 NormalAt(Vector2 point)
		{
			throw new NotImplementedException();
		}

		public bool Overlapping(ICollider other)
		{
			throw new NotImplementedException();
		}

		public float RayCast(Ray ray)
		{
			throw new NotImplementedException();
		}
	}
}
