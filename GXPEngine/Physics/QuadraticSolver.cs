using static GXPEngine.Mathf;

namespace GXPEngine.Physics
{
	internal static class QuadraticSolver
	{
		public static float GetDiscriminant(float a, float b, float c)
		{
			return (b * b) - (4 * a * c);
		}
		public static (float rootA, float rootB) GetRoots(float discriminant, float a, float b)
		{
			float sqrtDiscriminant = Sqrt(discriminant);
			float rootA = (-b + sqrtDiscriminant) / (2 * a);
			float rootB = (-b - sqrtDiscriminant) / (2 * a);
			return (rootA, rootB);
		}
	}
}
