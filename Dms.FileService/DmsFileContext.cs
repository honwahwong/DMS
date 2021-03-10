using Dms.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.FileService
{
    public class DmsFileContext : DbContext
    {
        public DbSet<DmsFile> DmsFiles { get; set; }
        public DmsFileContext()
        {
        }

        public DmsFileContext(DbContextOptions<DmsFileContext> options)
            : base(options)
        {
        }

    }
}
