#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserModel : RecordBase
    {
        #region Entity'den Kopyalanan �zellikler
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }
        #endregion



        #region View'larda G�sterim veya Veri Giri�i i�in Kullanaca��m�z �zellikler
        public RoleModel Role { get; set; }
        #endregion



        public UserModel(string userName, string password, bool isActive, int roleId, int id)
        {
            UserName = userName;
            Password = password;
            IsActive = isActive;
            RoleId = roleId;
            Id = id;

            Role = new RoleModel();
        }

        public UserModel()
        {
            Role = new RoleModel();
        }
    }
}

