using domain.Entity;

namespace domain.Interfaces
{
    public interface IParametreRepository
    {
        Task<Parametre> GetParam();
        Task<Parametre> Updatepram(Parametre entity);

    }
}
