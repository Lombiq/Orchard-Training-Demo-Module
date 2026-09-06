# Lombiq Training Demo for Orchard Core - Tests

Hello and again, welcome to the Training Demo module! This time we'll take a quick glimpse at unit testing.

We follow up here from `TestedService` in the module project. The basics were discussed there already so here we'll go right into testing.

You may have noticed that the structure of the unit test project is conveniently the same as the module's, e.g. tests for services are in the _Services_ folder/namespace.

We use the [xUnit framework](https://xunit.net/) for testing, because it's great and Orchard's tests use it too. You could of course use any other unit testing framework if you'd like to but you're on your own :-).

Run these tests from Test Explorer in an IDE with Microsoft Testing Platform support, or use `dotnet test --project Lombiq.TrainingDemo.Tests.csproj` with .NET SDK 10 or later. You can also run the built executable directly. The project references `xunit.v3` and enables the Microsoft Testing Platform runner. The repository's _global.json_ selects this runner for `dotnet test`; see the [xUnit documentation](https://xunit.net/docs/getting-started/v3/microsoft-testing-platform) for details.

To start, head over to _Services/TestedServiceTests_!
