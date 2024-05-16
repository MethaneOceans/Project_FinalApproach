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

	public MyGame() : base(1600, 900, pFullScreen: false, pPixelArt: false)
	{
		sceneManager = new SceneManager(this);
		sceneManager.AddScene("Level0", new TestLevel());
		sceneManager.AddScene("NineSlice", new NineSliceTest());

		targetFps = int.MaxValue;

		// Show the fps
		fpsCounter = new EasyDraw(200, 50);
		fpsCounter.TextAlign(CenterMode.Min, CenterMode.Min);
		AddChild(fpsCounter);
		sceneManager.SwitchScene("Level0");

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
		if (Input.GetKeyDown(Key.R)) sceneManager.Reload();

		if (Input.GetKeyDown(Key.ONE)) sceneManager.SwitchScene("Level0");
		if (Input.GetKeyDown(Key.TWO)) sceneManager.SwitchScene("NineSlice");

	}
	static void Main()
	{
		new MyGame().Start();
	}
}