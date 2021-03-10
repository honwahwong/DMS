using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Domain.Models
{
    public class DmsFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public long FileSize { get; set; }
        public byte[] FileContent { get; set; }
    }
}
