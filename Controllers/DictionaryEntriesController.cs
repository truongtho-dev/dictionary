using Dictionary.Models;
using Dictionary.Models.Entities;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dictionary.Controllers
{
    [Authorize(Roles = "admin")]
    public class DictionaryEntriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DictionaryEntries
        public async Task<ActionResult> Index()
        {
            var dictionaryEntries = db.DictionaryEntries.Include(d => d.Dictionary);
            return View(await dictionaryEntries.ToListAsync());
        }

        // GET: DictionaryEntries/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictionaryEntry dictionaryEntry = await db.DictionaryEntries.FindAsync(id);
            if (dictionaryEntry == null)
            {
                return HttpNotFound();
            }
            return View(dictionaryEntry);
        }

        // GET: DictionaryEntries/Create
        public ActionResult Create()
        {
            ViewBag.DictionaryId = new SelectList(db.Dictionaries, "Id", "Name");
            return View();
        }

        // POST: DictionaryEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DictionaryId,Word,Meaning")] DictionaryEntry dictionaryEntry)
        {
            if (ModelState.IsValid)
            {
                db.DictionaryEntries.Add(dictionaryEntry);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DictionaryId = new SelectList(db.Dictionaries, "Id", "Name", dictionaryEntry.DictionaryId);
            return View(dictionaryEntry);
        }

        // GET: DictionaryEntries/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictionaryEntry dictionaryEntry = await db.DictionaryEntries.FindAsync(id);
            if (dictionaryEntry == null)
            {
                return HttpNotFound();
            }
            ViewBag.DictionaryId = new SelectList(db.Dictionaries, "Id", "Name", dictionaryEntry.DictionaryId);
            return View(dictionaryEntry);
        }

        // POST: DictionaryEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DictionaryId,Word,Meaning")] DictionaryEntry dictionaryEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dictionaryEntry).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DictionaryId = new SelectList(db.Dictionaries, "Id", "Name", dictionaryEntry.DictionaryId);
            return View(dictionaryEntry);
        }

        // GET: DictionaryEntries/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictionaryEntry dictionaryEntry = await db.DictionaryEntries.FindAsync(id);
            if (dictionaryEntry == null)
            {
                return HttpNotFound();
            }
            return View(dictionaryEntry);
        }

        // POST: DictionaryEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            DictionaryEntry dictionaryEntry = await db.DictionaryEntries.FindAsync(id);
            db.DictionaryEntries.Remove(dictionaryEntry);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
