using System.ComponentModel;

namespace ArqEmCamadas.Domain.Enum;

public enum ETemplateType : byte
{
    [Description("Cadastro de Usuário")]
    RegisterAnalyst = 1,
    
    [Description("Recuperar senha")]
    ResetPassword,
    
    [Description("Cadastro do Contratante")]
    UserRegistration,
}