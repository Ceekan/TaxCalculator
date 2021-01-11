# Tax Calculator

The application consists of five main components:

- TaxCalculator.API - The RESTful API backend using AspNetCore
- TaxCalculator.UI - The MVC Razor frontend
- TaxCalculator.Tests - The Nunit tests
- TaxCalculator.xUnit.Tests - The xUnit tests
- TaxCalculatorDB - The SQL Server database

The application uses Visual Studio 2019 and .NET Core 5.0

## Initial Setup

Step 1
- Create the TaxCalculatorDB database in SQL Server 2016 and above.
  - Server name is (localdb)\mssqllocaldb
  - For convenience the TaxCalculatorDB Database Create Script is located in the MSSQL\Scripts folder.
  - This also includes the TaxCalculatorDB Table Scripts.
  - Path to the database .mdf is C:\MSSQL\TaxCalculatorDB.mdf
  - Path to the database .ldf is C:\MSSQL\TaxCalculatorDB_log.ldf

- Restore the database. The TaxCalculatorDB.bak is located in the MSSQL folder.
  - Path to the database .mdf and .ldf is C:\MSSQL\
  - The path can be changed within the TaxCalculatorDB Database Create Script which is located in the MSSQL\Scripts folder.

Step 2
- Open the TaxCalculator.sln within Visual Studio 2019
- Build the TaxCalculator solutin once the NuGet Packages are restored.
  - This will automatically begin processing bundleconfig.json file.

## Project Execution

Step 1
- Start the TaxCalculator.API by either running Ctrl+F5 (Start Without Debugging) or via the Command Prompt within the TaxCalculator solution folder.
  - The API will now be listening on: https://localhost:5001

```bash
dotnet run -p ./TaxCalculator.API
```
Step 2
- Start the TaxCalculator.UI by either running Ctrl+F5 (Start Without Debugging) or via the Command Prompt within the TaxCalculator solution folder.
  - The UI will now be listening on: https://localhost:5002
  
```bash
dotnet run -p ./TaxCalculator.UI
```

- TaxCalculator.Tests and TaxCalculator.xUnit.Tests can be executed via the Test Explorer within Visual Studio 2019 (Test -> Test Explorer or Ctrl+E, T)
