using AshamedApp.Application.DTOs;
using AshamedApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AshamedApp.API.Controllers;

[ApiController]
[Route("api/snapshots")]
public class SnapshotController(ISnapshotManagerService snapshotManagerService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSnapshotAsync(SnapshotDto snapshot)
    {
        try
        {
            var created = await snapshotManagerService.CreateSnapshotAsync(snapshot);
            return CreatedAtAction(nameof(GetSnapshotByIdAsync), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "MqttMessage not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "An unexpected error occurred while creating the snapshot",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
    
    [HttpGet("{id}")]
    [ActionName(nameof(GetSnapshotByIdAsync))]
    public async Task<IActionResult> GetSnapshotByIdAsync(int id)
    {
        try
        {
            var snapshot = await snapshotManagerService.GetSnapshotByIdAsync(id);
            if (snapshot == null) return NotFound();
            return Ok(snapshot);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Snapshot not found",
                Detail = e.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "An error occurred",
                Detail = e.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllSnapshotsAsync()
    {
        var allSnapshots = await snapshotManagerService.GetAllSnapshotsAsync();
        return Ok(allSnapshots);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSnapshot(int id, SnapshotDto snapshotDto)
    {
        if (snapshotDto == null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid input",
                Detail = "Request body cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        try
        {
            var updatedSnapshot = await snapshotManagerService.UpdateSnapshotAsync(id, snapshotDto.Title, snapshotDto.Description);
            return Ok(updatedSnapshot);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Snapshot not found",
                Detail = e.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "An error occurred",
                Detail = e.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSnapshotAsync(int id)
    {
        try
        {
            await snapshotManagerService.DeleteSnapshotAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Snapshot not found",
                Detail = e.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "An error occurred",
                Detail = e.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
}
