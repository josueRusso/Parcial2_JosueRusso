﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parcial2_JosueRusso.Server.DAL;
using Parcial2_JosueRusso.Shared;

namespace Parcial2_JosueRusso.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradasController : ControllerBase
    {
        private readonly Context _context;

        public EntradasController(Context context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Entradas>>> GetEntradas()
        {
            if (_context.Entradas == null)
            {
                return NotFound();
            }
            return await _context.Entradas.ToListAsync();
        }

        [HttpGet("{ip}")]

        public async Task<ActionResult<Entradas>> GetEntradas(int id)
        {
            if (_context.Entradas == null)
            {
                return NotFound();
            }
            var entrada = await _context.Entradas.Include(e => e.EntradasDetalles).Where( e => e.EntradaId == id).FirstOrDefaultAsync();
            if (entrada == null)
            {
                return NotFound();
            }
            return entrada;
        }

        public bool EntradasExiste (int id)
        {
            return (_context.Entradas?.Any(e => e.EntradaId == id)).GetValueOrDefault();
        }
        
        [HttpPost]

        public async Task<ActionResult<Entradas>> PostEntradas(Entradas entradas)
        {
            if(!EntradasExiste(entradas.EntradaId))
                _context.Entradas.Add(entradas);
            else
                _context.Entradas.Update(entradas);
            await _context.SaveChangesAsync();
            return Ok(entradas);
                    
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntradas(int id)
        {
            if(_context.Entradas == null)
            {
                return NotFound();
            }
            var entrada = await _context.Entradas.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }
            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}