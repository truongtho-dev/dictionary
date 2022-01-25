using Dictionary.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DictionaryEntity = Dictionary.Models.Entities.Dictionary;

namespace Dictionary.Controllers
{
    [Authorize(Roles = "admin")]
    public class DictionaryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dictionary
        public async Task<ActionResult> Index()
        {
            var dictionaries = db.Dictionaries.Include(d => d.Destination).Include(d => d.Source);
            return View(await dictionaries.ToListAsync());
        }

        // GET: Dictionary/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dictionary = await db.Dictionaries.FindAsync(id);
            if (dictionary == null)
            {
                return HttpNotFound();
            }
            return View(dictionary);
        }

        // GET: Dictionary/Create
        public ActionResult Create()
        {
            ViewBag.DestinationId = new SelectList(db.Languages, "Id", "Code");
            ViewBag.SourceId = new SelectList(db.Languages, "Id", "Code");
            return View();
        }

        // POST: Dictionary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SourceId,DestinationId,Name,Description")] DictionaryEntity dictionary)
        {
            if (ModelState.IsValid)
            {
                db.Dictionaries.Add(dictionary);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DestinationId = new SelectList(db.Languages, "Id", "Code", dictionary.DestinationId);
            ViewBag.SourceId = new SelectList(db.Languages, "Id", "Code", dictionary.SourceId);
            return View(dictionary);
        }

        // GET: Dictionary/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dictionary = await db.Dictionaries.FindAsync(id);
            if (dictionary == null)
            {
                return HttpNotFound();
            }
            ViewBag.DestinationId = new SelectList(db.Languages, "Id", "Code", dictionary.DestinationId);
            ViewBag.SourceId = new SelectList(db.Languages, "Id", "Code", dictionary.SourceId);
            return View(dictionary);
        }

        // POST: Dictionary/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SourceId,DestinationId,Name,Description")] DictionaryEntity dictionary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dictionary).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DestinationId = new SelectList(db.Languages, "Id", "Code", dictionary.DestinationId);
            ViewBag.SourceId = new SelectList(db.Languages, "Id", "Code", dictionary.SourceId);
            return View(dictionary);
        }

        // GET: Dictionary/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dictionary = await db.Dictionaries.FindAsync(id);
            if (dictionary == null)
            {
                return HttpNotFound();
            }
            return View(dictionary);
        }

        // POST: Dictionary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var dictionary = await db.Dictionaries.FindAsync(id);
            db.Dictionaries.Remove(dictionary);
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
