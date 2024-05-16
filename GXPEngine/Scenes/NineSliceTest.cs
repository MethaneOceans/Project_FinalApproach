using GXPEngine.Control;

namespace GXPEngine.Scenes
{
	internal class NineSliceTest : Scene
	{
		NineSlice slice;

		public override void Initialize()
		{
			base.Initialize();

			slice = new NineSlice("Textures/9Slice_Test.jpg", 1000, 600);
			AddChild(slice);
		}

		public void Update()
		{
			slice.Rotation++;
		}
	}
}
