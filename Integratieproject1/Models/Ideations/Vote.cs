﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Ideations
{
    public class Vote
    {
        public int voteNr { get; set; }
        public Boolean confirmed { get; set; }
        public VoteType voteType { get; set; }
    }
}
