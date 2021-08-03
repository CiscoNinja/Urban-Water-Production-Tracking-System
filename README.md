**Project Description**

Web app integrated with Electromagnetic Water Flow Meters. This web app receives water production data via text messages using SMS Enabler, directly from electromagnetic sensors (with SIM cards installed) into application database and also retrieves data saved in excel format into application database.

**Frameworks - Libraries**

* ASP.NET Web API
* Entity Framework
* Autofac
* Automapper
* FluentValidation
* AngularJS
* Bootstrap 3
* PostSharp
* 3rd part libraries


**Installation instructions**

 1. Build solution to restore packages
 2. Rebuild solution
 3. Change the connection strings inside the GwcltdApp.Data/App.config and   GwcltdApp.Web/Web.config according to your development environment
 4. Open Package Manager Console and
   Select GwcltdApp.Data as Default project (in package manager console) and run the following commands
   
     * add-migration "initial"
     * update-database -verbose  this will run the seed method i created to test the app
   
5. Run GwcltdApp.Web application

<h1>Images</h1>
<img src="Screenshot (144).png"/>
<img src="Screenshot (143).png"/>
<img src="Screenshot (146).png"/>
<img src="Screenshot (147).png"/>
<img src="Screenshot (149).png"/>
