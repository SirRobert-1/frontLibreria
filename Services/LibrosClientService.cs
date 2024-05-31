using frontendnet.Models;

namespace frontendnet.Services;

public class LibrosClientService(HttpClient client)
{
    public async Task<List<Libro>?> GetAsync(string? search)
    {
        return await client.GetFromJsonAsync<List<Libro>>($"api/libros?s={search}");
    }

    public async Task<Libro?> GetAsync(int id)
    {
        return await client.GetFromJsonAsync<Libro>($"api/libros/{id}");
    }

    public async Task<bool> PostAsync(Libro libro)
    {
        var response = await client.PostAsJsonAsync($"api/libros", libro);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync(Libro libro)
    {
        var response = await client.PutAsJsonAsync($"api/libros/{libro.LibroId}", libro);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await client.DeleteAsync($"api/libros/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PostAsync(int id, int generoId)
    {
        var response = await client.PostAsJsonAsync($"api/libros/{id}/generos", new { generoId });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id, int generoId)
    {
        var response = await client.DeleteAsync($"api/libros/{id}/generos/{generoId}");
        return response.IsSuccessStatusCode;
    }

}