
namespace Tulip.CLI
{

    interface ICommand
    {
        public static string Name { get; }
        public static string Help { get; }
        public ICommand Configure(List<string> args, Dictionary<string, string> kwargs);
        public void Execute();
    }
}