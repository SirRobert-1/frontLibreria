using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet.Models;

public class Libro
{
    [Display(Name = "Id")]
    public int? LibroId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public required string Titulo { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [DataType(DataType.MultilineText)]
    public string Sinopsis { get; set; } = "Sin sinopsis";

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1950, 2024, ErrorMessage = "El campo {0} debe estar entre {1} y {2}")]
    [Display(Name = "Año")]
    public int Anio { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Remote(action: "ValidaPortada", controller: "Libros", ErrorMessage = "El campo {0} debe ser una URL válida o N/A.")]
    public string Portada { get; set; } = "N/A";

    [Display(Name = "Eliminable")]
    public bool Protegida { get; set; } = false;
    public ICollection<Genero>? Generos { get; set; }
}