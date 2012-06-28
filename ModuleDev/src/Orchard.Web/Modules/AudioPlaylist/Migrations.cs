using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Core.Contents.Extensions;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Title.Models;
using Orchard.Data.Migration;

namespace AudioPlaylist {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            ContentDefinitionManager.AlterPartDefinition("Audio", part => part
                 .WithField("Mp3File", field => field.OfType("TextField"))
                 .WithField("OggFile", field => field.OfType("TextField"))
                 .WithField("Length", field => field.OfType("NumericField"))
                 .WithField("SequenceNumber", field => field.OfType("NumericField"))
                 .Attachable()
                 );
            


            ContentDefinitionManager.AlterTypeDefinition("Audio", type => type
                .WithPart(typeof(ContainablePart).Name)
                .WithPart(typeof(BodyPart).Name)
                .WithPart(typeof(CommonPart).Name)
                .WithPart(typeof(TitlePart).Name)
                .WithPart("Audio")
                .Creatable()
                );


            ContentDefinitionManager.AlterTypeDefinition("Playlist", type => type
               .WithPart(typeof(ContainerPart).Name)
               .WithPart(typeof(BodyPart).Name)
               .WithPart(typeof(CommonPart).Name)
               .WithPart(typeof(MenuPart).Name)
               .WithPart(typeof(AdminMenuPart).Name)
               .WithPart(typeof(TitlePart).Name)
               .Creatable()
               );

            return 1;
        }
    }
}