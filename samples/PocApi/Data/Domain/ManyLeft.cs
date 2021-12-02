using System.Collections.Generic;

namespace PocApi.Data.Domain
{
    public class ManyLeft
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<ManyRight> Rights { get; set; }
    }
}
