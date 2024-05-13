namespace GXPEngine.Physics
{
	internal interface ICollider
	{
		bool Overlapping(ICollider other);
		float RayCast(Ray ray);
		Vector2 NormalAt(Vector2 point);
	}
}
