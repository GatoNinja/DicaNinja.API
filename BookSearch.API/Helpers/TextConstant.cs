namespace BookSearch.API.Helpers
{
    public static class TextConstant
    {
        public static string NotFoundItem { get; set; } = "Item não encontrado";

        public static string ProblemToSaveRecord { get; set; } = "Ocorreu um problema ao salvar o registro";

        public static string ProblemToDeleteRecord { get; set; } = "Ocorreu um problema ao excluir o registro";

        public static string ForbiddenUser { get; set; } = "Usuário sem permissão";

        public static string ThereIsNoFile { get; set; } = "Não foram encontrados arquivos para processar";

        public static string WrongUserOrInvalidPassword { get; set; } =
            "O usuário não foi encontrado ou a senha é inválida";

        public static string PasswordDoesntMatch { get; set; } = "As senhas não estão iguais";

        public static string ExistingEmail { get; set; } = "Este e-mail já está cadastrado";
        public static string ExistingUsername { get; set; } = "Este login já está cadastrado";
        public static string RefreshTokenNull { get; set; } = "Token não informado";
        public static string UsernameNull { get; set; } = "Nome de usuário não informado";

        public static string RefreshTokenNotFound { get; set; } =
            "Refresh token não encontrado. Logue se novamente, por favor.";

        public static string RefreshTokenInvalid { get; set; } = "Refresh token inválido";

        public static string PasswordRecoveryInvalidCode { get; set; } =
            "O código informado é inválido, já foi utilizado ou o email não existe";

        public static string UserNotFound { get; set; } = "Usuário não encontrado";
    }
}