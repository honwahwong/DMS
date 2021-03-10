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
        public Task<int> AddFileAsync(DmsFile dmsFile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckFileExistAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<DmsFile> GetFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DmsFileView>> GetFileViewsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
