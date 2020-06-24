# Lombiq Training Demo for Orchard Core - Tests



Hello and again, welcome to the Training Demo module! This time we'll take a quick glimpse at unit testing.

We follow up here from `TestedService` in the module project. The basics were discussed there already so here we'll go right into testing.

You may have noticed that the structure of the unit test project is conveniently the same as the module's, e.g. tests for services are in the *Services* folder/namespace.

We use [xUnit framework](https://xunit.net/) for testing, because it's great and Orchard's tests use it too. You could of course use any other unit testing framework if you'd like to but you're on your own :-).

It's worth noting how such tests can be executed: If you're using Visual Studio then the tests will simply show up in the Test Explorer window and you can right click on them and select Run. Otherwise, you'll need to use a test runner; check out the [xUnit docs](https://xunit.net/) for that. Note that for this to work the project should reference the `xunit.runner.visualstudio` and `xunit` packages (but you need that anyway to write xUnit tests :-)).


To start, head over to *Services/TestedServiceTests*!