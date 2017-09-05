using System;
namespace Chat.Client.ConsoleApp.Options
{
    abstract class OptionBase
    {
        internal virtual void Execute(Program app)
        {
            throw new NotImplementedException();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    class CommandAttribute: Attribute
    {
        public string Name { get; set; }

        public CommandAttribute (string name)
        {
            Name = name;
        }
    }
}
