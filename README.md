**Frameworks - Libraries**

ASP.NET Web API
Entity Framework
Autofac
Automapper
FluentValidation
AngularJS
Bootstrap 3
3rd part libraries


**Installation instructions**

 1. Build solution to restore packages
 2. Rebuild solution
 3. Change the connection strings inside the GwcltdApp.Data/App.config and   GwcltdApp.Web/Web.config according to your development environment
 4. Open Package Manager Console and
   Select GwcltdApp.Data as Default project (in package manager console) and run the following   commands
   
   * add-migration "initial"
   * update-database -verbose  this will run the seed method i created to test the app
   
5. Run GwcltdApp.Web application