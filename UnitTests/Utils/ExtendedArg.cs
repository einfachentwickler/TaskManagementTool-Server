using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;

namespace Host.UnitTests.Utils;
public static class ExtendedArg
{
    public static TType Is<TType>(TType otherObj)
    {
        return Arg.Is<TType>(obj => ToPredicate(() => obj.Should().BeEquivalentTo(otherObj, string.Empty)));
    }

    private static bool ToPredicate(Action act)
    {
        using var scope = new AssertionScope();

        act();

        return scope.Discard().Length == 0;
    }
}