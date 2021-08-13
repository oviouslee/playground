using System;
using System.Collections.Generic;

namespace Playground.Data.Entities
{
    public class Change
    {
        public int Id { get; set; }
        public bool isApproved { get; set; }

        public IEnumerable<Diff> Diffs { get; set; }
    }
}