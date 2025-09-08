using System.ComponentModel.DataAnnotations;

namespace FormChenchen.Models
{
    public class Provincias
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Relación: Una provincia tiene muchos distritos
        public ICollection<Distritos> Distritos { get; set; } = new List<Distritos>();
    }
}
