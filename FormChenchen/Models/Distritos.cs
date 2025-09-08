using System.ComponentModel.DataAnnotations;

namespace FormChenchen.Models
{
    public class Distritos
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int province_id { get; set; }
        public Provincias Provincia { get; set; } = null!;

        // Relación: Un distrito tiene muchos corregimientos
        public ICollection<Corregimientos> Corregimientos { get; set; } = new List<Corregimientos>();
    }
}
