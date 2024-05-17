using System;                                   // System contains a lot of default C# libraries 
using System.Drawing;
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Control;
using GXPEngine.Scenes;                           // System.Drawing contains drawing tools such as Color definitions

/// <summary>
/// This MyGame class only contains the scenemanager and setup for the scenemanager.
/// The only available scenes are test scenes
/// </summary>
internal class MyGame : Game {
	public readonly SceneManager sceneManager;
	private readonly EasyDraw fpsCounter;
    Sprite bg;

    public float SoundVolume = 0.05f;

	public MyGame() : base(1600, 900, pFullScreen: false, pPixelArt: false)
	{
        bg = new Sprite("bg.png");
        sceneManager = new SceneManager(this);
		sceneManager.AddScene("Level0", new TestLevel());
		//sceneManager.AddScene("NineSlice", new NineSliceTest());
        sceneManager.AddScene("Level1", new LevelOne());
        sceneManager.AddScene("Level2", new LevelTwo());
        sceneManager.AddScene("Level3", new LevelThree());
        this.AddChild(bg);

        targetFps = int.MaxValue;

		// Show the fps
		fpsCounter = new EasyDraw(200, 50);
		fpsCounter.TextAlign(CenterMode.Min, CenterMode.Min);
		AddChild(fpsCounter);
		sceneManager.SwitchScene("Level1");


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

		//if (Input.GetKeyDown(Key.ONE)) sceneManager.SwitchScene("Level0");
		//if (Input.GetKeyDown(Key.TWO)) sceneManager.SwitchScene("NineSlice");
        if (Input.GetKeyDown(Key.ONE)) sceneManager.SwitchScene("Level1");

        if (Input.GetKeyDown(Key.TWO)) sceneManager.SwitchScene("Level2");

        if (Input.GetKeyDown(Key.THREE)) sceneManager.SwitchScene("Level3");
    }
	static void Main()
	{
		new MyGame().Start();
	}
}