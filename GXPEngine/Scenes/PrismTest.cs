using GXPEngine.Control;

namespace GXPEngine.Scenes
{
	internal class PrismTest : Scene
	{
		public override void Initialize()
		{
			base.Initialize();
			Prism prism = new Prism()
			{
				Position = new Vector2(100, 700),
			};
			AddChild(prism);
		}
	}
}
