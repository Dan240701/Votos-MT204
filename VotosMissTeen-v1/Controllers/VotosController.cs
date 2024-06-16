using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VotosMissTeen_v1.Models;

namespace VotosMissTeen_v1.Controllers
{
        [Authorize]
    public class VotosController : Controller
    {

        private ModelVotosContainer db = new ModelVotosContainer();

        // GET: Votos
        public ActionResult Index(int? idfase)
        {
                //filtrar
                string username = User.Identity.GetUserName();
                //&& v.FaseId == idfas
                if(idfase == 3)
            {
                var votoes3 = db.Votos.Where(v => v.Jurado.Username == username && v.FaseId == idfase && v.Participante.Clasificacion==true).Include(v => v.Fase).Include(v => v.Jurado).Include(v => v.Participante);
                return View(votoes3.ToList());
            }
                var votoes2 = db.Votos.Where(v => v.Jurado.Username == username && v.FaseId == idfase).Include(v => v.Fase).Include(v => v.Jurado).Include(v => v.Participante);
                return View(votoes2.ToList());
        }

        // Método para el contenido del modal
        public ActionResult ModalContent(int id)
        {
            Votos votos = db.Votos.Find(id);
            if (votos == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", votos); // La vista parcial debe llamarse "_DetailsPartial"
        }

        //NUEVO METODO ADAPTADO
        public ActionResult ObtenerPuntuaciones()
        {
            var query = db.Participantes
       .Where(p => p.Clasificacion) // Filtrar por Clasificacion = true
       .Select(p => new
       {
           ParticipanteId = p.Id,
           Participante = p.Nombre,
           Top6 = p.Clasificacion,
           PuntuacionFase1 = db.Votos
                               .Where(v => v.FaseId == 1 && v.ParticipanteId == p.Id)
                               .Sum(v => (int?)v.Puntuacion) ?? 0,
           PuntuacionFase2 = Math.Round(db.Votos
                                           .Where(v => v.FaseId == 2 && v.ParticipanteId == p.Id)
                                           .Average(v => (double?)v.Puntuacion) * 1.5 ?? 0, 2),
           PuntuacionFase3 = Math.Round(db.Votos
                                           .Where(v => v.FaseId == 3 && v.ParticipanteId == p.Id)
                                           .Average(v => (double?)v.Puntuacion) * 1.5 ?? 0, 2),
       })
       .Select(p => new
       {
           p.ParticipanteId,
           p.Participante,
           p.Top6,
           PuntuacionFase1 = p.PuntuacionFase1,
           PuntuacionFase2 = p.PuntuacionFase2,
           PuntuacionFase3 = p.PuntuacionFase3,
           PuntuacionTotal = Math.Round(p.PuntuacionFase1 + p.PuntuacionFase2 + p.PuntuacionFase3, 2)
       });


            return View("GraficoPartiFinal", query);

        }
        public JsonResult ObtenerInfoFase(int? idfase)
        {
            var fase = db.Fases
                        .Where(f => f.Id == idfase)
                        .Select(f => new {
                            f.Id,
                            f.NombreFase,
                            f.Foto,
                            f.Descripcion,
                            f.Estado
                        })
                        .SingleOrDefault(); // Utiliza SingleOrDefault para obtener un solo resultado o null si no se encuentra

            return Json(fase, JsonRequestBehavior.AllowGet);
        }
        //GET: Votos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Votos votos = db.Votos.Find(id);
            if (votos == null)
            {
                return HttpNotFound();
            }
            return View(votos);
        }

        // GET: Votos/Create
        public ActionResult Create()
        {
            ViewBag.JuradoId = new SelectList(db.Jurados, "Id", "Nombre");
            ViewBag.FaseId = new SelectList(db.Fases, "Id", "Foto");
            ViewBag.ParticipanteId = new SelectList(db.Participantes, "Id", "Foto");
            return View();
        }

        // POST: Votos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Puntuacion,JuradoId,FaseId,ParticipanteId")] Votos votos)
        {
            if (ModelState.IsValid)
            {
                db.Votos.Add(votos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JuradoId = new SelectList(db.Jurados, "Id", "Nombre", votos.JuradoId);
            ViewBag.FaseId = new SelectList(db.Fases, "Id", "Foto", votos.FaseId);
            ViewBag.ParticipanteId = new SelectList(db.Participantes, "Id", "Foto", votos.ParticipanteId);
            return View(votos);
        }

        // GET: Votos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Votos votos = db.Votos.Find(id);
            if (votos == null)
            {
                return HttpNotFound();
            }
            ViewBag.JuradoId = new SelectList(db.Jurados, "Id", "Nombre", votos.JuradoId);
            ViewBag.FaseId = new SelectList(db.Fases, "Id", "Foto", votos.FaseId);
            ViewBag.ParticipanteId = new SelectList(db.Participantes, "Id", "Foto", votos.ParticipanteId);
            return View(votos);
        }

        // POST: Votos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Puntuacion,JuradoId,FaseId,ParticipanteId")] Votos votos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(votos).State = EntityState.Modified;
                db.SaveChanges();
                int idFase = votos.FaseId;

                return RedirectToAction("Index", "Votos", new { idfase = idFase });

            }
            ViewBag.JuradoId = new SelectList(db.Jurados, "Id", "Nombre", votos.JuradoId);
            ViewBag.FaseId = new SelectList(db.Fases, "Id", "Foto", votos.FaseId);
            ViewBag.ParticipanteId = new SelectList(db.Participantes, "Id", "Foto", votos.ParticipanteId);
            return View(votos);
        }

        // GET: Votos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Votos votos = db.Votos.Find(id);
            if (votos == null)
            {
                return HttpNotFound();
            }
            return View(votos);
        }

        // POST: Votos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Votos votos = db.Votos.Find(id);
            db.Votos.Remove(votos);
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
