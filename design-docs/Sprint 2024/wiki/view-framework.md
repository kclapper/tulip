# [Understanding the UI Framework (#12)](https://github.com/kclapper/tulip/issues/12)

> [kclapper](https://github.com/kclapper)

The code base that was given to us at the beginning of this project
appeared to use multiple ASP.NET UI frameworks. This document is meant
to explain how this is currently set up.

## UI Framework Options

There appear to be three main options for building ASP.NET UIs: 
traditional MVC Views, Razor Pages, or Blazor.

### [Traditional MVC Views](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-8.0)

These views are written in a templating language called _Razor_. When a 
request comes in, the URL is mapped to a controller class. This class
then uses model data to populate the template in a view. The resulting
HTML is then sent back in a response.

### [Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio)

Razor Pages are similar to the traditional MVC View, but there isn't a 
separate controller. Instead, each Razor page has a page model which 
performs controller and model like functions. The View is written in Razor
markup, similar to the MVC Views, but it's associated page model is more
tightly integrated with the View.

### [Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-8.0)

Unlike both MVC and Razor Pages, Blazor does not generate different HTML
pages depending on the HTTP request. Instead, Blazor apps work by serving
a single HTML page which is dynamically changed based on user 
interactions. For example, instead of getting a new HTML page when a user
clicks a navigation button, the app just rewrites the existing HTML page.
Under the hood, it appears that Blazor uses Javascript and Webassembly
to make this happen.

## Our Application

Right now, we use a combination of traditional MVC Views and Razor Pages.
The traditional MVC views are in a `Views` folder and the associated
controllers are in the `Controllers` folder. 

The Razor pages are kept under the `Areas` folder. The `Areas` folder 
is a tool ASP.NET provides to separate application components related to 
different functional areas. For example, the application code to handle 
logging in and out might live in one Area, while the dashboard might live
in another. Right now the only Area that exists is for `Identity`, logging
users in and out. Under the `Identity` Area are the Razor Pages our 
application uses.