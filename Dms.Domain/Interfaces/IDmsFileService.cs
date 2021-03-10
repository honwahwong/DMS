using Dms.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Domain.Interfaces
{
    public interface IDmsFileService
    {
        Task<IEnumerable<DmsFileView>> GetFileViewsAsync();
    }
}
