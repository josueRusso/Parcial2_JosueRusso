using Microsoft.AspNetCore.Http;
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
            if (!EntradasExiste(entradas.EntradaId))
            {
                Productos? producto; 
                foreach(var consumido in entradas.EntradasDetalles)
                {
                    producto = _context.Productos.Find(consumido.ProductoId);
                    producto.Existencia -= (int)consumido.CantidadUtilizada;
                    _context.Entry(producto).State = EntityState.Modified;
                    _context.Entry(consumido).State = EntityState.Added;

                }
                _context.Entradas.Add(entradas);
            }
            else
            {
                var entradaAnterior = _context.Entradas
                    .Include(e => e.EntradasDetalles)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.EntradaId == entradas.EntradaId);
                Productos? producto;
                foreach (var consumido in entradaAnterior.EntradasDetalles)
                {
                    producto = _context.Productos.Find(consumido.ProductoId);
                    producto.Existencia += (int)consumido.CantidadUtilizada;
                    _context.Entry(producto).State = EntityState.Modified;
                }
                producto = _context.Productos.Find(entradaAnterior.ProductoId);
                producto.Existencia -= entradaAnterior.CantidadProducida;
                _context.Entry(producto).State = EntityState.Modified;
                _context.Database.ExecuteSqlRaw($"Delete from EntradasDetalles where EntradaId = {entradas.EntradaId}");
                foreach (var consumido in entradas.EntradasDetalles)
                {
                    producto = _context.Productos.Find(consumido.ProductoId);
                    producto.Existencia -= (int)consumido.CantidadUtilizada;
                    _context.Entry(producto).State = EntityState.Modified;
                    _context.Entry(consumido).State = EntityState.Added;
                }
                producto = _context.Productos.Find(entradas.ProductoId);
                producto.Existencia += entradas.CantidadProducida;
                _context.Entry(producto).State = EntityState.Modified;
                _context.Entradas.Update(entradas);
            }
            await _context.SaveChangesAsync();
            _context.Entry(entradas).State = EntityState.Detached;
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
            Productos? producto;
            foreach (var consumido in entrada.EntradasDetalles)
            {
                producto = _context.Productos.Find(consumido.ProductoId);
                producto.Existencia += (int)consumido.CantidadUtilizada;
                _context.Entry(producto).State = EntityState.Modified;
            }
            producto = _context.Productos.Find(entrada.ProductoId);
            producto.Existencia -= entrada.CantidadProducida;
            _context.Entry(producto).State = EntityState.Modified;
            _context.Database.ExecuteSqlRaw($"Delete from EntradasDetalles where EntradaId = {entrada.EntradaId}");
            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();
            _context.Entry(entrada).State = EntityState.Detached;

            return NoContent();
        }
    }
}
