using LibraryManagement.Features.Dashboard.DTOs;

namespace LibraryManagement.Features.Dashboard.Repositories;

public interface IDashboardRepository
{
    Task<List<BookLoanCountDto>> GetTopBooksLoansAsync(string biblioId, int count = 10);
    Task<List<BookRotationRateDto>> GetBookRotationRatesAsync(string biblioId);
    Task<List<UnusedBookDto>> GetUnusedBooksAsync(string biblioId, int monthsBack = 6);
    Task<(decimal delayRate, List<UserDelayCountDto> topUsers, List<BookDelayCountDto> problematicBooks)> GetDelayDataAsync(string biblioId);
    Task<(decimal sanctionRate, List<MonthlyLossDto> monthlyLosses, DelayVsLossDto delayVsLoss, decimal totalLossCost)> GetLossDataAsync(string biblioId);
    Task<(List<MonthlyLoanDto> monthlyLoans, decimal avgDuration)> GetResourceDataAsync(string biblioId);
    Task<(decimal beforeRate, decimal afterRate, List<MonthlyPolicyComparisonDto> comparison)> GetPolicyDataAsync(string biblioId);
}
