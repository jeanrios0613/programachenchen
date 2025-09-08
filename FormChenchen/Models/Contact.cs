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
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? Nombre { get; set; }


    [NotMapped]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El Apellido solo puede contener letras.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El Apellido debe tener entre 2 y 100 caracteres")]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? Apellido { get; set; }

    
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string FullName
    {
        get => $"{Nombre} {Apellido}";
        set { }
    }


    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(25, MinimumLength = 5, ErrorMessage = "La Identificación no puede ser menor a 5 caracteres")]
    [RegularExpression(@"^(?:\d-\d{1,3}-\d{1,3}|E-\d{1,3}-\d{1,3}|N-\d{1,3}-\d{1,3}|PE-\d{1,3}-\d{1,3})$",
         ErrorMessage = "Formato de cédula inválido. Ej: 8-123-456, E-12-345, N-1-234, PE-12-345")]
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