namespace DicaNinja.API.Helpers;

public static class TextConstant
{
    public static string ProblemToSaveRecord { get; private set; } = "Ocorreu um problema ao salvar o registro";

    public static string ForbiddenUser { get; private set; } = "Usuário sem permissão";


    public static string WrongUserOrInvalidPassword { get; private set; } =
        "O usuário não foi encontrado ou a senha é inválida";

    public static string PasswordDoesntMatch { get; private set; } = "As senhas não estão iguais";

    public static string ExistingEmail { get; private set; } = "Este e-mail já está cadastrado";
    public static string ExistingUsername { get; private set; } = "Este login já está cadastrado";
    public static string RefreshTokenNull { get; private set; } = "Token não informado";
    public static string UsernameNull { get; private set; } = "Nome de usuário não informado";

    public static string RefreshTokenNotFound { get; private set; } =
        "Refresh token não encontrado. Logue se novamente, por favor.";

    public static string RefreshTokenInvalid { get; private set; } = "Refresh token inválido";

    public static string PasswordRecoveryInvalidCode { get; private set; } =
        "O código informado é inválido, já foi utilizado ou o email não existe";

    public static string UserNotFound { get; private set; } = "Usuário não encontrado";
}
