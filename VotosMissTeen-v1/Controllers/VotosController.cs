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
              //Total Participantes x Puntuacion 
                var query = from v in db.Votos
                            join p in db.Participantes on v.ParticipanteId equals p.Id
                            where v.Participante.Clasificacion ==true
                            group v by p.Nombre into g
                            orderby g.Sum(v => v.Puntuacion) descending
                            select new
                            {
                                Nombre = g.Key,
                                TotalPuntuacion = g.Sum(v => v.Puntuacion)
                            };

                var resultados = query.ToList();

            // Definir la lista para almacenar las puntuaciones finales
            var puntuacionesFinales = new List<(int IdParticipante, double PuntuacionFinal)>();

            // Iterar sobre los resultados de la consulta
            foreach (var item in puntuacionesFinales)
            {
                var participanteid= item.IdParticipante;

                // Obtener la puntuación del Evento Popularidad (valor único entre 0 y 100)
                var puntuacionPopularidad = db.Votos
                    .Where(p => p.ParticipanteId == participanteid && p.FaseId == 1)
                    .FirstOrDefault()?.Puntuacion ?? 0;

                // Obtener las puntuaciones del Evento Traje de Gala (6 jurados, cada uno califica entre 0 y 10)
                var puntuacionesGala = db.Votos
                    .Where(p => p.ParticipanteId == participanteid && p.FaseId == 2)
                    .Select(p => p.Puntuacion)
                    .ToList();

                // Calcular el promedio de las puntuaciones del Evento Traje de Gala y escalar a 0-100
                var promedioGala = puntuacionesGala.Any() ? puntuacionesGala.Average() * 10 : 0;

                // Obtener las puntuaciones del Evento Pregunta Final (6 jurados, cada uno califica entre 0 y 10)
                var puntuacionesPregunta = db.Votos
                    .Where(p => p.ParticipanteId == participanteid && p.FaseId == 3)
                    .Select(p => p.Puntuacion)
                    .ToList();

                // Calcular el promedio de las puntuaciones del Evento Pregunta Final y escalar a 0-100
                var promedioPregunta = puntuacionesPregunta.Any() ? puntuacionesPregunta.Average() * 10 : 0;

                // Calcular la puntuación final combinando las puntuaciones ponderadas de cada evento
                var puntuacionFinal = (puntuacionPopularidad * 0.70) +
                                      (promedioGala * 0.15) +
                                      (promedioPregunta * 0.15);

                // Agregar la puntuación final junto con el ID del participante a la lista
                puntuacionesFinales.Add((item.IdParticipante, puntuacionFinal));
            }

            // Ordenar la lista por puntuación final en orden descendente
            var top6 = puntuacionesFinales
                .OrderByDescending(pf => pf.PuntuacionFinal)
                .Take(6)
                .ToList();

            // Imprimir los resultados de los primeros 6 participantes
            foreach (var resultado in top6)
            {
                Console.WriteLine($"ID Participante: {resultado.IdParticipante}, Puntuación Final: {resultado.PuntuacionFinal}");
            }

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
