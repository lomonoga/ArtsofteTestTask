using System.ComponentModel.DataAnnotations;

namespace Api.DTO;

public class UserRequest
{
    [Required(ErrorMessage = "Не указано ФИО")]
    [StringLength(250, ErrorMessage = "ФИО должен быть не больше 250 символов")]
    public string FIO { get; init; }
    
    [Required(ErrorMessage = "Не указан телефон")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Номер должен состоять из 11 цифр")]
    [RegularExpression(@"^7\d+", 
        ErrorMessage = "Некорректный формат телефона")]
    public string Phone { get; init; }
    
    [Required(ErrorMessage = "Не указан email")]
    [StringLength(150, ErrorMessage = "Email должен быть не больше 150 символов")]
    [EmailAddress(ErrorMessage = "Такая электронная почта недействительна")]
    public string Email { get; init; }
    
    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(20, ErrorMessage = "Пароль должно быть меньше или равен 20 символам")]
    public string Password { get; init; }
    
    [Required(ErrorMessage = "Введите подтверждение пароля")]
    [StringLength(20, ErrorMessage = "Подтверждение пароля должно быть меньше или равен 20 символам")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
    public string PasswordConfirm { get; init; }
    
}