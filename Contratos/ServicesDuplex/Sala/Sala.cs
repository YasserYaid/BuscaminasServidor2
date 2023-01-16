using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Contratos
{
    [DataContract]
    public class Sala
    {
        [DataMember]
        public string codigoSala { get; set; }
        [DataMember]
        public int turno { get; set; }
        [DataMember]
        public Tablero tablero { get; set; }
        [DataMember]
        public Dictionary<string, ISalaServiceCallback> jugadoresSalaCallback { get; set; }
        [DataMember]
        public Dictionary<string, IChatServiceCallback> jugadoresChatCallback { get; set; }
        [DataMember]
        public Dictionary<string, IJuegoServiceCallback> jugadoresJuegoCallback { get; set; }
    }
}
