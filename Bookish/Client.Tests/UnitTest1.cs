using Bunit;
using NUnit.Framework;

namespace Client.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ExampleTest()
        {
            using var ctx = new Bunit.TestContext();
            var cut = ctx.RenderComponent<Bookish.Client.Pages.Counter>();
            cut.MarkupMatches("<h1>Counter</h1><p>Current count: 0</p><button class=\"btn btn-primary\" blazor:onclick=\"1\">Click me</button>");
        }
    }
}