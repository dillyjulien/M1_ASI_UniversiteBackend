using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeExceptions;
using UniversiteDomain.Util;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;

namespace UniversiteDomain.UseCases.UeUseCases.Create;

public class CreateUeUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Ue> ExecuteAsync(string numeroUe, string intitule)
    {
        var unityEnseignement = new Ue{NumeroUe = numeroUe, Intitule = intitule};
        return await ExecuteAsync(unityEnseignement);
    }
    public async Task<Ue> ExecuteAsync(Ue unityEnseignement)
    {
        await CheckBusinessRules(unityEnseignement);
        Ue ue = await repositoryFactory.UeRepository().CreateAsync(unityEnseignement);
        repositoryFactory.UeRepository().SaveChangesAsync().Wait();
        return ue;
    }
    private async Task CheckBusinessRules(Ue unityEnseignement)
    {
        ArgumentNullException.ThrowIfNull(unityEnseignement);
        ArgumentNullException.ThrowIfNull(unityEnseignement.NumeroUe);
        ArgumentNullException.ThrowIfNull(unityEnseignement.Intitule);
        ArgumentNullException.ThrowIfNull(repositoryFactory.UeRepository());
        
        // On recherche un UE avec le même numéro étudiant
        List<Ue> existe = await repositoryFactory.UeRepository().FindByConditionAsync(e=>e.NumeroUe.Equals(unityEnseignement.NumeroUe));

        // L'UE définit à un intitulé suppérieur à 3 lettres
        if (unityEnseignement.Intitule.Length < 3) throw new InvalidIntituleUeException(unityEnseignement.Intitule +" incorrect - L'intitulé UE doit contenir plus de 3 caractères");
        
    }
}
