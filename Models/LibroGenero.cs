using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class LibroGenero
{
    [Display(Name = "Genero")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int? GeneroId { get; set; }

    public string? Nombre { get; set; }

    public Libro? Libro { get; set; }
}