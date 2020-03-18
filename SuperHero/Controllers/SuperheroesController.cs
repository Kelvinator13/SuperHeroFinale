using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperHero.Data;
using SuperHero.Models;

namespace SuperHero.Controllers
{
    public class SuperheroesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuperheroesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Superheroes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Superheroes.ToListAsync());
        }

        // GET: Superheroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superheroes = await _context.Superheroes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (superheroes == null)
            {
                return NotFound();
            }

            return View(superheroes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SuperheroName,AlterEgo,PrimaryAbility,SecondaryAbility,Catchphrase")] Superheroes superheroes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(superheroes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(superheroes);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superheroes = await _context.Superheroes.FindAsync(id);
            if (superheroes == null)
            {
                return NotFound();
            }
            return View(superheroes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SuperheroName,AlterEgo,PrimaryAbility,SecondaryAbility,Catchphrase")] Superheroes superheroes)
        {
            if (id != superheroes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(superheroes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuperheroesExists(superheroes.Id))
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
            return View(superheroes);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superheroes = await _context.Superheroes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (superheroes == null)
            {
                return NotFound();
            }

            return View(superheroes);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var superheroes = await _context.Superheroes.FindAsync(id);
            _context.Superheroes.Remove(superheroes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuperheroesExists(int id)
        {
            return _context.Superheroes.Any(e => e.Id == id);
        }
    }
}
