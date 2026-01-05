using System.Threading.Tasks;

namespace Yucca.Operations;

public interface IYuccaOperation
{
    Task Execute(string[] parameters);
}
