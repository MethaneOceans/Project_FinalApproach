using System;                                   // System contains a lot of default C# libraries 
using System.Drawing;
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Control;
using GXPEngine.Scenes;                           // System.Drawing contains drawing tools such as Color definitions

/// <summary>
/// This MyGame class only contains the scenemanager and setup for the scenemanager.
/// The only available scenes are test scenes
/// </summary>
public class MyGame : Game {
	private readonly SceneManager sceneManager;
	private readonly EasyDraw fpsCounter;

	public MyGame() : base(1600, 900, pFullScreen: false, pPixelArt: true)
	{
		sceneManager = new SceneManager(this);
		sceneManager.AddScene("CollisionTest", new CollisionTest());
		sceneManager.AddScene("RayTest", new RayTest());
		sceneManager.AddScene("CircleTest", new CircleTest());
		sceneManager.AddScene("PrismTest", new PrismTest());
		sceneManager.AddScene("ProtoTest", new ProtoTest());

		// Show the fps
		fpsCounter = new EasyDraw(200, 50);
		fpsCounter.TextAlign(CenterMode.Min, CenterMode.Min);
		AddChild(fpsCounter);
		sceneManager.SwitchScene("CollisionTest");

		Console.WriteLine("MyGame initialized");
	}

	void Update() {
		HandleInput();

		fpsCounter.ClearTransparent();
		fpsCounter.Fill(Color.Green);
		fpsCounter.Text($"{currentFps}");
	}

	private void HandleInput()
	{
		if (Input.GetKeyDown(Key.R))
		{
			sceneManager.Reload();
		}

		if (Input.GetKeyDown(Key.ONE)) sceneManager.SwitchScene("CollisionTest");
		if (Input.GetKeyDown(Key.TWO)) sceneManager.SwitchScene("RayTest");
		if (Input.GetKeyDown(Key.THREE)) sceneManager.SwitchScene("CircleTest");
		if (Input.GetKeyDown(Key.FOUR)) sceneManager.SwitchScene("PrismTest");
		if (Input.GetKeyDown(Key.FIVE)) sceneManager.SwitchScene("ProtoTest");
		if (Input.GetKeyDown(Key.SIX))
		{

		}
		if (Input.GetKeyDown(Key.SEVEN))
		{

		}
		if (Input.GetKeyDown(Key.EIGHT))
		{

		}
		if (Input.GetKeyDown(Key.NINE))
		{

		}
		if (Input.GetKeyDown(Key.ZERO))
		{

		}
	}
	static void Main()
	{
		new MyGame().Start();
	}
}