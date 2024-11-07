namespace UniversiteDomain.Exceptions.ParcoursExceptions;

public class DuplicateNumParcoursException : Exception
{
    public DuplicateNumParcoursException() : base() { }
    public DuplicateNumParcoursException(string message) : base(message) { }
    public DuplicateNumParcoursException(string message, Exception inner) : base(message, inner) { }
}
