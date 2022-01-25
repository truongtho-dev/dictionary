using Dictionary.Models;
using Dictionary.Models.Entities;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dictionary.Controllers
{
    [Authorize(Roles = "admin")]
    public class LanguageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Languages
        public async Task<ActionResult> Index()
        {
            return View(await db.Languages.ToListAsync());
        }

        // GET: Languages/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = await db.Languages.FindAsync(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // GET: Languages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,Name")] Language language)
        {
            if (ModelState.IsValid)
            {
                db.Languages.Add(language);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(language);
        }

        // GET: Languages/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = await db.Languages.FindAsync(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,Name")] Language language)
        {
            if (ModelState.IsValid)
            {
                db.Entry(language).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(language);
        }

        // GET: Languages/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = await db.Languages.FindAsync(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Language language = await db.Languages.FindAsync(id);
            db.Languages.Remove(language);
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
