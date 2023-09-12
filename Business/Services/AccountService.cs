using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Enums;

namespace Business.Services
{
    public interface IAccountService // IAccountService'i IService'ten implemente etmiyoruz çünkü bu servis UserService enjeksiyonu üzerinden login ve register iþlerini yapacak,
                                     // CRUD iþlemlerinin hepsini yapmayacak, bu yüzden Login ve Register methodlarýný içerisinde tanýmlýyoruz
    {
        Result Login(AccountLoginModel accountLoginModel, UserModel userResultModel); // kullanýcýlarýn kullanýcý giriþi için accountLoginModel view üzerinden kullanýcýdan aldýðýmýz verilerdir,
                                                                                      // userResultModel ise accountLoginModel'deki doðru verilere göre kullanýcýyý veritabanýndan çektikten sonra method içerisinde atayacaðýmýz ve referans tip olduðu için de Login methodunu çaðýrdýðýmýz yerde kullanabileceðimiz sonuç kullanýcý objesidir,
        Result Register(AccountRegisterModel model);                                  // böylelikle Login methodundan hem login iþlem sonucunu Result olarak hem de iþlem baþarýlýysa kullanýcý objesini UserModel objesi olarak dönebiliyoruz.
    }

    public class AccountService : IAccountService
    {
        private readonly IUserService _userService; // CRUD iþlemlerini yaptýðýmýz UserService objesini bu servise enjekte ediyoruz ki Query methodu üzerinden Login,
                                                    // Add methodu üzerinden de Register iþlemleri yapabilelim
        public AccountService(IUserService userService)
        {
            _userService = userService;
        }

        public Result Login(AccountLoginModel accountLoginModel, UserModel userResultModel) // kullanýcý giriþi
        {
            // önce accountLoginModel üzerinden kullanýcýnýn girmiþ olduðu kullanýcý adý ve þifreye sahip aktif kullanýcý sorgusu üzerinden veriyi çekip user'a atýyoruz,
            // kullanýcý adý ve þifre hassas veri olduðu için trim'lemiyoruz ve büyük küçük harf duyarlýlýðýný da ortadan kaldýrmýyoruz
            var user = _userService.Query().SingleOrDefault(u => u.UserName == accountLoginModel.UserName && u.Password == accountLoginModel.Password && u.IsActive);

            if (user is null) // eðer böyle bir kullanýcý bulunamadýysa
                return new ErrorResult("Invalid user name or password!"); // kullanýcý adý veya þifre hatalý sonucunu dönüyoruz

            // burada kullanýcý bulunmuþ demektir dolayýsýyla referans tip olduðu için userResultModel'i yukarýda çektiðimiz user'a göre dolduruyoruz,
            // dolayýsýyla hem sorgulanan kullanýcý objesi (userResultModel) hem de iþlem sonucunu SuccessResult objesi olarak methoddan dönüyoruz,
            // Account area -> Users controller -> Login action'ýnda sadece kullanýcý adý ve role ihtiyacýmýz olduðu için objemizi bu özellikler üzerinden dolduruyoruz
            userResultModel.UserName = user.UserName;
            userResultModel.Role.Name = user.Role.Name;
            userResultModel.Id = user.Id;

            return new SuccessResult();
        }

        public Result Register(AccountRegisterModel model)
        {
            // CAGIL -> cagýl -> cagil
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
