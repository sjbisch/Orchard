using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace AudioJPlayer.Models
{
    public class AudioPart : ContentPart<AudioRecord>
    {
        public int Length
        {
            get { return Record.Length; }
            set { Record.Length = value; }
        }

        public string File
        {
            get { return Record.File; }
            set { Record.File = value; }
        }

        public int TrackNumber
        {
            get { return Record.TrackNumber; }
            set { Record.TrackNumber = value; }
        }
    }
}
