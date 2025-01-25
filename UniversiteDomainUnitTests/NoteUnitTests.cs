using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.UseCases.NoteUsseCases.Create;
using UniversiteDomain.UseCases.UeUseCases;
using UniversiteDomain.UseCases.UeUseCases.Create;

namespace UniversiteDomainUnitTests;

public class NoteUnitTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public async Task CreateNoteUseCase()
    {
        long idetud = 1;
        long idue = 1;
        double value = 14.5;
        
        // On crée l'UE qui doit être ajouté en base
        Note testNote = new Note{Value = value,IdEtudiant = idetud, IdUe = idue};
        
        Ue ue = new Ue{Id = 1, NumeroUe = "UE1", Intitule = "UE1"};
        var mockUe = new Mock<IUeRepository>();
        mockUe.Setup(repo => repo.FindAsync(It.IsAny<long>())).ReturnsAsync(ue);
        
        Parcours parcours = new Parcours{Id = 1, NomParcours = "UE1", UesEnseignees = new List<Ue>{ue}};
        
        Etudiant etudiant = new Etudiant{Id = 1,Nom = "Dilly", Prenom = "Julien", Email = "dilly@gmail.com", NumEtud = "et1", ParcoursSuivi = parcours};
        var mockEtudiant = new Mock<IEtudiantRepository>();
        mockEtudiant.Setup(repo => repo.FindAsync(It.IsAny<long>())).ReturnsAsync(etudiant);
        
        
        //  Créons le mock du repository
        // On initialise une fausse datasource qui va simuler un EtudiantRepository
        var mocknote = new Mock<INoteRepository>();
        
        // Il faut ensuite aller dans le use case pour voir quelles fonctions simuler
        // Nous devons simuler FindByCondition et Create
        
        // Simulation de la fonction FindByCondition
        // On dit à ce mock que la note n'existe pas déjà
        // La réponse à l'appel FindByCondition est donc une liste vide
        var reponseFindByCondition = new List<Note>();
        
        // On crée un bouchon dans le mock pour la fonction FindByCondition
        // Quelque soit le paramètre de la fonction FindByCondition, on renvoie la liste vide
        mocknote.Setup(repo=>repo.FindByConditionAsync(It.IsAny<Expression<Func<Note, bool>>>())).ReturnsAsync(reponseFindByCondition);
        
        // Simulation de la fonction Create
        // On lui dit que l'ajout d'une note renvoie une note avec IdEtudiant = 1 et IdUe = 1
        Note noteCree =new Note{Value = value,IdEtudiant = idetud, IdUe = idue};
        mocknote.Setup(repoUe=>repoUe.CreateAsync(testNote)).ReturnsAsync(noteCree);
        
        var mockparcours = new Mock<IParcoursRepository>();
        
        // On crée le bouchon (un faux etudiantRepository). Il est prêt à être utilisé
        var fauxNoteRepository = mocknote.Object;
        
        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory.Setup(facto=>facto.NoteRepository()).Returns(fauxNoteRepository);
        
        // Création du use case en injectant notre faux repository
        CreateNoteUseCase useCase=new CreateNoteUseCase(mockFactory.Object);
        // Appel du use case
        var noteTeste=await useCase.ExecuteAsync(testNote);
        
        // Vérification du résultat
        Assert.That(noteTeste.IdUe, Is.EqualTo(noteCree.IdUe));
        Assert.That(noteTeste.IdEtudiant, Is.EqualTo(noteCree.IdEtudiant));
        Assert.That(noteTeste.Value, Is.EqualTo(noteCree.Value));
    }
}