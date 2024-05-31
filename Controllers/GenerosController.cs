using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Administrador")]
public class GenerosController(GenerosClientService generos) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Genero>? lista = [];
        try
        {
            lista = await generos.GetAsync();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(lista);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        Genero? item = null;
        try
        {
            item = await generos.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(item);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CrearAsync(Genero itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await generos.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Intentalo nuevamente.");
        return View(itemToCreate);
    }

    public async Task<IActionResult> EditarAsync(int id)
    {
        Genero? itemToEdit = null;
        try
        {
            itemToEdit = await generos.GetAsync(id);
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
    public async Task<IActionResult> EditarAsync(int id, Genero itemToEdit)
    {
        if (id != itemToEdit.GeneroId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await generos.PutAsync(itemToEdit);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Intentalo nuevamente.");
        return View(itemToEdit);
    }

    public async Task<IActionResult> Eliminar(int id, bool? showError = false)
    {
        Genero? itemToDelete = null;
        try
        {
            itemToDelete = await generos.GetAsync(id);
            if (itemToDelete == null) return NotFound();

            if (showError.GetValueOrDefault())
                ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Intentalo nuevamente.";
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(itemToDelete);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await generos.DeleteAsync(id);
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
}