using GXPEngine.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GXPEngine.Control
{
    internal class SceneManager
    {
        private Dictionary<string, Scene> SceneMap = new Dictionary<string, Scene>();
        private Scene currentScene;
        private GameObject parent;

        public SceneManager(GameObject game)
        {
            parent = game;
        }

        public void AddScene(string sceneKey, Scene scene)
        {
            SceneMap.Add(sceneKey, scene);
        }
        public void RemoveScene(string scene)
        {
            SceneMap.Remove(scene);
        }
        public Scene GetScene(string sceneKey) => SceneMap[sceneKey];

        public void SwitchScene(string scene)
        {
            Scene newScene = GetScene(scene);

            // Remove current scene from hierarchy
            if (currentScene != null) currentScene.Unload();

            // Add new scene to hierarchy
            newScene.Load(parent);
            currentScene = newScene;
        }
        public void Reload()
        {
            currentScene.Initialize();
        }
    }
}
