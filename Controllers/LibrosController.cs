using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

[Authorize(Roles = "Administrador, Usuario")]
public class LibrosController(LibrosClientService libros, GenerosClientService generos) : Controller
{
    public async Task<IActionResult> Index(string? s)
    {
        List<Libro>? lista = [];
        try
        {
            lista = await libros.GetAsync(s);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        if (User.FindFirstValue(ClaimTypes.Role) == "Administrador")
            ViewBag.SoloAdmin = true;

        ViewBag.search = s;
        return View(lista);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        Libro? item = null;
        try
        {
            item = await libros.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(item);
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CrearAsync(Libro itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await libros.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Intente nuevamente.");
        return View(itemToCreate);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> EditarAsync(int id)
    {
        Libro? itemToEdit = null;
        try
        {
            itemToEdit = await libros.GetAsync(id);
            if (itemToEdit == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(itemToEdit);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> EditarAsync(int id, Libro itemToEdit)
    {
        if (id != itemToEdit.LibroId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await libros.PutAsync(itemToEdit);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Intente nuevamente.");
        return View(itemToEdit);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id, bool? showError = false)
    {
        Libro? itemToDelete = null;
        try
        {
            itemToDelete = await libros.GetAsync(id);
            if (itemToDelete == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(itemToDelete);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await libros.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        return RedirectToAction(nameof(Eliminar), new { id, showError = true });
    }

    [AcceptVerbs("GET", "POST")]
    [Authorize(Roles = "Administrador")]
    public IActionResult ValidaPoster(string poster)
    {
        if (Uri.IsWellFormedUriString(poster, UriKind.Absolute) || poster.Equals("N/A"))
            return Json(true);
        return Json(false);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Generos(int id)
    {
        Libro? itemToView = null;
        try
        {
            itemToView = await libros.GetAsync(id);
            if (itemToView == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        ViewData["LibroId"] = itemToView?.LibroId;
        return View(itemToView);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GenerosAgregar(int id)
    {
        LibroGenero? itemToView = null;
        try
        {
            Libro? libro = await libros.GetAsync(id);
            if (libro == null) return NotFound();

            await GenerosDropDownListAsync();
            itemToView = new LibroGenero { Libro = libro };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(itemToView);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GenerosAgregar(int id, int generoId)
    {
        Libro? libro = null;
        if (ModelState.IsValid)
        {
            try
            {
                libro = await libros.GetAsync(id);
                if (libro == null) return NotFound();

                Genero? genero = await generos.GetAsync(generoId);
                return RedirectToAction(nameof(generos), new { id });
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("id", "No ha sido posible realizar la acción. Intente nuevamente.");
        await GenerosDropDownListAsync();
        return View(new LibroGenero { Libro = libro });
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GenerosRemover(int id, int generoId, bool? showError = false)
    {
        LibroGenero? itemToView = null;
        try
        {
            Libro? libro = await libros.GetAsync(id);
            if (libro == null) return NotFound();

            Genero? genero = await generos.GetAsync(generoId);
            if (genero == null) return NotFound();

            itemToView = new LibroGenero { Libro = libro, GeneroId = generoId, Nombre = genero.Nombre };

            if (showError.GetValueOrDefault())
                ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Intente nuevamente.";
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(itemToView);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GenerosRemover(int id, int generoId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await libros.DeleteAsync(id, generoId);
                return RedirectToAction(nameof(Generos), new { id });
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        return RedirectToAction(nameof(GenerosRemover), new { id, generoId, showError = true });
    }

    private async Task GenerosDropDownListAsync(object? itemSeleccionado = null)
    {
        var listado = await generos.GetAsync();
        ViewBag.Genero = new SelectList(listado, "GeneroId", "Nombre", itemSeleccionado);
    }

}