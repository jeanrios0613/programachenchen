using elchenchenvuelvecy.Controllers;
using FormChenchen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UbicacionesController : ControllerBase
{
    private readonly ToyoNoToyContext _context;
    private readonly ILogger<ToyoNoToyController> _logger;

    public UbicacionesController(ToyoNoToyContext context, ILogger<ToyoNoToyController> logger)
    {
        _context = context;
        _logger = logger;
    }
    [HttpGet("provincias")]
    public IActionResult GetProvincias()
    {
        var provincias = _context.Provincias
                                 .Select(p => new { p.Id, Name = p.Name.ToUpper() })
                                 .ToList();
        return Ok(provincias);
    }

    [HttpGet("distritos/{provinciaId}")]
    public IActionResult GetDistritos(int provinciaId)
    {
        var distritos = _context.Distritos
                                .Where(d => d.province_id == provinciaId)
                                .Select(d => new { d.Id, Name = d.Name.ToUpper() })
                                .ToList();
        return Ok(distritos);
    }

    [HttpGet("corregimientos/{distritoId}")]
    public IActionResult GetCorregimientos(int distritoId)
    {
        var corregimientos = _context.Corregimientos
                                     .Where(c => c.district_id == distritoId)
                                     .Select(c => new { c.Id, Name = c.Name.ToUpper() })
                                     .ToList();
        return Ok(corregimientos);
    }


     [HttpGet("EconomicActivities")]
     public async Task<IActionResult> GetAll()
      {
            var activities = await _context.EconomicActivities
                .OrderBy(e => e.Name)
                .ToListAsync();

            return Ok(activities);
      } 

}
