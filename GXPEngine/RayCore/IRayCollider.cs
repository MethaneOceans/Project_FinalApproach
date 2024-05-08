namespace GXPEngine
{
	internal interface IRayCollider
	{
		HitRecord RayCast(Ray ray);
	}
}
