using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FormChenchen.Models;

public partial class Contact
{

    public Guid Id { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    public string Email { get; set; } = null!;

    [NotMapped]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? Nombre { get; set; }
    [NotMapped]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? Apellido { get; set; }

    
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string FullName
    {
        get => $"{Nombre} {Apellido}";
        set { }
    }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(25, MinimumLength = 5, ErrorMessage = "La Identificacion no puede ser menor a 5 Carácteres")]
    public string IdentificationNumber { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string IdentificationType { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(9,  ErrorMessage = "Complete un numero de telefono valido")] 
    [RegularExpression(@"6\d{3}-\d{4}", ErrorMessage = "El número debe comenzar con 6 y tener el formato 6000-0000")] 
    public string Phone { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public Guid RequestId { get; set; }

    public virtual Request Request { get; set; } = null!;
}