using FluentAssertions;
using NSubstitute;

namespace WebApi.UnitTests.Utils;

public static class ExtendedArg
{
    public static TType Is<TType>(TType otherObj)
    {
        return Arg.Is<TType>(obj => ToPredicate(() => obj.Should().BeEquivalentTo(otherObj, string.Empty)));
    }

    private static bool ToPredicate(Action act)
    {
        try
        {
            act();
            return true;
        }
        catch
        {
            return false;
        }
    }
}