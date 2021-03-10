using Dms.Domain.Interfaces;
using Dms.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dms.FileService
{
    public class DmsFileService : IDmsFileService
    {
        public Task<IEnumerable<DmsFileView>> GetFileViewsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
