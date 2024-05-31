using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Genero
{
    [Display(Name = "Id")]
    public int? GeneroId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public required string Nombre { get; set; }

    [Display(Name = "Eliminable")]
    public bool Protegida { get; set; } = false;
}