using CommandConsole;
using CommandConsole.Attributes;
using CommandConsole.Parameters;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private void Start()
    {
        //Register the class.
        Console.Instance.RegisterClass<Cube>(this, gameObject.name.Replace(" ", ""));
    }

    private void OnDestroy()
    {
        //Deregister the class after Destroy otherwise the console will not work properly after Scene switch
        //alternative you can call Console.CreateNew() to reset the console on scene switch
        Console.Instance.DeregisterClass<Cube>(this, gameObject.name.Replace(" ", ""));
    }

    //Mark the Method with the ConsoleCommand to use it in the console
    [ConsoleCommand]
    public void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("_Color", color);
        
    }

    //You can also alter the return type. Set it to null to return nothing after the call
    [ConsoleCommand(ReturnMessage = null)]
    public void Move(Vector3 newPosition)
    {
        transform.position = newPosition;
        Console.Instance.Log(string.Format("Position changed to {0}", newPosition));
    }

    //When you have Method overloading you need to define a name different to the other method
    //object is not supported as a parameter but you can define with the ConsolePatameter Attribute a note type to use
    [ConsoleCommand(Name = "MoveLocal")]
    public void Move([ConsoleParameter(ParameterType = typeof(Vector3Parameter))]object newPosition)
    {
        Move(transform.position + (Vector3)newPosition);
    }

    //When you register the class by Default you need to write ClassName.MethodName when you set the Global Variable
    //to true then you need to call the method without the class name.
    [ConsoleCommand(Global = true)]
    public void Scale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }
}
