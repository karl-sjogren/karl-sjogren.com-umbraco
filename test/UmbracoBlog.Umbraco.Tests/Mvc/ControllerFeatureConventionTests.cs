using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using UmbracoBlog.Umbraco.Mvc;

namespace UmbracoBlog.Umbraco.Tests.Mvc;

public class ControllerFeatureConventionTests {
    [Fact]
    public void Apply_WhenCalledForNonFeature_ShouldNotSetFeatureName() {
        var builder = GetTypeBuilder("UmbracoBlog.Core.Controllers");
        var model = new ControllerModel(builder.CreateTypeInfo(), new List<object>());

        var convention = new ControllerFeatureConvention();
        convention.Apply(model);

        model.Properties.Count.ShouldBe(1);
        model.Properties.Keys.ShouldContain("feature");

        var featureName = model.Properties["feature"] as string;

        featureName.ShouldBeEmpty();
    }

    [Fact]
    public void Apply_WhenCalledForFeature_ShouldSetCorrectFeatureName() {
        var builder = GetTypeBuilder("UmbracoBlog.Umbraco.Features.Header");
        var model = new ControllerModel(builder.CreateTypeInfo(), new List<object>());

        var convention = new ControllerFeatureConvention();
        convention.Apply(model);

        model.Properties.Count.ShouldBe(1);
        model.Properties.Keys.ShouldContain("feature");

        var featureName = model.Properties["feature"] as string;

        featureName.ShouldBe("Features/Header");
    }

    private static TypeBuilder GetTypeBuilder(string @namespace) {
        var assemblyName = new AssemblyName("DynamicAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicAssembly");
        var typeBuilder = moduleBuilder.DefineType(@namespace + ".DynamicType", TypeAttributes.Public);
        return typeBuilder;
    }
}
