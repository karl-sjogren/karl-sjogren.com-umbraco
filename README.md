# Karls Umbraco Blog

This is a try to rebuild my personal blog using Umbraco and my Umbraco
Code First experiment. So this repository might also be considered a demo
of that.

The reusable `Shorthand.UmbracoHelpers` project provides injectable friendly
helper interfaces in the `Shorthand.UmbracoHelpers` namespace. They are registered
by `FriendlyHelpersComposer` (or manually with `services.AddFriendlyHelpers()`).
Inject the relevant interface, and pass the content, media, or URL helper as the
first argument instead of calling a friendly extension method. The helpers use the
current request context and do not resolve dependencies through `StaticServiceProvider`.

Projects referencing the helper package receive a banned-symbols file for the
four friendly extension classes. Project-reference consumers must explicitly
import the helper project's `buildTransitive/Shorthand.UmbracoHelpers.targets`,
as shown in the Umbraco project. Set `DisableShorthandUmbracoHelpersBannedSymbols`
to `true` in the consuming project to opt out. The consumer must reference
`Microsoft.CodeAnalysis.BannedApiAnalyzers` for these rules to be enforced.

Uses Font Awesome for icons, see [Font Awesome](https://fontawesome.com).

The design borrows heavily from the
[Chirpy jekyll theme](https://github.com/cotes2020/jekyll-theme-chirpy) that I
used for my previous blog.

## Thats a cool Icon Picker btw

Sure is, it allows you to pick Font Awesome icons easily in the backoffice.
You want to use it? Just copy the backoffice extension from this project to
your own Umbraco project.
