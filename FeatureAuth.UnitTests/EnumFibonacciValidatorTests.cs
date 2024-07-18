using FluentAssertions;

namespace FeatureAuth.UnitTests;

public class EnumFibonacciValidatorTests
{
    [Fact]
    public void IsValid_OneValueEnum_IsValid()
    {
        var result = EnumFibonacciValidator.IsValid<ValidOneValueEnum>();

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_ManyValueEnum_IsValid()
    {
        var result = EnumFibonacciValidator.IsValid<ValidMultiValueEnum>();

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_ManyUnorderedEnum_IsValid()
    {
        var result = EnumFibonacciValidator.IsValid<ValidUnorderedMultiValueEnum>();

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_BasicEnum_IsInvalid()
    {
        var result = EnumFibonacciValidator.IsValid<InvalidBasicEnum>();

        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_DuplicateEnumValues_IsInvalid()
    {
        var result = EnumFibonacciValidator.IsValid<InvalidDuplicateValueEnum>();

        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_LessThanZeroValues_IsInvalid()
    {
        var result = EnumFibonacciValidator.IsValid<InvalidLessThanZeroEnum>();

        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_FirstValueIsTwo_IsInvalid()
    {
        var result = EnumFibonacciValidator.IsValid<InvalidFirstValueNotOne>();

        result.Should().BeFalse();
    }

    private enum InvalidBasicEnum { ThisIsInvalid };
#pragma warning disable CA1069 // Enums values should not be duplicated
    private enum InvalidDuplicateValueEnum { One = 1, Two = 1, };
#pragma warning restore CA1069 // Enums values should not be duplicated

    private enum InvalidLessThanZeroEnum { One = -1, Two = 2 };

    private enum InvalidFirstValueNotOne { One = 2 };

    private enum ValidOneValueEnum { One = 1 };

    private enum ValidMultiValueEnum { Value1 = 1, Value2 = 2, Value3 = 3, Value5 = 5, Value8 = 8, Value13 = 13 };

    private enum ValidUnorderedMultiValueEnum { Value5 = 5, Value2 = 2, Value1 = 1, Value8 = 8, Value13 = 13, Value3 = 3, };
}