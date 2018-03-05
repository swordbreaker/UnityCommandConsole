using CommandConsole;
using CommandConsole.Attributes;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    private int i = 3;

    [SerializeField] private GameObject _cubePrefab;

    private void Start()
    {
        //Register the class.
        Console.Instance.RegisterClass<CubeSpawner>(this);
    }

    private void OnDestroy()
    {
        //Deregister the class after Destroy otherwise the console will not work properly after Scene switch
        //alternative you can call Console.CreateNew() to reset the console on scene switch
        Console.Instance.DeregisterClass<CubeSpawner>(this);
    }

    [ConsoleCommand(Global = true, ReturnMessage = null)]
    public void SpawnCube(Vector3 pos, Color color)
    {
        var cube = Instantiate(_cubePrefab, pos, Quaternion.identity);
        cube.name = "Cube" + ++i;
        cube.GetComponent<Cube>().ChangeColor(color);
        Console.Instance.Log(string.Format(cube.name + " spawned at" + pos));
    }
}
