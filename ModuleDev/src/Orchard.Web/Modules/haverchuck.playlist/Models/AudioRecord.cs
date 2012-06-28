using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace haverchuck.playlist.Models
{
    public class AudioRecord : ContentPartRecord
    {
        public virtual int Length { get; set; }
        public virtual string File { get; set; }
        public virtual int TrackNumber { get; set; } 
    }
}
