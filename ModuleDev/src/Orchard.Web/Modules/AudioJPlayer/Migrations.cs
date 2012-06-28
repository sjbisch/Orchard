using System;
using System.Collections.Generic;
using System.Data;
using AudioJPlayer.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Core.Contents.Extensions;
using Orchard.Core.Navigation.Models;
using Orchard.Data.Migration;

namespace AudioJPlayer {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            SchemaBuilder.CreateTable("AudioRecord", table => table
                .ContentPartRecord()
                .Column<int>("Length")
                .Column<string>("File", column => column.WithLength(50))
                .Column<int>("TrackNumber")
                );

            ContentDefinitionManager.AlterPartDefinition(typeof(AudioPart).Name, part => part
                .Attachable()
                );

            ContentDefinitionManager. AlterTypeDefinition("Playlist", type => type
                .WithPart(typeof(ContainerPart).Name)
                .WithPart(typeof(BodyPart).Name)
                .WithPart(typeof(CommonPart).Name)
                .WithPart(typeof(MenuPart).Name)
                .WithPart(typeof(AdminMenuPart).Name)
                .Creatable()
                );

            ContentDefinitionManager.AlterTypeDefinition("Song", type => type
                .WithPart(typeof(ContainerPart).Name)
                .WithPart(typeof(MenuPart).Name)
                .WithPart(typeof(AudioPart).Name)
                .WithPart(typeof(ContainablePart).Name)
                .Creatable()
                );

            return 1;
        }
    }
}