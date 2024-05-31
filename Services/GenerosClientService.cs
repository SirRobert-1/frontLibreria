using frontendnet.Models;

namespace frontendnet.Services;

public class GenerosClientService(HttpClient client)
{
    public async Task<List<Genero>?> GetAsync()
    {
        return await client.GetFromJsonAsync<List<Genero>>("api/generos");
    }

    public async Task<Genero?> GetAsync(int id)
    {
        return await client.GetFromJsonAsync<Genero>($"api/generos/{id}");
    }

    public async Task<bool> PostAsync(Genero genero)
    {
        var response = await client.PostAsJsonAsync($"api/generos", genero);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync(Genero genero)
    {
        var response = await client.PutAsJsonAsync($"api/generos/{genero.GeneroId}", genero);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await client.DeleteAsync($"api/generos/{id}");
        return response.IsSuccessStatusCode;
    }
}