using NUnit.Framework;

namespace Namh.Configuration.Template.Test;

[TestFixture]
public class ReloadTokenTests : TestBase
{
    [Test]
    public void GetReloadToken_ReturnsCurrentTokenThatChangesAfterReload()
    {
        var currentToken = _configuration.GetReloadToken();

        _configuration.Reload();

        Assert.That(currentToken.HasChanged, Is.True);
        Assert.That(_configuration.GetReloadToken(), Is.Not.SameAs(currentToken));
    }
}