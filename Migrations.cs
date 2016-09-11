/*
 * Previously we've seen how to describe a record. We also have to declare how to store it as well. This is where we 
 * need migrations. (Again, make sure to read through this page: 
 * http://docs.orchardproject.net/Documentation/Understanding-data-access)
 * Migrations are automatically run by the framework. You can use them to describe DB schema changes.
 */
using System;
using Orchard.Data.Migration;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo
{
    // Migrations should derive from IDataMigration, but practically from DataMigrationImpl
    public class Migrations : DataMigrationImpl
    {
        // Migrations have Create() and UpdateFromX methods. When the module is first enabled the Create() is called so
        // it can set up DB tables.
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(PersonRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name", column => column.WithLength(500))
                    .Column<string>("Sex") // Best to store enums as strings
                    .Column<DateTime>("BirthDateUtc")
                    // An infinite string should have Unlimited() set!
                    .Column<string>("Biography", column => column.Unlimited())
                )
            .AlterTable(typeof(PersonRecord).Name,
                table => table
                    // You can create indices from AlterTable
                    .CreateIndex("Name", new string[] { "Name" }) // We index Name so we can retrieve by name faster
            );


            // Here we return the number of the migration. If there were no update methods we'd return one. But we have
            // one, see it for more details.
            return 2;
        }

        /*
         * This is an update method. It is used to modify the existing schema. Update methods will be run when the module 
         * was already enabled before and the create method was run.
         * The X in UpdateFromX is the number of the update (the method's name is conventional). It means: "run this 
         * update if the module's current migration version is X". This method will run if it's 1.
         */
        public int UpdateFrom1()
        {
            // The initial version of our module did not store the person's name! What a mistake. We've brought disgrace
            // to our families. We quickly fix the issue by pushing out an update that modifies the schema to add the
            // Name.
            SchemaBuilder.AlterTable(typeof(PersonRecord).Name,
                table => table
                    .AddColumn<string>("Name", column => column.WithLength(500))
                );

            /*
             * By returning 2 we can chain an UpdateFrom2 method to the list of updates and thus update the schema from 
             * this state later. Notice that Create() returns 2 as well: this is a good practice. Create() initially had 
             * a schema defined where Name was not present. We added it in an update to our module but we not only 
             * included it in UpdateFrom1() but also added it to Create()'s table creation logic. What does this mean? 
             * Systems already having our module installed will run this update method and modify the table. However 
             * where the already updated module is installed only Create() will run.
             */
            return 2;

            // NEXT STATION: Services/PersonManager
        }
    }
}