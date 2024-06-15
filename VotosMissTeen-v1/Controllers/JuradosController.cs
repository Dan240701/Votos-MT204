using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VotosMissTeen_v1.Models;

namespace VotosMissTeen_v1.Controllers
{
    public class JuradosController : Controller
    {
        private ModelVotosContainer db = new ModelVotosContainer();

        // GET: Jurados
        public ActionResult Index()
        {
            return View(db.Jurados.ToList());
        }

        // GET: Jurados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jurado jurado = db.Jurados.Find(id);
            if (jurado == null)
            {
                return HttpNotFound();
            }
            return View(jurado);
        }

        // GET: Jurados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jurados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Username,Lugar")] Jurado jurado)
        {
            if (ModelState.IsValid)
            {
                db.Jurados.Add(jurado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jurado);
        }

        // GET: Jurados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jurado jurado = db.Jurados.Find(id);
            if (jurado == null)
            {
                return HttpNotFound();
            }
            return View(jurado);
        }

        // POST: Jurados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Username,Lugar")] Jurado jurado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jurado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jurado);
        }

        // GET: Jurados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jurado jurado = db.Jurados.Find(id);
            if (jurado == null)
            {
                return HttpNotFound();
            }
            return View(jurado);
        }

        // POST: Jurados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jurado jurado = db.Jurados.Find(id);
            db.Jurados.Remove(jurado);
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
