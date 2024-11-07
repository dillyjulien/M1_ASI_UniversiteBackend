using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Util;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Parcours> ExecuteAsync(string nomparcours, int anneparcours)
    {
        var parcours = new Parcours(){NomParcours = nomparcours, AnneeFormation = anneparcours};
        return await ExecuteAsync(parcours);
    }
    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        Parcours par = await repositoryFactory.ParcoursRepository().CreateAsync(parcours);
        repositoryFactory.ParcoursRepository().SaveChangesAsync().Wait();
        return par;
    }
    private async Task CheckBusinessRules(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(parcours.Id);
        ArgumentNullException.ThrowIfNull(parcours.NomParcours);
        ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
        ArgumentNullException.ThrowIfNull(repositoryFactory.ParcoursRepository());
        
        // On recherche un parcours avec le même numéro de parcours
        List<Parcours> existe = await repositoryFactory.ParcoursRepository().FindByConditionAsync(par=>par.Id.Equals(parcours.Id));

        // Si un parcours avec le même nom de parcours existe déjà, on lève une exception personnalisée
        if (existe .Any()) throw new DuplicateNomParcoursException(parcours.NomParcours+ " - ce nom de parcours est déjà créer");
        
        // Vérification de l'anne du parcours est valide
        if(parcours.AnneeFormation != 1 && parcours.AnneeFormation != 2) throw new InvalidAnneFormationParcoursException(parcours.NomParcours+ " - l'année de fomation est impossible");
        
        //Vérification de l'année de parcours existe déjà
        if (existe .Any()) throw new DuplicateNumParcoursException(parcours.AnneeFormation+ " - ce numéro de parcours existe déjà");
    }
}