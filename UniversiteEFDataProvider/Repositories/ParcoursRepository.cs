using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;
using Microsoft.EntityFrameworkCore;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository : Repository<Parcours>, IParcoursRepository
{
    public ParcoursRepository(UniversiteDbContext context) : base(context) { }

    public async Task AjouterParcoursAsync(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        await Context.Parcours.AddAsync(parcours);
        await Context.SaveChangesAsync();
    }

    public async Task<Parcours?> ObtenirParcoursParIdAsync(long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        return await Context.Parcours
            .Include(p => p.Inscrits)
            .Include(p => p.UesEnseignees)
            .FirstOrDefaultAsync(p => p.Id == idParcours);
    }

    public async Task<List<Parcours>> ObtenirTousLesParcoursAsync()
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        return await Context.Parcours
            .Include(p => p.Inscrits)
            .Include(p => p.UesEnseignees)
            .ToListAsync();
    }

    public async Task SupprimerParcoursAsync(long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        Parcours parcours = (await Context.Parcours.FindAsync(idParcours))!;
        Context.Parcours.Remove(parcours);
        await Context.SaveChangesAsync();
    }

    public async Task ModifierParcoursAsync(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        Context.Parcours.Update(parcours);
        await Context.SaveChangesAsync();
    }

    public async Task<List<Etudiant>> ObtenirEtudiantsParParcoursAsync(long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        var parcours = await ObtenirParcoursParIdAsync(idParcours);
        return parcours?.Inscrits ?? new List<Etudiant>();
    }

    public async Task<List<Ue>> ObtenirUesParParcoursAsync(long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        var parcours = await ObtenirParcoursParIdAsync(idParcours);
        return parcours?.UesEnseignees ?? new List<Ue>();
    }

    public async Task AjouterEtudiantAuParcoursAsync(long idParcours, Etudiant etudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        var parcours = await ObtenirParcoursParIdAsync(idParcours);
        if (parcours == null)
            throw new InvalidOperationException("Parcours introuvable.");

        parcours.Inscrits?.Add(etudiant);
        await Context.SaveChangesAsync();
    }

    public async Task AjouterUeAuParcoursAsync(long idParcours, Ue ue)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        var parcours = await ObtenirParcoursParIdAsync(idParcours);
        if (parcours == null)
            throw new InvalidOperationException("Parcours introuvable.");

        parcours.UesEnseignees?.Add(ue);
        await Context.SaveChangesAsync();
    }

    public Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant)
    {
        throw new NotImplementedException();
    }

    public Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant)
    {
        throw new NotImplementedException();
    }

    public Task<Parcours> AddEtudiantAsync(Parcours? parcours, List<Etudiant> etudiants)
    {
        throw new NotImplementedException();
    }

    public Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants)
    {
        throw new NotImplementedException();
    }

    public Task<Parcours> AddUeAsync(Parcours parcours, Ue UniteEnseignement)
    {
        throw new NotImplementedException();
    }

    public Task<Parcours> AddUeAsync(long idParcours, long IdUe)
    {
        throw new NotImplementedException();
    }

    public Task<Parcours> AddUeAsync(Parcours? parcours, List<Ue> UniteEnseignement)
    {
        throw new NotImplementedException();
    }

    public Task<Parcours> AddUeAsync(long idParcours, long[] IdUe)
    {
        throw new NotImplementedException();
    }
}