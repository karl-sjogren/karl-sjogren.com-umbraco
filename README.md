# Karls Umbraco Blog

This is a try to rebuild my personal blog using Umbraco and my Umbraco
Code First experiment. So this repository might also be considered a demo
of that.

## Sample blog posts

The repository includes 62 fictional programming posts with overlapping topics,
tags and a few code examples. To import them into an installed **local** site,
start Umbraco and create and publish a Blog Index. Copy its GUID from the
backoffice. The importer uses the authenticated Management API, creates and
publishes missing samples, and leaves existing samples untouched. It does not
run automatically and refuses non-local or non-HTTPS URLs. It requires Node.js
18 or later.

Sign in to the local backoffice, open browser developer tools, and inspect an
authenticated request to `/umbraco/management/api/v1/`. Copy the bearer token
(without the `Bearer ` prefix) and the request's `Cookie` header into the
following **temporary environment variables**. Treat both as credentials:
never commit, share or print them. The account needs permission to create and
publish posts under the selected Blog Index.

Node must trust the site's development certificate. If necessary, export the
public certificate to a new local file with `dotnet dev-certs https --export-path
<path-to-cert.pem> --format PEM` and set `NODE_EXTRA_CA_CERTS` to that path.
Then run from the repository root in PowerShell:

```powershell
$env:UMBRACO_SAMPLE_TOKEN = '<bearer-token>'
$env:UMBRACO_SAMPLE_COOKIE = '<request-cookie-header>'
$env:NODE_EXTRA_CA_CERTS = '<path-to-cert.pem>'
node scripts\import-sample-posts.mjs https://localhost:44363 '<blog-index-guid>'
Remove-Item Env:\UMBRACO_SAMPLE_TOKEN, Env:\UMBRACO_SAMPLE_COOKIE
```

The import checks for key collisions and drafts before creating anything.
If a create or publish fails, inspect the reported item in the backoffice before
retrying; publishing can leave a draft. Existing posts are never updated or
deleted. Repeating the command after a successful import skips all samples.

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
