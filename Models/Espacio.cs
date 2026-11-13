namespace tpfinal.Models;
public class Espacio
{
        public int Id {get; set;}
        public string Nombre {get; set;}
        public string Descripcion {get; set;}
        public string Imagen {get; set;}
        public int Capacidad {get; set;}
        public int IdCategoria {get; set;}
        public bool Activo {get; set;}
}