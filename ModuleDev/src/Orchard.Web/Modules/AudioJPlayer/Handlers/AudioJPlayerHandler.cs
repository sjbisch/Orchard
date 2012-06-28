using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioJPlayer.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace AudioJPlayer.Handlers
{
    public class AudioJPlayerHandler : ContentHandler
    {
        public AudioJPlayerHandler(IRepository<AudioRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
