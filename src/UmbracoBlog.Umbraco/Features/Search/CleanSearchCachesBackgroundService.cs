using Umbraco.Cms.Search.Core.Services.ContentIndexing;

namespace UmbracoBlog.Umbraco.Features.Search;

public class CleanSearchCachesBackgroundService : BackgroundService {
    private readonly IIndexDocumentService _indexDocumentService;
    private readonly ILogger<CleanSearchCachesBackgroundService> _logger;

    public CleanSearchCachesBackgroundService(IIndexDocumentService indexDocumentService, ILogger<CleanSearchCachesBackgroundService> logger) {
        _indexDocumentService = indexDocumentService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("Starting to clean search caches...");
        //await _indexDocumentService.DeleteAllAsync();
        _logger.LogInformation("Finished cleaning search caches.");
    }
}
