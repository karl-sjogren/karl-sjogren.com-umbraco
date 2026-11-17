using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Shorthand.UmbracoHelpers;

public class FriendlyHelpersComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddFriendlyHelpers();
    }
}
