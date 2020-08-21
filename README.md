# megamealplanner-prototype
Early attempt at an ASP .NET based site using databases. Tracks meals, ingredients, calories etc...

Credit goes to Tiffanie Juracka for creating the graphics.

You'll need to install SQL express on your machine, or point the connection strings to another database server. Two databses are used: aspnetdb and weightloss_website.

To get this working:

1) Install the .NET Framework SDK 4.5 or later
2) Use the aspnet_regsql.exe command to generate the aspnetdb database
3) Create the weightloss_website using SSMS or sqlcmd and use the foodData.edmx.sql file to create the tables and schema
4) Use the website to create a user. You'll be making this user an admin in the next step.
5) Use the stored proc aspnet_Roles_CreateRole to create the admin role. Then use the stored proc aspnet_UsersInRoles_AddUsersToRoles to assign the user from step #4 to the admin role. This user will now have the ability to change user accounts and change meal ingredients after folliwng step #6
6) Use SSMS or sqlcmd to add an ingredient to the dbo.Ingredients table. Until you do this, the ingredients admin page will only show a blank placeholder and you'll be unable to manipulate or add ingredients to meals on your own.

That's it, shoot me a message if you have any questions!
