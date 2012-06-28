using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using haverchuck.playlist.Models;

namespace haverchuck.playlist.Handlers
{
    public class AudioHandler : ContentHandler
    {
        public AudioHandler(IRepository<AudioRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
