using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using haverchuck.playlist.Models;

namespace haverchuck.playlist.Drivers
{
    public class AudioDriver : ContentPartDriver<AudioPart>
    {
        protected override DriverResult Display(AudioPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Audio", () => shapeHelper.Parts_Audio(
                    Length: part.Length,
                    File: part.File,
                    TrackNumber: part.TrackNumber
                ));
        }

        // GET
        protected override DriverResult Editor(AudioPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Audio_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Audio", Model: part, Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(AudioPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }    
    }
}
