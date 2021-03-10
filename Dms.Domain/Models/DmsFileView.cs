using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Domain.Models
{
    public class DmsFileView
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public long FileSize { get; set; }
    }
}
