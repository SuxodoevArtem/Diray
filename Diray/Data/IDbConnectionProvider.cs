using System.Data;

namespace Diray.Data
{
    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }
}
