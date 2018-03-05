using CommandConsole.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommandConsole
{
    public class EssentialCommands
    {
        [ConsoleCommand(Name = "loadscene_id", Global = true, ReturnMessage = "Scene Loaded")]
        public void LoadScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }

        [ConsoleCommand(Name = "loadscene", Global = true, ReturnMessage = "Scene Loaded")]
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        [ConsoleCommand(Name = "quit", Global = true)]
        public void QuitGame()
        {
            Application.Quit();
        }

        [ConsoleCommand(Name = "timescale",Global = true, ReturnMessage = null)]
        public void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
            Console.Instance.Log(string.Format("TimeScale set to {0}", timeScale));
        }

        [ConsoleCommand(Name = "gravity", Global = true, ReturnMessage = null)]
        public void SetGravity(Vector3 gravity)
        {
            Physics.gravity = gravity;
            Console.Instance.Log(string.Format("Gravity set to {0}", gravity));
        }
    }
}
