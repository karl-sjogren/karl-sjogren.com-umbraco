using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace UmbracoBlog.Umbraco.Mvc;

public class ControllerFeatureConvention : IControllerModelConvention {
    public void Apply(ControllerModel controller) {
        var name = DeriveFeatureFolderName(controller);
        controller.Properties.Add("feature", name);
    }

    private static string DeriveFeatureFolderName(ControllerModel model) {
        var @namespace = model.ControllerType.Namespace;
        if(@namespace == null) {
            return string.Empty;
        }

        var result = @namespace.Split('.')
            .SkipWhile(static s => s != "Features")
            .Aggregate(string.Empty, (current, s) => current + s + "/")
            .TrimEnd('/');

        return result;
    }
}
