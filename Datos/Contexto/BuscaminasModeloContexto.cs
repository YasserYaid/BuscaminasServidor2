using Datos.Properties.Recursos;
using Datos.BuscaminasModelos;
using System.Data.Entity;

namespace Datos.Contexto
{
    public class BuscaminasModeloContexto : DbContext
    {
        public BuscaminasModeloContexto() : base(Recurso.NombreBaseDatos_MSJCONST)
        {
            Database.SetInitializer<BuscaminasModeloContexto>(new DropCreateDatabaseIfModelChanges<BuscaminasModeloContexto>());
        }
        public virtual DbSet<Jugador> Jugadores { get; set; }
        public virtual DbSet<Sala> Salas { get; set; }
        public virtual DbSet<Amistad> Amistades { get; set; }
    }
}
