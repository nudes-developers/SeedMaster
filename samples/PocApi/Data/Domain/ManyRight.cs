using System.Collections.Generic;

namespace PocApi.Data.Domain
{
    public class ManyRight
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<ManyLeft> Lefts { get; set; }
    }
}
