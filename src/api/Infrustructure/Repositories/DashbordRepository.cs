using Microsoft.EntityFrameworkCore;
using LibraryManagement.Features.Dashboard.DTOs;
using Mapster;
using Data;

namespace LibraryManagement.Features.Dashboard.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly BiblioDbContext _context;

    public DashboardRepository(BiblioDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookLoanCountDto>> GetTopBooksLoansAsync(string biblioId, int count = 10)
    {
        var query = await (from e in _context.Emprunts
                          join i in _context.Inventaires on e.Id_inv equals i.id_inv
                          join l in _context.Livres on i.id_liv equals l.id_livre
                          where e.id_biblio == biblioId
                          group new { l.id_livre, l.titre } by new { l.id_livre, l.titre } into g
                          orderby g.Count() descending
                          select new { BookId = g.Key.id_livre, BookTitle = g.Key.titre, LoanCount = g.Count() })
                          .Take(count)
                          .ToListAsync();

        return query.Adapt<List<BookLoanCountDto>>();
    }
      public async Task<List<BookRotationRateDto>> GetBookRotationRatesAsync(string biblioId)
      {
          var query = await (from i in _context.Inventaires
                            join l in _context.Livres on i.id_liv equals l.id_livre
                            where i.id_biblio == biblioId
                            let totalCopies = _context.Inventaires.Count()
                            let loanCount = (from e in _context.Emprunts
                                            join inv in _context.Inventaires on e.Id_inv equals inv.id_inv
                                            select e).Count()
                            where totalCopies > 0
                            select new
                            {
                                BookTitle = l.titre,
                                RotationRate = totalCopies > 0 ? (decimal)loanCount / totalCopies : 0,
                                Available = totalCopies - loanCount,
                                Loaned = loanCount
                            })
                            .OrderByDescending(x => x.RotationRate)
                            .ToListAsync();

          return query.Adapt<List<BookRotationRateDto>>();
      }

      public async Task<List<UnusedBookDto>> GetUnusedBooksAsync(string biblioId, int monthsBack = 6)
      {
          var cutoffDate = DateTime.Now.AddMonths(-monthsBack);
          var query = await (from inv in _context.Inventaires
                            join livre in _context.Livres on inv.id_liv equals livre.id_livre
                            where inv.id_biblio == biblioId
                            where !(from e in _context.Emprunts
                                 join l in _context.Livres on e.Id_inv equals l.id_livre
                                 where inv.id_liv == l.id_livre && e.date_emp >= cutoffDate
                                 select e).Any()
                            select new
                            {
                                BookTitle = livre.titre,
                                BookId = livre.id_livre,
                                LastLoan = (from e in _context.Emprunts
                                         join invent in _context.Inventaires on e.Id_inv equals invent.id_inv
                                         where invent.id_liv == livre.id_livre
                                         orderby e.date_emp descending
                                         select e.date_emp).FirstOrDefault()
                            })
                            .ToListAsync();

          return query.Adapt<List<UnusedBookDto>>();
      }
    public async Task<(decimal delayRate, List<UserDelayCountDto> topUsers, List<BookDelayCountDto> problematicBooks)> GetDelayDataAsync(string biblioId)
    {
        var totalLoans = await _context.Emprunts.CountAsync(e => e.id_biblio == biblioId);
        var delayedLoans = await _context.Emprunts
            .CountAsync(e => e.id_biblio == biblioId && 
                        e.date_effectif.HasValue && 
                        e.date_effectif > e.date_retour_prevu);

        var delayRate = totalLoans > 0 ? (decimal)delayedLoans / totalLoans * 100 : 0;

        // Top delayed users
        var delayedUsersQuery = await (from e in _context.Emprunts
                                      join m in _context.Membres on e.id_membre equals m.id_membre
                                      where e.id_biblio == biblioId && 
                                            e.date_effectif.HasValue && 
                                            e.date_effectif > e.date_retour_prevu
                                      group new { m.id_membre, m.nom, m.prenom } by new { m.id_membre, m.nom, m.prenom } into g
                                      orderby g.Count() descending
                                      select new
                                      {
                                          UserName = $"{g.Key.nom} {g.Key.prenom}",
                                          DelayCount = g.Count(),
                                          UserId = g.Key.id_membre
                                      })
                                      .Take(5)
                                      .ToListAsync();

        var topUsers = delayedUsersQuery.Adapt<List<UserDelayCountDto>>();

        // Problematic books
        var problematicBooksQuery = await (from e in _context.Emprunts
                                          join i in _context.Inventaires on e.Id_inv equals i.id_inv
                                          join l in _context.Livres on i.id_liv equals l.id_livre
                                          where e.id_biblio == biblioId && 
                                                e.date_effectif.HasValue && 
                                                e.date_effectif > e.date_retour_prevu
                                          group new { l.id_livre, l.titre } by new { l.id_livre, l.titre } into g
                                          orderby g.Count() descending
                                          select new
                                          {
                                              BookTitle = g.Key.titre,
                                              DelayCount = g.Count(),
                                              BookId = g.Key.id_livre
                                          })
                                          .Take(5)
                                          .ToListAsync();

        var problematicBooks = problematicBooksQuery.Adapt<List<BookDelayCountDto>>();

        return (delayRate, topUsers, problematicBooks);
    }

    public async Task<(decimal sanctionRate, List<MonthlyLossDto> monthlyLosses, DelayVsLossDto delayVsLoss, decimal totalLossCost)> GetLossDataAsync(string biblioId)
    {
        var totalLoans = await _context.Emprunts.CountAsync(e => e.id_biblio == biblioId);
        var sanctionCount = await _context.Sanctions.CountAsync(s => s.id_biblio == biblioId);
        var sanctionRate = totalLoans > 0 ? (decimal)sanctionCount / totalLoans * 100 : 0;

        var monthlyLossesQuery = await (from s in _context.Sanctions
                                       where s.id_biblio == biblioId && 
                                             s.date_sanction >= DateTime.Now.AddMonths(-12)
                                       group s by new { s.date_sanction.Month, s.date_sanction.Year } into g
                                       orderby g.Key.Year, g.Key.Month
                                       select new
                                       {
                                           Month = g.Key.Month,
                                           Year = g.Key.Year,
                                           LossCost = g.Where(s => s.raison.Equals("perte")).Sum(s => s.montant),
                                           FineAmount = g.Where(s => s.raison.Equals("retard")).Sum(s => s.montant)
                                       })
                                       .ToListAsync();

        var monthlyLosses = monthlyLossesQuery.Adapt<List<MonthlyLossDto>>();

        var delayCount = await _context.Emprunts
            .CountAsync(e => e.id_biblio == biblioId && 
                        e.date_effectif.HasValue && 
                        e.date_effectif > e.date_retour_prevu);
        
        var lossCount = await _context.Sanctions
            .CountAsync(s => s.id_biblio == biblioId && s.raison.Equals("perte"));

        var totalLossCostNullable = await _context.Sanctions
            .Where(s => s.id_biblio == biblioId)
            .SumAsync(s => (decimal?)s.montant);

        var totalLossCost = totalLossCostNullable ?? 0;

        var delayVsLoss = new DelayVsLossDto { DelayCount = delayCount, LossCount = lossCount };

        return (sanctionRate, monthlyLosses, delayVsLoss, totalLossCost);
    }

    public async Task<(List<MonthlyLoanDto> monthlyLoans, decimal avgDuration)> GetResourceDataAsync(string biblioId)
    {
        var monthlyLoansQuery = await (from e in _context.Emprunts
                                      where e.id_biblio == biblioId && 
                                            e.date_emp >= DateTime.Now.AddMonths(-12)
                                      group e by new { e.date_emp.Month, e.date_emp.Year } into g
                                      orderby g.Key.Year, g.Key.Month
                                      select new
                                      {
                                          Month = g.Key.Month,
                                          Year = g.Key.Year,
                                          LoanCount = g.Count()
                                      })
                                      .ToListAsync();

        var monthlyLoans = monthlyLoansQuery.Adapt<List<MonthlyLoanDto>>();

        var avgDurationNullable = await (from e in _context.Emprunts
                                        where e.id_biblio == biblioId && e.date_effectif.HasValue
                                        select (double?)(e.date_effectif!.Value - e.date_emp).TotalDays)
                                        .AverageAsync();

        var avgDuration = (decimal)(avgDurationNullable ?? 0);

        return (monthlyLoans, avgDuration);
    }

    public async Task<(decimal beforeRate, decimal afterRate, List<MonthlyPolicyComparisonDto> comparison)> GetPolicyDataAsync(string biblioId)
    {
        var policyChangeDate = await _context.Parametres
            .Where(p => p.IdBiblio == biblioId)
            .Select(p => p.date_modification)
            .FirstOrDefaultAsync();

        if (policyChangeDate == default)
            policyChangeDate = DateTime.Now.AddMonths(-6);

        var totalLoansBefore = await _context.Emprunts
            .CountAsync(e => e.id_biblio == biblioId && e.date_emp < policyChangeDate);
        
        var delayedLoansBefore = await _context.Emprunts
            .CountAsync(e => e.id_biblio == biblioId && 
                        e.date_emp < policyChangeDate &&
                        e.date_effectif.HasValue &&
                        e.date_effectif > e.date_retour_prevu);

        var totalLoansAfter = await _context.Emprunts
            .CountAsync(e => e.id_biblio == biblioId && e.date_emp >= policyChangeDate);
        
        var delayedLoansAfter = await _context.Emprunts
            .CountAsync(e => e.id_biblio == biblioId && 
                        e.date_emp >= policyChangeDate &&
                        e.date_effectif.HasValue &&
                        e.date_effectif > e.date_retour_prevu);

        var beforeRate = totalLoansBefore > 0 ? (decimal)delayedLoansBefore / totalLoansBefore * 100 : 0;
        var afterRate = totalLoansAfter > 0 ? (decimal)delayedLoansAfter / totalLoansAfter * 100 : 0;

               var monthlyComparisonQuery = await (from e in _context.Emprunts
                                           where e.id_biblio == biblioId && 
                                                 e.date_emp >= DateTime.Now.AddMonths(-12)
                                           group e by new { e.date_emp.Month, e.date_emp.Year } into g
                                           let totalInMonth = g.Count()
                                           let delayedInMonthBefore = g.Count(x => x.date_emp < policyChangeDate && 
                                                                              x.date_effectif.HasValue && 
                                                                              x.date_effectif > x.date_retour_prevu)
                                           let delayedInMonthAfter = g.Count(x => x.date_emp >= policyChangeDate && 
                                                                             x.date_effectif.HasValue && 
                                                                             x.date_effectif > x.date_retour_prevu)
                                           let totalBeforeInMonth = g.Count(x => x.date_emp < policyChangeDate)
                                           let totalAfterInMonth = g.Count(x => x.date_emp >= policyChangeDate)
                                           orderby g.Key.Year, g.Key.Month
                                           select new
                                           {
                                               Month = g.Key.Month,
                                               Year = g.Key.Year,
                                               BeforeRate = totalBeforeInMonth > 0 ? (decimal)delayedInMonthBefore / totalBeforeInMonth * 100 : 0,
                                               AfterRate = totalAfterInMonth > 0 ? (decimal)delayedInMonthAfter / totalAfterInMonth * 100 : 0
                                           })
                                           .ToListAsync();

        var comparison = monthlyComparisonQuery.Adapt<List<MonthlyPolicyComparisonDto>>();

        return (beforeRate, afterRate, comparison);
    }
}
