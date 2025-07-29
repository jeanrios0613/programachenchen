namespace managerelchenchenvuelve.Models
{
    public class AsignacionClass
    {
        public int Id { get; set; }
        public string? Usuario { get; set; }   
        public string? NombreCompleto { get; set; }  
        
        public string? Letters { get; set; }
        public List<string>? Ids { get; set; }
    }
}
