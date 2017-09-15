using System;
using System.Linq;
using System.Reflection;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace Chat.Client.ConsoleApp.Options.People
{
    [Verb("people")]
    class PeopleOption: RootOptionBase
    {
        [Value(0, Required = true)]
        public long PeopleId { get; set; }
        
        protected override string Str => $"People {PeopleId}";
        protected override Type BaseOptionType => typeof(PeopleOptionBase);
        protected override void AfterParsed(OptionBase opt, Program app)
        {
            base.AfterParsed(opt, app);
            ((PeopleOptionBase)opt).PeopleId = PeopleId;
        }

        internal override void Execute(Program app)
        {
            ReadCommands(app);
        }
    }

    abstract class PeopleOptionBase : OptionBase
    {
        public long PeopleId { get; set; }
    }
}