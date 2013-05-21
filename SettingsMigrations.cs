using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class SettingsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            // Well, nothing we don't know alread, right?
            SchemaBuilder.CreateTable(typeof(DemoSettingsPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("Message", column => column.WithLength(2048))
            );


            return 1;
        }

        // NEXT STATION: Handlers/DemoSettingsPartHandler!
    }
}