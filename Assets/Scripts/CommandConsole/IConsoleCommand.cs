using CommandConsole.ConsoleParser;

namespace CommandConsole
{
    public interface IConsoleCommand
    {
        string CommandName { get;}
        string ReturnMessage { get; set; }
        void Execute(params IValue[] arguments);
        string GetCommandSyntax();
    }
}
