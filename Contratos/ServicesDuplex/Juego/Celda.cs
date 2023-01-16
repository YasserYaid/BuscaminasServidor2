using System.Runtime.Serialization;

namespace Contratos
{
    [DataContract]
    public class Celda
    {
        [DataMember]
        public int filaPosicion { get; set; }
        [DataMember]
        public int columnaPosicion { get; set; }
        [DataMember]
        public bool esMarcada { get; set; }
        [DataMember]
        public bool esMinada { get; set; }
        [DataMember]
        public bool esRevelada { get; set; }
    }
}