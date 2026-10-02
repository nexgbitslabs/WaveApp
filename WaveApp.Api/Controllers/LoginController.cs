using Microsoft.AspNetCore.Mvc;
using WaveApp.Application.Interfaces;
using WaveApp.Core.DTOs;

namespace WaveApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }


    // =========================================================
    // GET: api/login
    // =========================================================

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LoginReadDto>>> GetAll()
    {
        var logins = await _loginService.GetAllAsync();

        return Ok(logins);
    }


    // =========================================================
    // GET: api/login/1
    // =========================================================

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoginReadDto>> GetById(int id)
    {
        var login = await _loginService.GetByIdAsync(id);

        if (login == null)
            return NotFound();

        return Ok(login);
    }


    // =========================================================
    // POST: api/login
    // =========================================================

    [HttpPost]
    public async Task<ActionResult<LoginReadDto>> Create(
        [FromBody] LoginCreateDto dto)
    {
        try
        {
            var login = await _loginService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = login.Id },
                login);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // =========================================================
    // PUT: api/login/1
    // =========================================================

    [HttpPut("{id:int}")]
    public async Task<ActionResult<LoginReadDto>> Update(
        int id,
        [FromBody] LoginUpdateDto dto)
    {
        try
        {
            var login = await _loginService.UpdateAsync(id, dto);

            if (login == null)
                return NotFound();

            return Ok(login);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // =========================================================
    // DELETE: api/login/1
    // =========================================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _loginService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}