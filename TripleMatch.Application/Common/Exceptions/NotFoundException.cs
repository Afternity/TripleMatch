namespace TripleMatch.Application.Common.Exceptions
{
    public class NotFoundException
        : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Объект \"{name}\" ({key}) не найден.") { }
    }
}
