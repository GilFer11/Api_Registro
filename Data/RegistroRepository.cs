using Dapper;
using MySqlConnector;
using ApiRegistrosDiarios.Models;

namespace ApiRegistrosDiarios.Data;

public class RegistroRepository
{
    private readonly string _cs;

    public RegistroRepository(IConfiguration config)
    {
        _cs = config.GetConnectionString("MySql")!;
    }

    private MySqlConnection GetConn() => new MySqlConnection(_cs);

    public async Task<IEnumerable<RegistroDiario>> GetAllAsync()
    {
        using var conn = GetConn();
        return await conn.QueryAsync<RegistroDiario>(
            "SELECT id, fecha, actividad, descripcion, horas, responsable, observaciones FROM registro ORDER BY fecha DESC");
    }

    public async Task<RegistroDiario?> GetByIdAsync(int id)
    {
        using var conn = GetConn();
        return await conn.QueryFirstOrDefaultAsync<RegistroDiario>(
            "SELECT id, fecha, actividad, descripcion, horas, responsable, observaciones FROM registro WHERE id=@id",
            new { id });
    }

    public async Task<int> CreateAsync(RegistroDiario r)
    {
        using var conn = GetConn();
        var sql = @"INSERT INTO registro (fecha, actividad, descripcion, horas, responsable, observaciones)
                    VALUES (@Fecha, @Actividad, @Descripcion, @Horas, @Responsable, @Observaciones);
                    SELECT LAST_INSERT_ID();";
        return await conn.ExecuteScalarAsync<int>(sql, r);
    }

    public async Task<bool> UpdateAsync(int id, RegistroDiario r)
    {
        using var conn = GetConn();
        var rows = await conn.ExecuteAsync(
            @"UPDATE registro SET
                fecha=@Fecha,
                actividad=@Actividad,
                descripcion=@Descripcion,
                horas=@Horas,
                responsable=@Responsable,
                observaciones=@Observaciones
              WHERE id=@Id",
            new {
                Id = id, r.Fecha, r.Actividad, r.Descripcion,
                r.Horas, r.Responsable, r.Observaciones
            });
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = GetConn();
        var rows = await conn.ExecuteAsync("DELETE FROM registro WHERE id=@id", new { id });
        return rows > 0;
    }
}
