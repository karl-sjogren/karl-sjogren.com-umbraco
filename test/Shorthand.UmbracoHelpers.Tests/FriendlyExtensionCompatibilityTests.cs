using System.Globalization;
using System.Reflection;
using Umbraco.Extensions;

namespace Shorthand.UmbracoHelpers.Tests;

public class FriendlyExtensionCompatibilityTests {
#pragma warning disable RS0030 // These tests must inspect the APIs that the helpers replace.
    [Theory]
    [InlineData(typeof(IFriendlyImageCropperTemplateHelper), typeof(FriendlyImageCropperTemplateExtensions))]
    [InlineData(typeof(IFriendlyPublishedContentHelper), typeof(FriendlyPublishedContentExtensions))]
    [InlineData(typeof(IFriendlyPublishedElementHelper), typeof(FriendlyPublishedElementExtensions))]
    [InlineData(typeof(IFriendlyUrlHelper), typeof(FriendlyUrlHelperExtensions))]
#pragma warning restore RS0030
    public void InterfaceMatchesExtensionMethods(Type helperType, Type extensionType) {
        var helperMethods = helperType.GetMethods().Select(DescribeMethod).ToHashSet(StringComparer.Ordinal);
        var extensionMethods = extensionType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(DescribeMethod).ToHashSet(StringComparer.Ordinal);

        var missing = extensionMethods.Except(helperMethods).Order(StringComparer.Ordinal);
        var extra = helperMethods.Except(extensionMethods).Order(StringComparer.Ordinal);
        var differences = missing.Select(signature => $"Missing from {helperType.Name}: {signature}")
            .Concat(extra.Select(signature => $"Only in {helperType.Name}: {signature}"));

        string.Join(Environment.NewLine, differences).ShouldBeEmpty();
    }

    private static string DescribeMethod(MethodInfo method) {
        var nullability = new NullabilityInfoContext();
        var genericArguments = method.GetGenericArguments();
        var constraints = genericArguments.Select(argument =>
            $"{DescribeType(argument)}:{argument.GenericParameterAttributes}:" +
            string.Join(",", argument.GetGenericParameterConstraints().Select(DescribeType).Order(StringComparer.Ordinal)));
        var parameters = method.GetParameters().Select(parameter =>
            (parameter.IsIn ? "in " : parameter.IsOut ? "out " : "") +
            (parameter.IsDefined(typeof(ParamArrayAttribute), false) ? "params " : "") +
            $"{DescribeType(parameter.ParameterType)}{DescribeNullability(nullability.Create(parameter))} {parameter.Name}" +
            (parameter.HasDefaultValue ? $" = {FormatDefault(parameter.DefaultValue)}" : parameter.IsOptional ? " optional" : ""));

        return $"{DescribeType(method.ReturnType)}{DescribeNullability(nullability.Create(method.ReturnParameter))} {method.Name}`{genericArguments.Length}" +
            $"({string.Join(", ", parameters)}) [{string.Join("; ", constraints)}]";
    }

    private static string DescribeNullability(NullabilityInfo info)
        => $"?{info.ReadState}<{string.Join(",", info.GenericTypeArguments.Select(DescribeNullability))}>" +
            (info.ElementType is null ? "" : $"[{DescribeNullability(info.ElementType)}]");

    private static string FormatDefault(object? value)
        => value is null ? "null" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? "null";

    private static string DescribeType(Type type) {
        if(type.IsGenericParameter) {
            return $"!!{type.GenericParameterPosition}";
        }

        if(type.HasElementType) {
            return $"{DescribeType(type.GetElementType()!)}{(type.IsArray ? $"[{new string(',', type.GetArrayRank() - 1)}]" : type.IsByRef ? "&" : "*")}";
        }

        if(type.IsGenericType) {
            return $"{type.GetGenericTypeDefinition().FullName}<{string.Join(",", type.GetGenericArguments().Select(DescribeType))}>";
        }

        return type.FullName ?? type.Name;
    }
}
