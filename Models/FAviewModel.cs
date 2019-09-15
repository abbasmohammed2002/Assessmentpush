using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessments.Models
{
    public class FAviewModel
    {
        public List<string> FAname { get; set; }

        public FAviewModel()
        {
            this.FAname = new List<string>();
        }

    }
}
