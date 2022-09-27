using System;

namespace AspNetCore.Identity.FileSystem
{
    public class FsRole
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
