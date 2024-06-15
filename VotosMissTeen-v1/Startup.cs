using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Linq;
using VotosMissTeen_v1.Models;

[assembly: OwinStartupAttribute(typeof(VotosMissTeen_v1.Startup))]
namespace VotosMissTeen_v1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //Eliminar();
           // CrearRolesUsuario();
        }

        public void CrearRolesUsuario()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var ManejadorRol = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
             ModelVotosContainer db = new ModelVotosContainer(); // Aquí debe coincidir con el nombre correcto de tu contexto de base de datos


            if (!ManejadorRol.RoleExists("Admin"))
            {
                var rol = new IdentityRole();
                rol.Name = "Admin";
                ManejadorRol.Create(rol);

                var user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "Admin@sistemas.com";
                string PWD = "Miss.2024";
                var chkUser = ManejadorUsuario.Create(user, PWD);

                if (chkUser.Succeeded)
                {
                    ManejadorUsuario.AddToRole(user.Id, "Admin");
                }
            }

            if (!ManejadorRol.RoleExists("Jurado"))
            {
                var rol = new IdentityRole();
                rol.Name = "Jurado";
                ManejadorRol.Create(rol);

                List<Juradoss> jurados = new List<Juradoss>()
                {
                    new Juradoss("Voto Publico", "Publico", "JPublico"),
                    new Juradoss("Luis Morales", "Arquitecto", "LMorales" ),
                    new Juradoss("Grethel Gamez", "Miss Teen Mesoamérica 2023", "GGamez"),
                    new Juradoss("Alexander Velásquez", "Director y Representante Truss Internacional", "AVelasquez"),
                    new Juradoss("Karla Cuadra", "Gerente General Centro Convenciones Olof Palme", "KCuadra"),
                    new Juradoss("Camila Ortega", "Directora Nicaragua Diseña", "COrtega"),
                    new Juradoss("Luciana Velasquez", "Miss Teen Universe 2024", "LVelasquez"),
                    new Juradoss("Alexander Montiel", "Director CEO Teen Universe Internacional", "AMontiel"),
                    new Juradoss("Reyna Ruedas", "Alcaldesa de Managua", "RRuedas"),
                    new Juradoss("Javiera Perez", "Co Directora de Instituto de las Culturas de Los Pueblos y Juventudes", "JPerez")
                };

                foreach (var item in jurados)
                {
                    var user = new ApplicationUser();
                    user.UserName = item.Username;
                    user.Email = item.Username + "@missteen.com";
                    string PWD = "Miss.2024";
                    var chkUser = ManejadorUsuario.Create(user, PWD);

                    if (chkUser.Succeeded)
                    {
                        ManejadorUsuario.AddToRole(user.Id, "Jurado");
                        Jurado jurado = new Jurado(); // Asumiendo que tu entidad se llama Jurado y no Jurados
                        jurado.Nombre = item.Nombre;
                        jurado.Username = item.Username;
                        jurado.Lugar = item.Lugar;
                        db.Jurados.Add(jurado); // Asegúrate de que el nombre de la tabla sea el correcto
                        db.SaveChanges();
                    }
                }
            }

                List<Candidatas> candidatas = new List<Candidatas>() {
                   new Candidatas("participante1.jpg", "Jilma Tórrez", "18 años", "MATAGALPA", true),
                    new Candidatas("participante2.jpg", "Victoria Downs", "17 años", "ESTELI", true),
                    new Candidatas("participante3.jpg", "Beverly Briceño", "18 años", "MANAGUA", true),
                    new Candidatas("participante4.jpg", "Hanny Martínez", "18 años", "RIVAS", true),
                    new Candidatas("participante5.jpg", "Lindsay Mena", "17 años", "MANAGUA", true),
                    new Candidatas("participante6.jpg", "Madeling Castillo", "18 años", "MATAGALPA", true),
                    new Candidatas("participante7.jpg", "Ekatherine Castellón", "17 años", "GRANADA", false),
                    new Candidatas("participante8.jpg", "Sofía Pulido", "18 años", "LEON", false),
                    new Candidatas("participante9.jpg", "Edmara Aguilar", "18 años", "BOACO", false),
                    new Candidatas("participante10.jpg", "Nicole Chávez", "18 años", "CARAZO", false),
                    new Candidatas("participante11.jpg", "Gabriela Collado", "17 años", "MANAGUA", false),
                    new Candidatas("participante12.jpg", "Alejandra Guerrero", "18 años", "MANAGUA", false),
                    new Candidatas("participante13.jpg", "Leah Zeledón", "14 años", "JINOTEGA", false),
                    new Candidatas("participante14.jpg", "Jennifer Palacios", "18 años", "MANAGUA", false),
                    new Candidatas("participante15.jpg", "María Alejandrina Pérez", "18 años", "CARAZO", false),
                    new Candidatas("participante16.jpg", "Dulce Sunsin", "17 años", "MASAYA", false)


                 };
            foreach (var item in candidatas)
            {
                Participante participante = new Participante();
                participante.Nombre = item.Nombre;
                participante.Foto = item.Foto;
                participante.Edad = item.Edad;
                participante.Departamento = item.Departamento;
                participante.Clasificacion = item.Clasificacion;
                db.Participantes.Add(participante);
                db.SaveChanges();
            }


            // Fases
            List<Fasex> fb = new List<Fasex>() {
                new Fasex("fase1.jpg","pasarela de traje de noche","pasarela","13/06/2024", false),
                new Fasex("fase2.jpg","pasarela de traje de noche","pasarela","14/06/2024", true),
                new Fasex("fase3.jpg","pasarela de traje de noche ","pasarela","15/06/2024", true),
            };
            foreach (var item in fb)
            {
                Fase fs = new Fase();
                fs.Foto = item.Foto;
                fs.NombreFase = item.Nombre;
                fs.Descripcion = item.Descripcion;
                fs.Fecha = item.Fecha;
                fs.Estado = item.Estado;
                db.Fases.Add(fs);
                db.SaveChanges();

            }


            //Votos
            double[] puntuaciones = { 65.44, 64.38, 62.75, 57.25, 57.13, 55.63, 53.88, 52.75, 51.94, 51.19, 50.44, 49.50, 48.31, 46.88, 46.38, 45.88 };


            for (int participante = 1; participante <= 16; participante++)
            {
                    Votos vot = new Votos();
                    vot.JuradoId = 1;
                    vot.FaseId = 1;
                    vot.ParticipanteId = participante;
                    vot.Puntuacion = puntuaciones[participante-1];
                    db.Votos.Add(vot);
                
            }
            db.SaveChanges();

            for (int fase = 2; fase <= 3; fase++)  // Cambiado a <= para incluir la fase 4
            {
                for (int jurado = 2; jurado <= 8; jurado++)
                {
                    for (int parti = 1; parti <= 16; parti++)
                    {
                        // Crear un nuevo objeto Votos xd
                        Votos vot = new Votos();
                        vot.JuradoId = jurado;        
                        vot.ParticipanteId = parti;  
                        vot.FaseId = fase;          
                        vot.Puntuacion = 0;           
                        db.Votos.Add(vot);
                    }
                }
            }
            //Se guarda fueraaa
            db.SaveChanges();

        }
        public static void Eliminar()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                // Eliminar roles existentes
                var roles = roleManager.Roles.ToList();
                foreach (var role in roles)
                {
                    roleManager.Delete(role);
                }
            }
            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Eliminar usuarios existentes
                var users = userManager.Users.ToList();
                foreach (var user in users)
                {
                    userManager.Delete(user);
                }
            }

        }
    }
    
    public class Juradoss
    {
        public Juradoss(string nombre, string lugar, string username)
        {
            this.Nombre = nombre;
            this.Lugar = lugar;
            this.Username = username;
        }

        public string Nombre { get; set; }
        public string Lugar { get; set; }
        public string Username { get; set; }
    }

    public class Candidatas
    {
        public Candidatas(string foto, string nombre, string edad, string departamento, bool clasificacion)
        {
            Foto = foto;
            Nombre = nombre;
            Edad = edad;
            Departamento = departamento;
            Clasificacion = clasificacion;
        }

        public string Foto { get; set; }
        public string Nombre { get; set; }
        public string   Edad { get; set; }
        public string Departamento { get; set; }
        public  bool Clasificacion { get; set; }
    }

    public class VotosxParticipante
    {
        public VotosxParticipante(double puntuacion, int participanteId, int faseId, int juradoId)
        {
            Puntuacion = puntuacion;
            ParticipanteId = participanteId;
            FaseId = faseId;
            JuradoId = juradoId;
        }

        public double Puntuacion { get; set; }
        public int ParticipanteId { get; set; }
        public int FaseId { get; set; }
        public int JuradoId { get; set; }
    }

    public class Fasex
    {
        public Fasex(string foto, string nombre, string descripcion, string fecha, bool estado)
        {
            Foto = foto;
            Nombre = nombre;
            Descripcion = descripcion;
            Fecha = fecha;
            this.Estado = estado;
        }

        public string Foto { get; set; }
        public  string Nombre { get; set; }
        public  string Descripcion { get; set; }
        public string  Fecha { get; set; }
        public bool Estado { get; set; }
    }
}
