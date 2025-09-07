using System.ComponentModel;

namespace ArqEmCamadas.Domain.UserPolicies;

public enum ERole : byte
{
    [Description("Administrador")]
    Administrator = 1,
    
    [Description("Dono")]
    Owner,
    
    [Description("Analista")]
    Analyst
}