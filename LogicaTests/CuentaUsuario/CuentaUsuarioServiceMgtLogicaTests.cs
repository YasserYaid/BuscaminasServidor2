using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Tests
{
    [TestClass()]
    public class CuentaUsuarioServiceMgtLogicaTests
    {
        [TestMethod]
        public void ObtenerIdJugador_Prueba1()
        {
            int id = CuentaUsuarioServiceMgtLogica.ObtenerIdJugador("Juan");
            Assert.AreEqual(1, id);
        }

        [TestMethod]
        public void ObtenerIdJugador_Prueba2()
        {
            int id = CuentaUsuarioServiceMgtLogica.ObtenerIdJugador("No registrado");
            Assert.AreEqual(-1, id); ;
        }

        [TestMethod]
        public void ObtenerIdJugador_Prueba3()
        {
            int id = CuentaUsuarioServiceMgtLogica.ObtenerIdJugador("");
            Assert.AreEqual(-1, id); ;
        }

        [TestMethod]
        public void Existe_Prueba1()
        {
            bool estado = CuentaUsuarioServiceMgtLogica.VerificarExistenciaJugador("Beto");
            Assert.AreEqual(true, estado);
        }

        [TestMethod]
        public void Existe_Prueba2()
        {
            bool estado = CuentaUsuarioServiceMgtLogica.VerificarExistenciaJugador("UsuarioFalso");
            Assert.AreEqual(false, estado);
        }

        [TestMethod]
        public void Existe_Prueba3()
        {
            bool estado = CuentaUsuarioServiceMgtLogica.VerificarExistenciaJugador("");
            Assert.AreEqual(false, estado);
        }


        [TestMethod]
        public void ExisteCuenta_Prueba2()
        {
            CuentaUsuarioServiceMgtLogica cuentaExistente = new CuentaUsuarioServiceMgtLogica();
            EstadoOperacion estado = cuentaExistente.ValidarCredenciales("Cristiano Ronaldo", "Cristiano@gmail.com");
            Assert.AreEqual(EstadoOperacion.FALLIDO, estado);
        }
    }
}