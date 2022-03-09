# VendorsStockQuoteInformationApp

Architecting an ASP.NET Core Angular Application for fetching stockquote information of a given vendor

Implementation details:
- All vendors (Netflix and Apple) are fetched from VendorRepository and displayed in Angular MatTable
- There are 2 filters on the screen, one for filtering by vendor name and the other one is filtering by vendor symbol
- Each line contains an extra column to fetch the stock quote information by symbol. When clicked, Angular dialog box is opened and stockquote information is fetched.
- Depending on the file extension the import strategy is decided. Thus stock quote information by vendor symbol is either fetched from a csv file or json file

The project structure is described below:
AspNetAngularApp
  - AspNetAngularApp.Api (Contains controllers, mappers, ViewModels, StartUp)
  - AspNetAngularApp.Core (Contains Models and Interfaces for Strategy,factory and reposity patterns)
  - AspNetAngularApp.Data (Contains repository and strategy pattern implementations, csv and json files)
  - AspNetAngularApp.Extensions (Contains import strategy picker implementation and vendor factory to fetch stock quote of vendor)
  - AspNetAngularApp.UI (Contains Angular files. ClientApp > src > app contains required code to implement Material design)
AspNetAngularApp.Tests
  - ControllerTests (XUnit tests with FluentAssertions and Moq library)
  - IntegrationTests (Xunit tests with TestServer)
  
  Technology stack:
  - .netcore 3.1
  - Angular Material Design (Angular version 13.1.4)
  - Angular Mattable
  - Xunit with FluentAssertions and Moq for testing
  - TestServer for integration testing
  - Strategy design pattern for deciding whether import from a csv or json
  - Repository pattern to create and fetch vendor information
  - factory pattern to get the right stock quote service by vendor
  - SwaggerUI to display and test endpoints 
  - Automapper to map from model to viewmodel
