The food ordering project is composed of two main components: an ASP.NET Core MVC web application and a web APIs project.

The web application serves as an admin dashboard that enables CRUD operations on the food ordering system’s entities.

The web APIs project consists of four sub-projects:
  - The first project is an ASP.NET Core Web API project (API), which contains the controllers that expose the APIs or endpoints for the customers to order food.
  - The second project is a class library project (core), which handles the data access and persistence by including the entities, the database context classes, and the migrations.
  - The third project is a class library project (infrastructure), which implements the business logic and data manipulation by using repository and unit of work interfaces and their concrete classes, as well as the specification pattern.
  - The forth project is a class library project (services), which provides any logic related to the controllers.

The project adheres to the Unit Of Work and Repository patterns to manage the data access and execute any business logic.

The project employs the specification pattern to enable the customers to search, filter, sort, and view food items from various categories.

The project also offers features such as authentication and authorization with custom tokens, pagination, data seeding, data caching with Redis and a user-defined cache attribute, custom exception middleware, custom response handler, and online payment with stripe payment gateway.
