using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Util;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;

namespace UniversiteDomain.UseCases.NoteUsseCases.Create;

public class CreateNoteUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Note> ExecuteAsync(double value, long idetudiant, long idue)
    {
        var note = new Note{Value = value, IdEtudiant = idetudiant, IdUe = idue};
        return await ExecuteAsync(note);
    }
    public async Task<Note> ExecuteAsync(Note note)
    {
        await CheckBusinessRules(note);
        Note nt = await repositoryFactory.NoteRepository().CreateAsync(note);
        repositoryFactory.NoteRepository().SaveChangesAsync().Wait();
        return nt;
    }
    private async Task CheckBusinessRules(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        ArgumentNullException.ThrowIfNull(note.Value);
        ArgumentNullException.ThrowIfNull(note.IdEtudiant);
        ArgumentNullException.ThrowIfNull(note.IdUe);
        ArgumentNullException.ThrowIfNull(repositoryFactory.NoteRepository());
        
        // Une note doit être compris entre 0 et 20
        if (note.Value < 0 || note.Value > 20) throw new InvalidNoteException("La note doit être comprise entre 0 et 20");
        
        // Un étudiant ne peut avoir qu'une seul note dans un UE
        List<Note> noteList = await repositoryFactory.NoteRepository().FindByConditionAsync(e=>e.IdUe.Equals(note.IdUe) && e.IdEtudiant.Equals(note.IdEtudiant));
        if (noteList is { Count:>0 }){
            throw new InvalidNoteException("Un étudiant ne peut avoir qu'une seul note dans un UE");
        }

        List<Parcours> listParcours = await repositoryFactory.ParcoursRepository().FindByConditionAsync(p => p.Inscrits.Any(e=>e.Id == note.IdEtudiant) && p.UesEnseignees.Any(e => e.Id == note.IdUe));
        // Le métier définit que les nom doite contenir plus de 3 lettres
        if (listParcours is { Count: 0 }){
            throw new EtudiantNonInscritException("L'étudiant n'est pas inscrit dans la parcours");
        }
    }
}