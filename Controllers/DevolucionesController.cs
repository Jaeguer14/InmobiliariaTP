using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class DevolucionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DevolucionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Devoluciones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Devolucion.Include(d => d.Casa).Include(d => d.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Devoluciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Devolucion == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion
                .Include(d => d.Casa)
                .Include(d => d.Cliente)
                .FirstOrDefaultAsync(m => m.DevolucionId == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // GET: Devoluciones/Create
        public IActionResult Create()
        {
          ViewData["CasaID"] = new SelectList(_context.Casas.Where(x => x.EstaAlquilada == true && x.IsDeleted == false), "CasaID", "CasaNombre");
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "Nombre");
            return View();
        }

        // POST: Devoluciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("DevolucionID,DevolucionDate,ClienteID,CasaID,Nombre,Apellido,NombreCasa")] Devolucion @devolucion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // var restringClient = (from a in _context.Rental where a.ClientID == @return.ClientID select a).OrderByDescending(a => a.RentalID);
                    // var restringClientOrdenado = restringClient.FirstOrDefault();
                    var ClienteID = (from a in _context.Alquiler where a.ClienteID == @devolucion.ClienteID && a.CasaID == @devolucion.CasaID select a).SingleOrDefault();
                    if(ClienteID  != null)
                    {
                        // if(restringClientOrdenado.HouseID == @return.HouseID)
                        if (ClienteID.Date < @devolucion.DevolucionDate)
                        {
                            var Casa = (from a in _context.Casas where a.CasaID == @devolucion.CasaID select a).SingleOrDefault();
                            var Cliente = (from a in _context.Clientes where a.ClienteID == @devolucion.ClienteID select a).SingleOrDefault();
                            @devolucion.NombreCasa = Casa.CasaNombre;
                            @devolucion.Nombre = Cliente.Nombre ;
                            @devolucion.ClienteID = Cliente.ClienteID;
                            @devolucion.CasaID = Casa.CasaID;
                            Casa.EstaAlquilada = false;
                            _context.Add(@devolucion);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        
                    }
                }
                catch (System.Exception ex){
                    var error = ex;
                }
            }
            ViewData["CasaID"] = new SelectList(_context.Casas.Where(x => x.EstaAlquilada == true && x.IsDeleted == false), "CasaID", "CasaNombre");
            ViewData["AlquilerId"] = new SelectList(_context.Alquiler, "AlquilerID", "CasaID", "ClienteID");
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID","Nombre");
            return View(@devolucion);

        }
        // public async Task<IActionResult> Create([Bind("DevolucionId,DevolucionDate,ClienteID,Nombre,Apellido,CasaID,NombreCasa")] Devolucion devolucion)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(devolucion);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["CasaID"] = new SelectList(_context.Casas, "CasaID", "CasaID", devolucion.CasaID);
        //     ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "ClienteID", devolucion.ClienteID);
        //     return View(devolucion);
        // }

        // GET: Devoluciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Devolucion == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion.FindAsync(id);
            if (devolucion == null)
            {
                return NotFound();
            }
            ViewData["CasaID"] = new SelectList(_context.Casas, "CasaID", "CasaID", devolucion.CasaID);
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "ClienteID", devolucion.ClienteID);
            return View(devolucion);
        }

        // POST: Devoluciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DevolucionId,DevolucionDate,ClienteID,Nombre,Apellido,CasaID,NombreCasa")] Devolucion devolucion)
        {
            if (id != devolucion.DevolucionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devolucion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevolucionExists(devolucion.DevolucionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CasaID"] = new SelectList(_context.Casas, "CasaID", "CasaID", devolucion.CasaID);
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "ClienteID", devolucion.ClienteID);
            return View(devolucion);
        }

        // GET: Devoluciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Devolucion == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion
                .Include(d => d.Casa)
                .Include(d => d.Cliente)
                .FirstOrDefaultAsync(m => m.DevolucionId == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // POST: Devoluciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Devolucion == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Devolucion'  is null.");
            }
            var devolucion = await _context.Devolucion.FindAsync(id);
            if (devolucion != null)
            {
                _context.Devolucion.Remove(devolucion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevolucionExists(int id)
        {
          return _context.Devolucion.Any(e => e.DevolucionId == id);
        }
    }
}
