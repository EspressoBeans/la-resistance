# la-resistance
ASP.NET C# MVC5 Application

What you've happened upon is a C# ASP.NET application developed as a code challenge.  The goal of the challenge was to provide a .NET full stack solution to calculating resistor Ohm value based on the color coded bands found on common resistor electrical components.

This Visual Studio 2017 solution has 3 projects: an <b>API</b>, a <b>UI</b>, and a Unit/Integrations <b>Test</b> project.

- The Web API 2 project is a data access interface for the UI and is coded to access data with a simple repository pattern.  It also contains an "engine" for calculating the resistance values based on colored band string values passed into it.  The API project also contains the data used for the project which requires (LocalDb)\MSSQLLocalDB.

- The MVC5 project is a standard baseline MVC application with custom graphical representation of a resistor image (which I think looks like a corndog), an output table for displaying calculated values, and an intuitive band color selection area using dropdowns.

- The Unit/Integrations test project uses MSTest V2 framework.  The Unit test uses standard coding practices (testing only logic).  However, as this is a demo project, the Integration tests coded are not recommended nor best practices since they require Entity Framework to run tests and validate hard-coded test values against data pulled from database.

Email me with any questions vmguadalupe@yahoo.com.

Below are some screenshots:

![alt text](https://raw.githubusercontent.com/EspressoBeans/la-resistance/master/git_resources/screenshot_main.png)

![alt text](https://raw.githubusercontent.com/EspressoBeans/la-resistance/master/git_resources/screenshot_main_dd_example.png)
