
namespace Tulip.CLI
{
    class ArgumentParser
    {
        public Queue<string> Args { get; } = new Queue<string>();
        public Dictionary<string, string> Kwargs { get; } = new Dictionary<string, string>();
        public ArgumentParser(string[] cliArgs)
        {
            if (cliArgs.Length == 0)
            {
                throw new ArgumentException("No arguments provided");
            }

            string? keyword = null;
            for (var i = 0; i < cliArgs.Length; i++)
            {
                var arg = cliArgs[i];
                if (arg.StartsWith("-"))
                {
                    if (keyword != null)
                    {
                        throw new ArgumentException($"Two keywords in a row: {keyword} {arg}");
                    }

                    keyword = arg;
                    continue;
                }

                if (keyword != null)
                {
                    Kwargs.Add(keyword, arg);
                    keyword = null;
                }
                else 
                {
                    Args.Enqueue(arg);
                }
            }
        }
    }
}