<Query Kind="Statements">
  <Reference Relative="Nito.BrowserBoss\bin\Release\Nito.BrowserBoss.dll">C:\Work\BrowserBoss\Nito.BrowserBoss\Nito.BrowserBoss\bin\Release\Nito.BrowserBoss.dll</Reference>
  <Namespace>Nito.BrowserBoss</Namespace>
</Query>

// This is a sample site designed to be difficult to script.
Boss.Url = "http://newtours.demoaut.com/";

Boss.Click("REGISTER");                   // You can click links by text
Boss.Write("#email", "StephenCleary");          // CSS selectors are commonly used
Boss.Write("//input[@type='password']", "password"); // XPath also works fine
// Note that the last line matched both password inputs, and filled them both in.
Boss.Click("input[name='register']");

Console.WriteLine("Done");
