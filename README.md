# Karls Umbraco Blog

This is a try to rebuild my personal blog using Umbraco and my Umbraco
Code First experiment. So this repository might also be considered a demo
of that.

The reusable `Shorthand.UmbracoHelpers` project provides injectable friendly
helper interfaces in the `Shorthand.UmbracoHelpers` namespace. They are registered
by `FriendlyHelpersComposer` (or manually with `services.AddFriendlyHelpers()`). Inject the relevant interface, and
pass the content, media, or URL helper as the first argument instead of calling
a friendly extension method. The helpers use the current request context and do
not resolve dependencies through `StaticServiceProvider`.
