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

			slice.SetOrigin(slice.Width / 2f, slice.Height / 2f);
			slice.Position = new Vector2(width / 2, height / 2);
		}

		public void Update()
		{
			if (Input.GetKey(Key.E)) slice.Rotation++;
			if (Input.GetKey(Key.Q)) slice.Rotation--;
		}
	}
}
