namespace UmbracoBlog.Umbraco.Features.Pagination;

public static class PaginationHttpContextExtensions {
    private const string _paginationRouteKey = "UmbracoBlog.Pagination.Page";

    extension(HttpContext context) {
        public int GetCurrentPage() =>
            context.Items.TryGetValue(_paginationRouteKey, out var page) && page is int currentPage ? currentPage : 1;

        public bool IsPaginationRequest() =>
            context.Items.ContainsKey(_paginationRouteKey);

        public void SetCurrentPage(int currentPage) =>
            context.Items[_paginationRouteKey] = currentPage;
    }
}
