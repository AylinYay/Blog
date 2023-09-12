using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Enums;

namespace Business.Services
{
    public interface IAccountService // IAccountService'i IService'ten implemente etmiyoruz ��nk� bu servis UserService enjeksiyonu �zerinden login ve register i�lerini yapacak,
                                     // CRUD i�lemlerinin hepsini yapmayacak, bu y�zden Login ve Register methodlar�n� i�erisinde tan�ml�yoruz
    {
        Result Login(AccountLoginModel accountLoginModel, UserModel userResultModel); // kullan�c�lar�n kullan�c� giri�i i�in accountLoginModel view �zerinden kullan�c�dan ald���m�z verilerdir,
                                                                                      // userResultModel ise accountLoginModel'deki do�ru verilere g�re kullan�c�y� veritaban�ndan �ektikten sonra method i�erisinde atayaca��m�z ve referans tip oldu�u i�in de Login methodunu �a��rd���m�z yerde kullanabilece�imiz sonu� kullan�c� objesidir,
        Result Register(AccountRegisterModel model);                                  // b�ylelikle Login methodundan hem login i�lem sonucunu Result olarak hem de i�lem ba�ar�l�ysa kullan�c� objesini UserModel objesi olarak d�nebiliyoruz.
    }

    public class AccountService : IAccountService
    {
        private readonly IUserService _userService; // CRUD i�lemlerini yapt���m�z UserService objesini bu servise enjekte ediyoruz ki Query methodu �zerinden Login,
                                                    // Add methodu �zerinden de Register i�lemleri yapabilelim
        public AccountService(IUserService userService)
        {
            _userService = userService;
        }

        public Result Login(AccountLoginModel accountLoginModel, UserModel userResultModel) // kullan�c� giri�i
        {
            // �nce accountLoginModel �zerinden kullan�c�n�n girmi� oldu�u kullan�c� ad� ve �ifreye sahip aktif kullan�c� sorgusu �zerinden veriyi �ekip user'a at�yoruz,
            // kullan�c� ad� ve �ifre hassas veri oldu�u i�in trim'lemiyoruz ve b�y�k k���k harf duyarl�l���n� da ortadan kald�rm�yoruz
            var user = _userService.Query().SingleOrDefault(u => u.UserName == accountLoginModel.UserName && u.Password == accountLoginModel.Password && u.IsActive);

            if (user is null) // e�er b�yle bir kullan�c� bulunamad�ysa
                return new ErrorResult("Invalid user name or password!"); // kullan�c� ad� veya �ifre hatal� sonucunu d�n�yoruz

            // burada kullan�c� bulunmu� demektir dolay�s�yla referans tip oldu�u i�in userResultModel'i yukar�da �ekti�imiz user'a g�re dolduruyoruz,
            // dolay�s�yla hem sorgulanan kullan�c� objesi (userResultModel) hem de i�lem sonucunu SuccessResult objesi olarak methoddan d�n�yoruz,
            // Account area -> Users controller -> Login action'�nda sadece kullan�c� ad� ve role ihtiyac�m�z oldu�u i�in objemizi bu �zellikler �zerinden dolduruyoruz
            userResultModel.UserName = user.UserName;
            userResultModel.Role.Name = user.Role.Name;
            userResultModel.Id = user.Id;

            return new SuccessResult();
        }

        public Result Register(AccountRegisterModel model)
        {
            // CAGIL -> cag�l -> cagil
            //List<UserModel> users = _userService.Query().ToList();

            //if (users.Any(u => u.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase)))
            //    return new ErrorResult("User with the same user name exists!");

            UserModel userModel = new UserModel()
            {
                IsActive = true,
                Password = model.Password,
                RoleId = (int)Roles.User,
                UserName = model.UserName
            };

            return _userService.Add(userModel);
        }
    }
}
