using Microsoft.AspNetCore.SignalR;
using LibraryManagement.Features.Dashboard.DTOs;
using LibraryManagement.Features.Dashboard.Repositories;
using Mapster;

namespace LibraryManagement.Features.Dashboard.Services;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardDataAsync(string biblioId);
    Task BroadcastDashboardUpdateAsync(string biblioId);
    Task NotifyDataChangeAsync(string biblioId, string changeType);
}

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;
    private readonly IHubContext<DashboardHub> _hubContext;

    public DashboardService(IDashboardRepository repository, IHubContext<DashboardHub> hubContext)
    {
        _repository = repository;
        _hubContext = hubContext;
    }

    public async Task<DashboardResponse> GetDashboardDataAsync(string biblioId)
    {
        var tasks = new Task[]
        {
            _repository.GetTopBooksLoansAsync(biblioId),
            _repository.GetBookRotationRatesAsync(biblioId),
            _repository.GetUnusedBooksAsync(biblioId),
            _repository.GetDelayDataAsync(biblioId),
            _repository.GetLossDataAsync(biblioId),
            _repository.GetResourceDataAsync(biblioId),
            _repository.GetPolicyDataAsync(biblioId)
        };

        await Task.WhenAll(tasks);

        // Get all data
        var topBooks = await _repository.GetTopBooksLoansAsync(biblioId);
        var rotationRates = await _repository.GetBookRotationRatesAsync(biblioId);
        var unusedBooks = await _repository.GetUnusedBooksAsync(biblioId);
        var delayData = await _repository.GetDelayDataAsync(biblioId);
        var lossData = await _repository.GetLossDataAsync(biblioId);
        var resourceData = await _repository.GetResourceDataAsync(biblioId);
        var policyData = await _repository.GetPolicyDataAsync(biblioId);

        return new DashboardResponse
        {
            CatalogueOptimization = new CatalogueOptimizationDto
            {
                TopBooksLoans = topBooks,
                BookRotationRates = rotationRates,
                UnusedBooks = unusedBooks
            },
            DelayReduction = new DelayReductionDto
            {
                DelayRate = delayData.delayRate,
                TopDelayedUsers = delayData.topUsers,
                ProblematicBooks = delayData.problematicBooks
            },
            LossAnalysis = new LossAnalysisDto
            {
                SanctionRate = lossData.sanctionRate,
                MonthlyLosses = lossData.monthlyLosses,
                DelayVsLoss = lossData.delayVsLoss,
                TotalLossCost = lossData.totalLossCost
            },
            ResourcePlanning = new ResourcePlanningDto
            {
                MonthlyLoans = resourceData.monthlyLoans,
                AverageLoanDuration = resourceData.avgDuration
            },
            PolicyEvaluation = new PolicyEvaluationDto
            {
                DelayRateBeforePolicy = policyData.beforeRate,
                DelayRateAfterPolicy = policyData.afterRate,
                MonthlyComparison = policyData.comparison
            }
        };
    }

    public async Task BroadcastDashboardUpdateAsync(string biblioId)
    {
        var data = await GetDashboardDataAsync(biblioId);
        await _hubContext.Clients.Group($"biblio_{biblioId}")
            .SendAsync("DashboardUpdated", data);
    }

    public async Task NotifyDataChangeAsync(string biblioId, string changeType)
    {
        // Send specific change notifications
        await _hubContext.Clients.Group($"biblio_{biblioId}")
            .SendAsync("DataChanged", new { changeType, timestamp = DateTime.UtcNow });

        // Optionally send updated data immediately
        await BroadcastDashboardUpdateAsync(biblioId);
    }
}

// SignalR Hub in the same file
public class DashboardHub : Hub
{
    public async Task JoinBiblioGroup(string biblioId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"biblio_{biblioId}");
        await Clients.Caller.SendAsync("JoinedGroup", $"biblio_{biblioId}");
    }

    public async Task LeaveBiblioGroup(string biblioId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"biblio_{biblioId}");
        await Clients.Caller.SendAsync("LeftGroup", $"biblio_{biblioId}");
    }

    public async Task RequestDashboardUpdate(string biblioId)
    {
        // Client can request immediate update
        await Clients.Group($"biblio_{biblioId}")
            .SendAsync("UpdateRequested", new { biblioId, requestedBy = Context.ConnectionId });
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
