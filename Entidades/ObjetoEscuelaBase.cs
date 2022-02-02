using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEscuela.Entidades
{
    public abstract class ObjetoEscuelaBase
    {
        public string UniqueId { get; set; }
        public string Nombre { get; set; }

        public ObjetoEscuelaBase()
        {
            UniqueId=Guid.NewGuid().ToString();

        }

        public override string ToString()
        {
            return $"Nombre: {Nombre}, ID: {UniqueId}";
        }

    }
}
