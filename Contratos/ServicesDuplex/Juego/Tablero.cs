using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Contratos
{
    [DataContract]
    public class Tablero
    {
        [DataMember]
        public int ancho { get; set; }
        [DataMember]
        public int alto { get; set; }
        [DataMember]
        public int numeroMinas { get; set; }
        [DataMember]
        public List<Celda> bombas;
    }
}