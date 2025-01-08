using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Terui.Attributes;

namespace Terui
{
    public sealed class App
    {
        private readonly Dictionary<string, object> commands = [];
        public App()
        {

        }
        public App AddCommand(
            ICommand command)
        {
            commands.Add(command.Name, command);
            return this;
        }
        public async Task RunAsync(string[] args)
        {
            if (args.Length == 0)
            {
                Help(commands);
                return;
            }
            if (!commands.TryGetValue(args[0], out var command))
            {
                Help(commands);
                return;
            }
            var cmd = (ICommand)command;
            if (args.Length > 1)
            {
                var argIndex = 1;
                ICommand? subCmd = cmd.GetSubCommand(args[argIndex]);
                while (subCmd != null)
                {
                    cmd = subCmd;
                    subCmd = cmd.GetSubCommand(args[argIndex]);
                }
            }
            CheckArgs(args, cmd);
            await cmd.ExecuteAsync();
        }
        private static void CheckArgs(
            string[] args,
            ICommand command)
        {
            var unwrapped = Unwrap(args, command);
            var argsProperties = command.ArgsType.GetProperties();
            if (argsProperties.Length == 0)
                return;
            var attributes = argsProperties.SelectMany(c => c.GetCustomAttributes(false));

            foreach (var property in argsProperties)
            {
                ValidateOption(property, command, unwrapped.Options);
                ValidateArgument(property, command, unwrapped.Arguments);
            }
        }
        private static void ValidateOption(
            PropertyInfo property,
            ICommand command,
            string[] args
        )
        {
            var optionAttribute = property.GetCustomAttribute<OptionAttribute>();
            if (optionAttribute == null)
                return;
            var anyName = args.Any(a => a.Contains(optionAttribute.GetName()));

            var alias = optionAttribute.GetAlias();
            if (!anyName && !string.IsNullOrWhiteSpace(alias))
            {
                var anyAlias = args.Any(a => a.Contains(alias));
                if (!anyAlias)
                    return;
            }
            command.SetArg(property.Name, true);
        }
        private static void ValidateArgument(
            PropertyInfo property,
            ICommand command,
            string[] args)
        {
            var argAttribute = property.GetCustomAttribute<ArgumentAttribute>();
            if (argAttribute == null || args.Length < argAttribute.GetPosition())
                return;
            if (args.Length == 0 && !argAttribute.IsRequired())
                return;

            command.SetArg(property.Name, args[argAttribute.GetPosition()]);
        }
        private static UnwrappedArgs Unwrap(
            string[] args,
            ICommand command)
        {
            args = args[1..];
            var options = args.Where(a => a.Contains('-'));
            var arguments = args.Where(a => command.GetSubCommand(a) == null && !a.Contains('-'));
            return new UnwrappedArgs()
            {
                Options = options.ToArray(),
                Arguments = arguments.ToArray()
            };
        }
        private static void Help(Dictionary<string, object> commands)
        {
            Console.WriteLine("List of commands");
            Console.WriteLine();
            foreach (var command in commands)
            {
                Console.WriteLine(command.Key);
            }
        }
    }
}