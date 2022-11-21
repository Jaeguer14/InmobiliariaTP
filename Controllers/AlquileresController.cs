using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Data;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    public class AlquileresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlquileresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Alquileres
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Alquiler.Include(a => a.Casa).Include(a => a.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Alquileres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Alquiler == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler
                .Include(a => a.Casa)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(m => m.AlquilerId == id);
            if (alquiler == null)
            {
                return NotFound();
            }

            return View(alquiler);
        }

        // GET: Alquileres/Create
        public IActionResult Create()
        {
            ViewData["CasaID"] = new SelectList(_context.Casas, "CasaID", "CasaID");
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "ClienteID");
            return View();
        }

        // POST: Alquileres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlquilerId,Date,ClienteID,Nombre,Apellido,CasaID,NombreCasa")] Alquiler alquiler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alquiler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CasaID"] = new SelectList(_context.Casas, "CasaID", "CasaID", alquiler.CasaID);
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "ClienteID", alquiler.ClienteID);
            return View(alquiler);
        }

        // GET: Alquileres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Alquiler == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler.FindAsync(id);
            if (alquiler == null)
            {
                return NotFound();
            }
            ViewData["CasaID"] = new SelectList(_context.Casas, "CasaID", "CasaID", alquiler.CasaID);
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "ClienteID", alquiler.ClienteID);
            return View(alquiler);
        }

        // POST: Alquileres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlquilerId,Date,ClienteID,Nombre,Apellido,CasaID,NombreCasa")] Alquiler alquiler)
        {
            if (id != alquiler.AlquilerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alquiler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlquilerExists(alquiler.AlquilerId))
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
            ViewData["CasaID"] = new SelectList(_context.Casas, "CasaID", "CasaID", alquiler.CasaID);
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "ClienteID", alquiler.ClienteID);
            return View(alquiler);
        }

        // GET: Alquileres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alquiler == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler
                .Include(a => a.Casa)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(m => m.AlquilerId == id);
            if (alquiler == null)
            {
                return NotFound();
            }

            return View(alquiler);
        }

        // POST: Alquileres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Alquiler == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Alquiler'  is null.");
            }
            var alquiler = await _context.Alquiler.FindAsync(id);
            if (alquiler != null)
            {
                _context.Alquiler.Remove(alquiler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlquilerExists(int id)
        {
          return _context.Alquiler.Any(e => e.AlquilerId == id);
        }
    }
}
