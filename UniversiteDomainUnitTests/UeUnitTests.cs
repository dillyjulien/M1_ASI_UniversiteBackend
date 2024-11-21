using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.UeUseCases;
using UniversiteDomain.UseCases.UeUseCases.Create;

namespace UniversiteDomainUnitTests;


public class UeUnitTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public async Task CreateUeUseCase()
    {
        long id = 1;
        string numeroUe = "ue1";
        string intitule = "UE n°1";
        
        // On crée l'UE qui doit être ajouté en base
        Ue UniteEnseignementSansId = new Ue{NumeroUe=numeroUe, Intitule = intitule};
        
        //  Créons le mock du repository
        // On initialise une fausse datasource qui va simuler un EtudiantRepository
        var mock = new Mock<IUeRepository>();
        // Il faut ensuite aller dans le use case pour voir quelles fonctions simuler
        // Nous devons simuler FindByCondition et Create
        
        // Simulation de la fonction FindByCondition
        // On dit à ce mock que l'UE n'existe pas déjà
        // La réponse à l'appel FindByCondition est donc une liste vide
        var reponseFindByCondition = new List<Ue>();
        
        // On crée un bouchon dans le mock pour la fonction FindByCondition
        // Quelque soit le paramètre de la fonction FindByCondition, on renvoie la liste vide
        mock.Setup(repo=>repo.FindByConditionAsync(It.IsAny<Expression<Func<Ue, bool>>>())).ReturnsAsync(reponseFindByCondition);
        
        // Simulation de la fonction Create
        // On lui dit que l'ajout d'un étudiant renvoie un étudiant avec l'Id 1
        Ue ueCree =new Ue{NumeroUe=numeroUe, Intitule = intitule};
        mock.Setup(repoUe=>repoUe.CreateAsync(UniteEnseignementSansId)).ReturnsAsync(ueCree);
        
        // On crée le bouchon (un faux etudiantRepository). Il est prêt à être utilisé
        var fauxUeRepository = mock.Object;
        
        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory.Setup(facto=>facto.UeRepository()).Returns(fauxUeRepository);
        
        // Création du use case en injectant notre faux repository
        CreateUeUseCase useCase=new CreateUeUseCase(mockFactory.Object);
        // Appel du use case
        var ueTeste=await useCase.ExecuteAsync(UniteEnseignementSansId);
        
        // Vérification du résultat
        Assert.That(ueTeste.Id, Is.EqualTo(ueCree.Id));
        Assert.That(ueTeste.NumeroUe, Is.EqualTo(ueCree.NumeroUe));
        Assert.That(ueTeste.Intitule, Is.EqualTo(ueCree.Intitule));
    }
}