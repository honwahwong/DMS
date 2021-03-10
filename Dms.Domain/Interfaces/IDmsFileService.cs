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
        Task<bool> CheckFileExistAsync(string fileName);
        Task<int> AddFileAsync(DmsFile dmsFile);
        Task<DmsFile> GetFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
