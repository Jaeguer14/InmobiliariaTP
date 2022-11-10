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
    public class CasasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CasasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Casas
        public async Task<IActionResult> Index()
        {
              return View(await _context.Casas.ToListAsync());
        }

        // GET: Casas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Casas == null)
            {
                return NotFound();
            }

            var casa = await _context.Casas
                .FirstOrDefaultAsync(m => m.CasaID == id);
            if (casa == null)
            {
                return NotFound();
            }

            return View(casa);
        }

        // GET: Casas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Casas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CasaID,Nombre,Domicilio,DueñoNombre,Imagen,EstaAlquilada")] Casa casa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(casa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(casa);
        }

        // GET: Casas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Casas == null)
            {
                return NotFound();
            }

            var casa = await _context.Casas.FindAsync(id);
            if (casa == null)
            {
                return NotFound();
            }
            return View(casa);
        }

        // POST: Casas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CasaID,Nombre,Domicilio,DueñoNombre,Imagen,EstaAlquilada")] Casa casa)
        {
            if (id != casa.CasaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(casa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasaExists(casa.CasaID))
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
            return View(casa);
        }

        // GET: Casas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Casas == null)
            {
                return NotFound();
            }

            var casa = await _context.Casas
                .FirstOrDefaultAsync(m => m.CasaID == id);
            if (casa == null)
            {
                return NotFound();
            }

            return View(casa);
        }

        // POST: Casas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Casas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Casas'  is null.");
            }
            var casa = await _context.Casas.FindAsync(id);
            if (casa != null)
            {
                _context.Casas.Remove(casa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasaExists(int id)
        {
          return _context.Casas.Any(e => e.CasaID == id);
        }
    }
}
