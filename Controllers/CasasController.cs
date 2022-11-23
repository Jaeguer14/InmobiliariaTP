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
            return _context.Casas != null ?
            View(await _context.Casas.ToListAsync()) :
            Problem("Entity set 'InmobiliariaContext.Casas'  is null.");
            // return View(await _context.Casas.ToListAsync());
        }




        // GET: Casas/Create
        [Authorize]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Casas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CasaID,CasaNombre,Domicilio,PropietarioNombre")] Casa casa, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
               if (Imagen != null && Imagen.Length > 0)
                {
                    byte[] ImagenCasa = null;
                    using (var fs1 = Imagen.OpenReadStream())
                    using (var ms1 = new MemoryStream())

                    {
                        fs1.CopyTo(ms1);
                        ImagenCasa = ms1.ToArray();
                    }
                    casa.Imagen = ImagenCasa;

                }
                _context.Add(casa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(casa);
        }

        // GET: Casas/Edit/5
        [Authorize]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CasaID,Nombre,Domicilio,PropietarioNombre")] Casa casa, IFormFile Imagen)
        {
            if (id != casa.CasaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Imagen != null && Imagen.Length > 0)
                {
                    byte[] ImagenCasa = null;
                    using (var fs1 = Imagen.OpenReadStream())
                    using (var ms1 = new MemoryStream())

                    {
                        fs1.CopyTo(ms1);
                        ImagenCasa = ms1.ToArray();
                    }
                    casa.Imagen = ImagenCasa;

                }
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
        [Authorize]
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

        //  POST: Casas/Delete/5
        //  [HttpPost, ActionName("Delete")]
        //  [ValidateAntiForgeryToken]
        [Authorize]
         public async Task<IActionResult> DeleteConfirmed(int id)
         {
            var casa = await _context.Casas.FindAsync(id);
            if (casa.EstaAlquilada == true)
            {
                return RedirectToAction(nameof(Index));
            }
            if (casa != null)
            {
                var houseAlquilada = (from a in _context.Alquiler where a.CasaID == id select a).Count();
                if (houseAlquilada == 0)  
                {
                    _context.Casas.Remove(casa);
                    await _context.SaveChangesAsync();
                } 
                else
                {
                    casa.IsDeleted = true; 
                    casa.CasaNombre = "ELIMINADA";
                    _context.Update(casa);
                    await _context.SaveChangesAsync();
                }             

            }
             return RedirectToAction(nameof(Index));
         }




        //  public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var Casa = await _context.Casas.FindAsync(id);

        //     if (Casa != null)
        //     {
        //         var CasaInAlquilada = (from a in _context.Rental where a.CountryID == id select a).ToList();
        //         if (CasaInAlquilada.Count == 0)
        //         {
        //            _context.Country.Remove(country);
        //            await _context.SaveChangesAsync();  
        //         }
        //         else
        //         {
                    
        //         }
                
        //     }
        //     return RedirectToAction(nameof(Index));
        // }
        
        public IActionResult ConvertirImagen(int id)
        {
            var casa = _context.Casas.FirstOrDefault(i => i.CasaID == id);

            if (casa == null) return NoContent();


            byte[] imageData = casa.Imagen;
            if (imageData != null)
            {
                return File(imageData, casa.ImagenContentType);
            }
            return NoContent();
        }

        private bool CasaExists(int id)
        {
            return _context.Casas.Any(e => e.CasaID == id);
        }
    }
}
