using ArqEmCamadas.ApplicationService.Dtos.EmailDtos.Request;

namespace ArqEmCamadas.ApplicationService.Services.EmailServices.Templates;

public static class EmailTemplate
{
  
  // Todos os modelos são ilustrativos (necessário substituir)
    public static string RegisterModel(EmailSenderRequest dto)
    {
       var template = $"""
                        <!DOCTYPE html>
                        <html lang="pt">
                        <head>
                          <meta charset="UTF-8">
                          <title>Bem-vindo</title>
                        </head>
                        <body style="margin:0; font-family: Arial, sans-serif; background:#f4f4f4;">
                         
                          <div style="background:#363a40; color:#fff; display:flex; align-items:center; justify-content:space-between; padding:16px 24px;">
                            <div style="display:flex; align-items:center;">
                              <div>
                                <img src="{dto.ClientSideUrl}/logo.png" width="200px" alt="Partilha Online">
                              </div>
                            </div>
                            <div style="font-size:16px; font-weight:700; letter-spacing:1px;">
                              Partilha Online
                            </div>
                          </div>
                         
                          <div style="max-width:600px; margin:20px auto; background:#fff; border-radius:8px; overflow:hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1);">
                            
                            <div style="padding: 30px; text-align:center;">
                              <h1 style="color:#363a40; margin-bottom:20px; font-size:24px;">Bem-vindo ao Partilha Online!</h1>
                              <p style="color:#666; font-size:16px; line-height:1.5; margin-bottom:30px;">
                                Olá <strong>{dto.UserName}</strong>, é um prazer tê-lo conosco! Seu cadastro foi realizado com sucesso.
                              </p>
                            </div>

                            <div style="background:#f8f9fa; padding:20px; margin:0 30px 30px 30px; border-radius:6px; border-left:4px solid #363a40;">
                              <h3 style="color:#363a40; margin:0 0 15px 0; font-size:18px;">Seus dados de acesso:</h3>
                              <table style="width:100%; border-collapse:collapse;">
                                <tr>
                                  <td style="padding:8px 0; color:#666; font-weight:600; width:80px;">E-mail:</td>
                                  <td style="padding:8px 0; color:#333;">{dto.Email}</td>
                                </tr>
                                <tr>
                                  <td style="padding:8px 0; color:#666; font-weight:600;">Senha:</td>
                                  <td style="padding:8px 0; color:#333;">{dto.Password}</td>
                                </tr>
                              </table>
                            </div>

                            <div style="padding: 0 30px 30px 30px; text-align:center;">
                              <p style="color:#666; font-size:14px; line-height:1.5; margin-bottom:20px;">
                                Recomendamos que você altere sua senha no primeiro acesso por motivos de segurança.
                              </p>
                              <p style="color:#666; font-size:14px; line-height:1.5;">
                                Se você tiver alguma dúvida ou precisar de ajuda, não hesite em entrar em contato conosco.
                              </p>
                            </div>
                          </div>
                          
                          <div style="text-align:center; padding:20px;">
                            <p style="color:#999; font-size:12px; margin:0;">
                              Este e-mail foi enviado automaticamente. Por favor, não responda a este e-mail.
                            </p>
                          </div>
                        </body>
                        </html>
                        """;

        return template;
    }

    public static string UserRegistrationModel(EmailSenderRequest dto)
    {
        var template = $"""
                        <!DOCTYPE html>
                        <html lang="pt">
                        <head>
                          <meta charset="UTF-8">
                          <title>Bem-vindo ao Partilha Online</title>
                        </head>
                        <body style="margin:0; font-family: Arial, sans-serif; background:#f4f4f4;">
                         
                          <div style="background:#363a40; color:#fff; display:flex; align-items:center; justify-content:space-between; padding:16px 24px;">
                            <div style="display:flex; align-items:center;">
                              <div>
                                <img src="{dto.ClientSideUrl}/logo.png" width="200px" alt="Partilha Online">
                              </div>
                            </div>
                            <div style="font-size:16px; font-weight:700; letter-spacing:1px;">
                              Partilha Online
                            </div>
                          </div>
                         
                          <div style="max-width:600px; margin:20px auto; background:#fff; border-radius:8px; overflow:hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1);">
                            
                            <div style="padding: 30px; text-align:center;">
                              <h1 style="color:#363a40; margin-bottom:20px; font-size:24px;">Bem-vindo ao Partilha Online!</h1>
                              <p style="color:#666; font-size:16px; line-height:1.5; margin-bottom:30px;">
                                Olá <strong>{dto.UserName}</strong>, é um prazer tê-lo conosco! Seu cadastro foi realizado com sucesso.
                              </p>
                            </div>

                            <div style="background:#f8f9fa; padding:20px; margin:0 30px 30px 30px; border-radius:6px; border-left:4px solid #363a40;">
                              <h3 style="color:#363a40; margin:0 0 15px 0; font-size:18px;">Seus dados de acesso:</h3>
                              <table style="width:100%; border-collapse:collapse;">
                                <tr>
                                  <td style="padding:8px 0; color:#666; font-weight:600; width:80px;">E-mail:</td>
                                  <td style="padding:8px 0; color:#333;">{dto.Email}</td>
                                </tr>
                                <tr>
                                  <td style="padding:8px 0; color:#666; font-weight:600;">Senha:</td>
                                  <td style="padding:8px 0; color:#333;">{dto.Password}</td>
                                </tr>
                              </table>
                            </div>

                            <div style="padding: 0 30px 30px 30px; text-align:center;">
                              <p style="color:#666; font-size:14px; line-height:1.5; margin-bottom:20px;">
                                Recomendamos que você altere sua senha no primeiro acesso por motivos de segurança.
                              </p>
                              <p style="color:#666; font-size:14px; line-height:1.5;">
                                Se você tiver alguma dúvida ou precisar de ajuda, não hesite em entrar em contato conosco.
                              </p>
                            </div>
                          </div>
                          
                          <div style="text-align:center; padding:20px;">
                            <p style="color:#999; font-size:12px; margin:0;">
                              Este e-mail foi enviado automaticamente. Por favor, não responda a este e-mail.
                            </p>
                          </div>
                        </body>
                        </html>
                        """;

        return template;
    }

    public static string RecoveryPasswordModel(EmailSenderRequest dto)
    {
        var template = $"""
                        <!DOCTYPE html>
                        <html lang="pt">
                        <head>
                          <meta charset="UTF-8">
                          <title>Xbits</title>
                        </head>
                        <body style="margin:0; font-family: Arial, sans-serif; background:#fff;">
                         
                          <div style="background:#363a40; color:#fff; display:flex; align-items:center; justify-content:space-between; padding:16px 24px;">
                            <div style="display:flex; align-items:center;">
                              <div>
                                <img src="{dto.ClientSideUrl}/logo.png" width="200px" alt="">
                              </div>
                            </div>
                            <div style="font-size:16px; font-weight:700; letter-spacing:1px;">
                              Partilha
                            </div>
                          </div>
                         
                          <div style="padding: 20px; border-radius:6px; overflow:hidden; background-color: #fff;">
                            <table style="width:100%; border-collapse:collapse; height: fit-content;">
                              <tr>
                                <td style="background:#E3E5E8; color:#666; font-weight:bold; padding:12px 10px; width: 144px; height: 34px; border: 3px solid #fff;">Data desvio</td>
                                <td style="padding:12px 10px; background-color: #F6F6F6; border: 3px solid #fff;">{dto.Date}</td>
                              </tr>
                              <tr>
                                <td style="background:#E3E5E8; color:#666; font-weight:bold; padding:12px 10px;  border: 3px solid #fff;">Nome</td>
                                <td style="padding:12px 10px; background-color: #F6F6F6; border: 3px solid #fff;">{dto.UserName}</td>
                              </tr>
                              <tr>
                                <td style="background:#E3E5E8; color:#666; font-weight:bold; padding:12px 10px; border: 3px solid #fff;">Descrição do desvio</td>
                                <td style="padding:12px 10px; background-color: #F6F6F6; border: 3px solid #fff;">{dto.Password}</td>
                              </tr>
                            </table>
                          </div>
                          <div style="display:flex; justify-content:flex-end; align-items:center; margin:24px 32px 0 0;">
                            <div style="display:flex; align-items:center;">
                              <img src="{dto.ClientSideUrl}/logo.png" width="100px" alt="">
                            </div>
                          </div>
                        </body>
                        </html>
                        """;

        return template;
    }
}