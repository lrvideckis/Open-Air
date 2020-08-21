
** The Repos directory holds the classes for the repository pattern for each entity in the data model.
The repository pattern basically adds a layer of abstraction between the database calls and the business
logic in the OpenAir.Web project. This way, the .Web project doesn't need to know anything about LINQ
or the DbContext used in database managment-- that should be the job of the .Data project.