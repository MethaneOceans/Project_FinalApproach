namespace GXPEngine.Physics
{
	internal enum ColliderType
	{
		// Isn't affected by physics but objects can hit it.
		Static,
		//  Is affected by physics and objects can hit it.
		Rigid,
		// Isn't affected by objects but objects overlapping this at the end of a step will invoke whatever trigger it has.
		Ghost,
	}
}
