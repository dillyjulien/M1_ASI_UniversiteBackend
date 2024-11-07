namespace UniversiteDomain.Exceptions.ParcoursExceptions;

public class InvalidAnneFormationParcoursException : Exception
{
    public InvalidAnneFormationParcoursException() : base() { }
    public InvalidAnneFormationParcoursException(string message) : base(message) { }
    public InvalidAnneFormationParcoursException(string message, Exception inner) : base(message, inner) { }
}
