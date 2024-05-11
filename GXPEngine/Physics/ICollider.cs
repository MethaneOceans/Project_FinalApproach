using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Physics
{
	internal interface ICollider<T>
	{
		bool Overlapping(T other);
	}
}
