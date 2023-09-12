#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class AccountLoginModel // login (uygulamaya giriþ) view'ý için herhangi bir entity üzerinden oluþturmadýðýmýz model
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
