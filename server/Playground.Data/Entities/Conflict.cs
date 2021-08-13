using System;
using System.Collections.Generic;

namespace Playground.Data.Entities
{
    public class Conflict
    {
        public int Id { get; set; }

        public int OriginDiffId { get; set; }
        public int TargetDiffId { get; set; }

    }
}