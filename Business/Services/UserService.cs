using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess;

namespace Business.Services
{
    public interface IUserService : IService<UserModel>
    {
        // ihtiyaca g�re uygulamam�z�n kullanaca�� servisler baz�nda ihtiya� duyulan method tan�mlar�
        // burada yap�larak bu interface'i implemente eden t�m s�n�flarda bu method tan�m� �zerinden
        // methodun olu�turulmas� sa�lanabilir.

        List<UserModel> GetList();
    }

    public class UserService : IUserService
    {
        private readonly RepoBase<User> _userRepo;

        public UserService(RepoBase<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public Result Add(UserModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _userRepo.Dispose();
        }

        public List<UserModel> GetList()
        {
            return Query().ToList();
        }

        public IQueryable<UserModel> Query()
        {
            return _userRepo.Query().OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName).Select(u => new UserModel()
            {
                Guid = u.Guid,
                UserName = u.UserName,
                Id = u.Id,
                IsActive = u.IsActive,
                Password = u.Password,
                RoleId = u.RoleId
            });
        }

        public Result Update(UserModel model)
        {
            throw new NotImplementedException();
        }
    }
}
