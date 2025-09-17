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
    public string? Email { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]{2,100}$",
        ErrorMessage = "El Nombre solo puede contener letras y espacios (2-100 caracteres).")]
    public string? Nombre { get; set; }


    [NotMapped]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]{2,100}$",
        ErrorMessage = "El Apellido solo puede contener letras y espacios (2-100 caracteres).")]
    public string? Apellido { get; set; }
  
    
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string FullName
    {
        get => $"{Nombre} {Apellido}";
        set { }
    }


    [Required(ErrorMessage = "La identificación es obligatoria.")]
    [RegularExpression(
    @"^[A-Za-z0-9-]{5,50}$",
    ErrorMessage = "El campo debe tener entre 5 y 50 caracteres y solo puede contener letras, números y guiones.")]
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