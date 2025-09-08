using System.ComponentModel.DataAnnotations;

namespace FormChenchen.Models
{
    public class Corregimientos
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int district_id { get; set; }
        public Distritos Distrito { get; set; } = null!;
    }
}
