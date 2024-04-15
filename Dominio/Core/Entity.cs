using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Core
{
    public abstract class Entity
    {
        protected Entity()
        {
        }

        public Entity(string modificadoPor)
        {
            ModificadoPor = modificadoPor;
        }

        public string ModificadoPor { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string DescripcionTransaccion { get; set; }
        public byte[] RowVersion { get; set; }
        public Guid TransaccionUId { get; set; }
        public string TipoTransaccion { get; set; }
    }
}
