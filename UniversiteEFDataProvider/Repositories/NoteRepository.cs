using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;
using Microsoft.EntityFrameworkCore;

namespace UniversiteEFDataProvider.Repositories;

public class NoteRepository : Repository<Note>, INoteRepository
{
    public NoteRepository(UniversiteDbContext context) : base(context) { }

    public async Task AjouterNoteAsync(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        await Context.Notes.AddAsync(note);
        await Context.SaveChangesAsync();
    }

    public async Task<List<Note>> ObtenirNotesParEtudiantAsync(long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return await Context.Notes
            .Where(n => n.IdEtudiant == idEtudiant)
            .Include(n => n.Etudiant)
            .Include(n => n.Ue)
            .ToListAsync();
    }

    public async Task<List<Note>> ObtenirNotesParUeAsync(long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return await Context.Notes
            .Where(n => n.IdUe == idUe)
            .Include(n => n.Etudiant)
            .Include(n => n.Ue)
            .ToListAsync();
    }

    public async Task SupprimerNoteAsync(long idNote)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        Note note = (await Context.Notes.FindAsync(idNote))!;
        Context.Notes.Remove(note);
        await Context.SaveChangesAsync();
    }

    public async Task ModifierNoteAsync(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        Context.Notes.Update(note);
        await Context.SaveChangesAsync();
    }

    public async Task<double> CalculerMoyenneParEtudiantAsync(long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return await Context.Notes
            .Where(n => n.IdEtudiant == idEtudiant)
            .AverageAsync(n => n.Value);
    }

    public async Task<double> CalculerMoyenneParUeAsync(long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return await Context.Notes
            .Where(n => n.IdUe == idUe)
            .AverageAsync(n => n.Value);
    }
}
