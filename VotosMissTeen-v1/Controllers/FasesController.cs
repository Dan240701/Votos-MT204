using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VotosMissTeen_v1.Models;

namespace VotosMissTeen_v1.Controllers
{
    [Authorize]
    public class FasesController : Controller
    {
        private ModelVotosContainer db = new ModelVotosContainer();

        // GET: Fases
        public ActionResult Index()
        {
            return View(db.Fases.ToList().Where(f => f.Estado == true));
        }

        // GET: Fases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fase fase = db.Fases.Find(id);
            if (fase == null)
            {
                return HttpNotFound();
            }
            return View(fase);
        }

        // GET: Fases/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fases/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Foto,NombreFase,Descripcion,Fecha,Estado")] Fase fase)
        {
            if (ModelState.IsValid)
            {
                db.Fases.Add(fase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fase);
        }

        // GET: Fases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fase fase = db.Fases.Find(id);
            if (fase == null)
            {
                return HttpNotFound();
            }
            return View(fase);
        }

        // POST: Fases/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Foto,NombreFase,Descripcion,Fecha,Estado")] Fase fase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fase);
        }

        // GET: Fases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fase fase = db.Fases.Find(id);
            if (fase == null)
            {
                return HttpNotFound();
            }
            return View(fase);
        }

        // POST: Fases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fase fase = db.Fases.Find(id);
            db.Fases.Remove(fase);
            db.SaveChanges();
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
