using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;
using Microsoft.EntityFrameworkCore;

namespace UniversiteEFDataProvider.Repositories;

public class UeRepository : Repository<Ue>, IUeRepository
{
    public UeRepository(UniversiteDbContext context) : base(context) { }

    public async Task AjouterUeAsync(Ue ue)
    {
        ArgumentNullException.ThrowIfNull(ue);
        await Context.Ues.AddAsync(ue);
        await Context.SaveChangesAsync();
    }

    public async Task<Ue?> ObtenirUeParIdAsync(long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        return await Context.Ues
            .Include(u => u.EnseigneeDans)
            .Include(u => u.Notes)
            .FirstOrDefaultAsync(u => u.Id == idUe);
    }

    public async Task<List<Ue>> ObtenirToutesLesUesAsync()
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        return await Context.Ues
            .Include(u => u.EnseigneeDans)
            .Include(u => u.Notes)
            .ToListAsync();
    }

    public async Task SupprimerUeAsync(long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Ue ue = (await Context.Ues.FindAsync(idUe))!;
        Context.Ues.Remove(ue);
        await Context.SaveChangesAsync();
    }

    public async Task ModifierUeAsync(Ue ue)
    {
        ArgumentNullException.ThrowIfNull(ue);
        Context.Ues.Update(ue);
        await Context.SaveChangesAsync();
    }

    public async Task<List<Ue>> ObtenirUesParParcoursAsync(long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        return await Context.Ues
            .Where(u => u.EnseigneeDans!.Any(p => p.Id == idParcours))
            .Include(u => u.EnseigneeDans)
            .Include(u => u.Notes)
            .ToListAsync();
    }

    public async Task<double> CalculerMoyenneParUeAsync(long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        var ue = await ObtenirUeParIdAsync(idUe);
        if (ue == null || ue.Notes == null || !ue.Notes.Any())
            throw new InvalidOperationException("Aucune note trouvée pour cette UE.");

        return ue.Notes.Average(n => n.Value);
    }
}