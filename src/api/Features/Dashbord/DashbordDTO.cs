namespace LibraryManagement.Features.Dashboard.DTOs;

public class DashboardResponse
{
    public CatalogueOptimizationDto CatalogueOptimization { get; set; } = new();
    public DelayReductionDto DelayReduction { get; set; } = new();
    public LossAnalysisDto LossAnalysis { get; set; } = new();
    public ResourcePlanningDto ResourcePlanning { get; set; } = new();
    public PolicyEvaluationDto PolicyEvaluation { get; set; } = new();
}

public class CatalogueOptimizationDto
{
    public List<BookLoanCountDto> TopBooksLoans { get; set; } = new();
    public List<BookRotationRateDto> BookRotationRates { get; set; } = new();
    public List<UnusedBookDto> UnusedBooks { get; set; } = new();
}

public class DelayReductionDto
{
    public decimal DelayRate { get; set; }
    public List<UserDelayCountDto> TopDelayedUsers { get; set; } = new();
    public List<BookDelayCountDto> ProblematicBooks { get; set; } = new();
}

public class LossAnalysisDto
{
    public decimal SanctionRate { get; set; }
    public List<MonthlyLossDto> MonthlyLosses { get; set; } = new();
    public DelayVsLossDto DelayVsLoss { get; set; } = new();
    public decimal TotalLossCost { get; set; }
}

public class ResourcePlanningDto
{
    public List<MonthlyLoanDto> MonthlyLoans { get; set; } = new();
    public decimal AverageLoanDuration { get; set; }
}

public class PolicyEvaluationDto
{
    public decimal DelayRateBeforePolicy { get; set; }
    public decimal DelayRateAfterPolicy { get; set; }
    public List<MonthlyPolicyComparisonDto> MonthlyComparison { get; set; } = new();
}

// Supporting DTOs
public class BookLoanCountDto
{
    public string BookTitle { get; set; } = string.Empty;
    public int LoanCount { get; set; }
    public string BookId { get; set; } = string.Empty;
}

public class BookRotationRateDto
{
    public string BookTitle { get; set; } = string.Empty;
    public decimal RotationRate { get; set; }
    public int Available { get; set; }
    public int Loaned { get; set; }
}

public class UnusedBookDto
{
    public string BookTitle { get; set; } = string.Empty;
    public DateTime LastLoan { get; set; }
    public string BookId { get; set; } = string.Empty;
}

public class UserDelayCountDto
{
    public string UserName { get; set; } = string.Empty;
    public int DelayCount { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class BookDelayCountDto
{
    public string BookTitle { get; set; } = string.Empty;
    public int DelayCount { get; set; }
    public string BookId { get; set; } = string.Empty;
}

public class MonthlyLossDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal LossCost { get; set; }
    public decimal FineAmount { get; set; }
}

public class DelayVsLossDto
{
    public int DelayCount { get; set; }
    public int LossCount { get; set; }
}

public class MonthlyLoanDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int LoanCount { get; set; }
}

public class MonthlyPolicyComparisonDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BeforeRate { get; set; }
    public decimal AfterRate { get; set; }
}
