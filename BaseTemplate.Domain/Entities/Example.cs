﻿using BaseTemplate.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Domain.Entities
{
    public class Example : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
