
namespace Citywire.App.DataLayer
{
    public interface IRepository<T>
    {
        T GetById(int id);

        bool Create(T obj);
    }
}           