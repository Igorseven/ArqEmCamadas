using System.ComponentModel;

namespace ArqEmCamadas.Domain.Handlers.NotificationSettings;

public enum EMessage
{
    [Description("{0} obrigatório.")]
    Required,

    [Description("Campo {0} permite {1} caracteres.")]
    MoreExpected,

    [Description("{0} não encontrado.")]
    NotFound,

    [Description("Não há registro de dados no sistema.")]
    EmptyList,

    [Description("Selecione uma opção")]
    SelectAnOption,

    [Description("Este {0} já existe na base de dados.")]
    Exist,

    [Description("O {0} deve ser maior que {1}.")]
    ValueExpected,

    [Description("Objeto não configurado")]
    ErrorNotConfigured,

    [Description("{0} no Formato inválido")]
    InvalidFormat,

    [Description("{0} está inativo.")]
    Inactive,

    [Description("Credenciais inválidas")]
    InvalidCredentials,

    [Description("Erro inesperado ocorreu")]
    UnexpectedError,

    [Description("Token Inválido")]
    InvalidToken,

    [Description("Já existe um solicitante principal neste processo")]
    IsMainExist,

    [Description("{0} deve ser {1}")]
    DateExpected,

    [Description("Não é possível deletar {0}")]
    NoDelete,
}