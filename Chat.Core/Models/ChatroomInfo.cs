using System.Collections.Generic;
using System.Linq;

namespace Chat.Core.Models
{
    public partial class ChatroomInfo
    {
        public IEnumerable<long> PeopleIds => peoples_.Select(p => p.Id);
    }
}