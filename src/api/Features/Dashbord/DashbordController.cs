using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.Features.Dashboard.Services;
using LibraryManagement.Features.Dashboard.DTOs;

namespace LibraryManagement.Features.Dashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("{biblioId}")]
    public async Task<ActionResult<DashboardResponse>> GetDashboardData(string biblioId)
    {
        try
        {
            var data = await _dashboardService.GetDashboardDataAsync(biblioId);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving dashboard data", error = ex.Message });
        }
    }

    [HttpPost("{biblioId}/refresh")]
    public async Task<IActionResult> RefreshDashboard(string biblioId)
    {
        try
        {
            await _dashboardService.BroadcastDashboardUpdateAsync(biblioId);
            return Ok(new { message = "Dashboard refreshed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error refreshing dashboard", error = ex.Message });
        }
    }

    [HttpPost("{biblioId}/notify/{changeType}")]
    public async Task<IActionResult> NotifyDataChange(string biblioId, string changeType)
    {
        try
        {
            await _dashboardService.NotifyDataChangeAsync(biblioId, changeType);
            return Ok(new { message = $"Notification sent for {changeType}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error sending notification", error = ex.Message });
        }
    }
}
