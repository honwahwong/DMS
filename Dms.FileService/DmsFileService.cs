using Dms.Domain.Interfaces;
using Dms.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.FileService
{
    public class DmsFileService : IDmsFileService
    {
        private DmsFileContext _context;
        public DmsFileService(DmsFileContext context)
        {
            _context = context;
        }

        public async Task<int> AddFileAsync(DmsFile dmsFile)
        {
            _context.DmsFiles.Add(dmsFile);
            await _context.SaveChangesAsync();
            return dmsFile.Id;
        }

        public async Task<bool> CheckFileExistAsync(string fileName)
        {
            var count = await _context.DmsFiles.Where(x => x.Name == fileName).CountAsync();
            return count > 0;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var dmsFile = await _context.DmsFiles.FirstOrDefaultAsync(x => x.Name == fileName);
            _context.DmsFiles.Remove(dmsFile);
            await _context.SaveChangesAsync();
        }

        public async Task<DmsFile> GetFileAsync(string fileName)
        {
            return await _context.DmsFiles.FirstOrDefaultAsync(x => x.Name == fileName);
        }

        public async Task<IEnumerable<DmsFileView>> GetFileViewsAsync()
        {
            return await _context.DmsFiles
                .Select(x =>
                    new DmsFileView
                    {
                        Name = x.Name,
                        Location = x.Location,
                        FileSize = x.FileSize
                    })
                .ToListAsync();
        }
    }
}
