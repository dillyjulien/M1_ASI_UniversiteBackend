namespace UniversiteDomain.Entities;

public class Note
{
    public float Value { get; set; }
    public long IdEtudiant { get; set; }
    public long IdUe { get; set; }
    public Etudiant Etudiant { get; set; }
    public Ue Ue { get; set; }
    
    
    public override string ToString()
    {
        return "Note : "+Value +" de l'étudiant "+IdEtudiant+" et de l'UE "+IdUe;
    }
}