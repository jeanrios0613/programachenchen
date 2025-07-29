using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FormChenchen.Models;

public partial class Enterprise
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? BusinessName { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string BusinessDescription { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string EconomicActivity { get; set; } = null!;

    [RegularExpression(@"^[_a-zA-Z0-9][a-zA-Z0-9._]{0,29}$",
    ErrorMessage = "El nombre de usuario de Instagram no es válido.")]
    public string? Instagram { get; set; } 

    [NotMapped]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? RucEmpresa { get; set; }


    [NotMapped]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? DvEmpresa { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Ruc
    {
        get => $"{RucEmpresa} DV  {DvEmpresa}";
        set { }
    }

    //[RegularExpression(@"^(https?:\/\/)?www\.([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w\.-]*)*\/?$",
    //ErrorMessage = "El sitio web no tiene un formato válido.")]

    [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",
    ErrorMessage = "El sitio web no tiene un formato válido.")]
    public string? WebSite { get; set; } = "No Tiene Sitio Web";

    public DateTime CreationDate { get; set; }

    public Guid RequestId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string BusinessTime { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Corregimiento { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string District { get; set; } = null!;

   
    [RegularExpression(@"^(\d{1,3}(,\d{3})*|\d+)(\.\d{1,2})?$", ErrorMessage = "Ingrese un número válido con hasta 2 decimales.")]
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public decimal MonthlySales { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateTime OperationsStartDate { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Province { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [RegularExpression(@"^(\d{1,3}(,\d{3})*|\d+)(\.\d{1,2})?$", ErrorMessage = "Ingrese un número válido con hasta 2 decimales.")]
    public decimal  ProyectedSales { get; set; }

    public virtual Request Request { get; set; } = null!;
}