using System;
using System.Collections.Generic;

namespace Playground.Data.Entities
{
    public class Diff
    {
        public int Id { get; set; }
        public int ChangeId { get; set; }

        public string Type { get; set; }
        public string Previous { get; set; }
        public string Proposed { get; set; }

        public Change Change { get; set; }

    }
}