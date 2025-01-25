namespace UniversiteDomain.Exceptions.EtudiantExceptions;

public class EtudiantNonInscritException : Exception
{
    public EtudiantNonInscritException() : base() { }
    public EtudiantNonInscritException(string message) : base(message) { }
    public EtudiantNonInscritException(string message, Exception inner) : base(message, inner) { }
}