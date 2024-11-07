namespace UniversiteDomain.Exceptions.ParcoursExceptions;

public class EtudiantNotFoundException : Exception
{
    public EtudiantNotFoundException() : base() { }
    public EtudiantNotFoundException(string message) : base(message) { }
    public EtudiantNotFoundException(string message, Exception inner) : base(message, inner) { }
}
