
<h3 style="font-weight:normal;">Frameworks - Libraries</h3>
<ol>
<li>ASP.NET Web API</li>
<li>Entity Framework</li>
<li>Autofac</li>
<li>Automapper</li>
<li>FluentValidation</li>
<li>AngularJS</li>
<li>Bootstrap 3</li>
<li>3rd part libraries</li>
</ol>

<h3 style="font-weight:normal;">Installation instructions</h3>
<ol>
<li>Build solution to restore packages</li>
<li>Rebuild solution</li>
<li>Change the connection strings inside the GwcltdApp.Data/App.config and GwcltdApp.Web/Web.config
   accoarding to your development environment</li>
<li>Open Package Manager Console</li>
<li>Select GwcltdApp.Data as Default project (in package manager console) and run the following commands
   <ol>
   <li>add-migration "initial"</li>
   <li>update-database -verbose</li>
   </ol></li>
<li>Run GwcltdApp.Web application</li>