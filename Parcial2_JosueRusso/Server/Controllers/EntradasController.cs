using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parcial2_JosueRusso.Server.DAL;
using Parcial2_JosueRusso.Shared;
using System.Text.RegularExpressions;

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

        [HttpGet("{id}")]

        public async Task<ActionResult<Entradas>> GetEntradas(int id)
        {
            if (_context.Entradas == null)
            {
                return NotFound();
            }
            var entrada = await _context.Entradas.Include(e => e.EntradasDetalles).Where(e => e.EntradaId == id).FirstOrDefaultAsync();
            if (entrada == null)
            {
                return NotFound();
            }
            return entrada;
        }

        public bool EntradasExiste(int id)
        {
            return (_context.Entradas?.Any(e => e.EntradaId == id)).GetValueOrDefault();
        }

        [HttpPost]

        public async Task<ActionResult<Entradas>> PostEntradas(Entradas entradas)
        {
                //Recorre cada detalle de la entrada(entradas.EntradasDetalles) 
                //para actualizar la cantidad de existencias de los productos relacionados
                //(_context.Productos). Resta la cantidad utilizada en el detalle de la entrada de la cantidad de existencias del producto.
                //Luego, marca las entidades modificadas para su posterior persistencia en la base de datos.
            if (!EntradasExiste(entradas.EntradaId))
            {
                Productos? producto;
                foreach (var consumido in entradas.EntradasDetalles)
                {
                    producto = _context.Productos.Find(consumido.ProductoId);
                    producto.Existencia -= (int)consumido.CantidadUtilizada;
                    _context.Entry(producto).State = EntityState.Modified;
                    _context.Entry(consumido).State = EntityState.Added;

                }
                _context.Entradas.Add(entradas);
            }
            else

                //Para cada detalle de la entrada anterior, 
                //actualiza la cantidad de existencias del producto.
                //Suma la cantidad utilizada en el detalle de la entrada anterior a la cantidad de existencias del producto. 
                //Luego, marca las entidades modificadas para  la base de datos.
                //Actualiza la cantidad de existencias del producto con la entrada anterior. 
                //Resta la cantidad producida en la entrada anterior de la cantidad de existencias del producto y marca la entidad como modificada.
                //Elimina los detalles de la entrada anterior(EntradasDetalles) de la base de datos.
                //Para cada detalle de la nueva entrada(entradas.EntradasDetalles), 
                //actualiza la cantidad de existencias del producto relacionado. 
                //Resta la cantidad utilizada en el detalle de la nueva entrada de la cantidad de existencias del producto. 
                //Luego, marca las entidades modificadas para su posterior persistencia en la base de datos.
                //Actualiza la cantidad de existencias del producto relacionado con la nueva entrada. 
                //Suma la cantidad producida en la nueva entrada a la cantidad de existencias del producto y marca la entidad como modificada.
                //Actualiza la entrada en la base de datos con los cambios realizados en la nueva entrada(entradas).

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

        [HttpDelete("{EntradaId}")]
        public async Task<IActionResult> EliminarEntrada(int EntradaId)
        {
            var entrada = await _context.Entradas.Include(e => e.EntradasDetalles).FirstOrDefaultAsync(e => e.EntradaId == EntradaId);

            if (entrada == null)
            {
                return NotFound();
            }

            foreach (var detalle in entrada.EntradasDetalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);

                if (producto != null)
                {
                    producto.Existencia += (int)detalle.CantidadUtilizada;
                    _context.Productos.Update(producto);
                }
            }

            var productoPrincipal = await _context.Productos.FindAsync(entrada.ProductoId);

            if (productoPrincipal != null)
            {
                productoPrincipal.Existencia += entrada.CantidadProducida;
                _context.Productos.Update(productoPrincipal);
            }

            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
