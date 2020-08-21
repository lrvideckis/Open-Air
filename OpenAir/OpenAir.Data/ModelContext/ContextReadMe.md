DESCRIPTION:

The DbContext is a class that comes with EntityFrameworkCore. This class allows for easy
database manipulation without having to write your own SQL code in a database managment 
software system. It is still nice to use SQL Server Managment Studio to verify changes
made to the database, however. The structure in OpenAirDbContext is setup in such a way
that a database is created (if its not already there) and maintained with tables
corresponding to the DbSets. The DbSets are a container type that usually hold a class
also in the data model.

A migration is something that entity framework does to create or update a database.
There is a connection string in the context to point to the server that the db should
exist on. When the migration occurs. It looks at the db sets in the context. Then,
it looks at the classes in those sets. Tables are created with attributes matching
those in the classes referenced in the db set.

In addition, there are context maps that aid in the model creating. This is where
conditions are set like primary key and not null. Finally, in the db context, there
is a function called onModelCreate(). This is where the context maps are called and
any database seeding is done. Database seeding is just having entity framework put 
some data entries in the tables it just created.

MIGRATIONS:

**When trying to perform a migration, in the package Manager Console (if you don't see
the package manager, just search for it in the search bar or look for in in the view 
menu drop down) use:

PM> EntityFrameworkCore\Add-Migration
name: <the name of your migration>


**After this , you will need to update the database. Use:

PM> EntityFrameworkCore\Update-Database

You will see some verbose text output (you can see the create tables and inserts here) in
the package maganger console. At this point (given there are no errors),
the database should currently reflect the class structure that is setup in OpenAirDbContext.cs


**Also, if you need some quick help in the package manager use:

PM> Get-Help about_EntityFrameworkCore

This should output the commands for EntityFrameworkCore in the package manager.

**More documentation can be found here: https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell#add-migration
