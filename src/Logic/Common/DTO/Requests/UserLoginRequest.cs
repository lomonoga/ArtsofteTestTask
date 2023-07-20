using System.ComponentModel.DataAnnotations;

namespace Logic.Common.DTO.Requests;

public class UserLoginRequest
{
    [Required(ErrorMessage = "Не указан телефон")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Номер должен состоять из 11 цифр")]
    [RegularExpression(@"^7\d+", 
        ErrorMessage = "Некорректный формат телефона")]
    public string Phone { get; set; }
    public string Password { get; set; }
}