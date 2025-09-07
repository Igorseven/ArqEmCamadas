using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.EmailDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.ApplicationService.Interfaces.MapperContracts;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.ApplicationService.Projections;
using ArqEmCamadas.ApplicationService.Traces;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using ArqEmCamadas.Domain.Interfaces;
using ArqEmCamadas.Domain.UserPolicies;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArqEmCamadas.ApplicationService.Services.UserServices;

public sealed class UserCommandService(
    INotificationHandler notification,
    IValidate<User> validate,
    ILoggerHandler logger,
    IUserRepository userRepository,
    IUserMapper userMapper,
    IRoleFacadeQueryService roleFacadeQueryService,
    IEmailCommandService emailCommandService)
    : ServiceBase<User>(notification, validate, logger), IUserCommandService, IUserFacadeCommandService
{
    private const string EntityName = "Usuário";

    public async Task<bool> RegisterAsync(
        UserRegisterRequest dtoRegister,
        UserCredential userCredential)
    {
        if (await CheckUserEmail(dtoRegister.Email))
            return Notification.CreateNotification(
                UserTrace.Save,
                EMessage.Exist.GetDescription().FormatTo(EntityName));

        var role = await roleFacadeQueryService.FindRoleByPredicateAsync(
            r => r.Name == Policy.Analyst,
            RoleProjection.RoleNameProjection(),
            true);

        var user = userMapper.DtoRegisterToDomain(dtoRegister, role!.Id);

        if (!await EntityValidationAsync(user))
            return false;

        var password = user.PasswordHash;

        var result = await userRepository.SaveAsync(user);

        if (!result.Succeeded)
            return Notification.CreateNotifications(
                result.SetNotificationByIdentityResult(UserTrace.Save));
        
        //TODO: enviar e-mail quando cadastrado
        await emailCommandService.SendEmailAsync(CreateEmailData(user, password!), ETemplateType.UserRegistration);

        GenerateLogger(UserTrace.Save, userCredential.Id, user.Id.ToString());

        return result.Succeeded;
    }

    public async Task<bool> RegisterFacadeASync(UserFacadeRegisterRequest dtoFacadeRegister)
    {
        var user = userMapper.DtoFacadeRegisterToDomain(dtoFacadeRegister);
        
        var password = user.PasswordHash;

        var result = await userRepository.SaveAsync(user);
        
        if (!result.Succeeded)
            return Notification.CreateNotifications(
                result.SetNotificationByIdentityResult(UserTrace.Save));
        
        await emailCommandService.SendEmailAsync(CreateEmailData(user, password!), ETemplateType.UserRegistration);

        return result.Succeeded;
    }

    public async Task<bool> UpdateAsync(
        UserUpdateRequest updateRequest,
        UserCredential userCredential)
    {
        var user = await userRepository.FindByPredicateAsync(u => u.Id == updateRequest.Id);

        if (user is null)
            return Notification.CreateNotification(
                UserTrace.Update,
                EMessage.NotFound.Description().FormatTo(EntityName));

        var updated = userMapper.DtoUpdateToDomain(user, updateRequest);

        if (!await EntityValidationAsync(updated))
            return false;

        var result = await userRepository.UpdateAsync(updated);

        if (result.Succeeded)
            GenerateLogger(UserTrace.Update, userCredential.Id, updated.Id.ToString());

        return result == IdentityResult.Success ||
               Notification.CreateNotification("Erro inesperado", EMessage.UnexpectedError.Description());
    }

    public async Task<bool> ActivateOrDeactivateAsync(
        Guid userId, 
        UserCredential userCredential)
    {
        var user = await userRepository.FindByPredicateAsync(u => u.Id == userId);

        if (user is null)
            return Notification.CreateNotification(
                UserTrace.ActivateOrDeactivate,
                EMessage.NotFound.GetDescription().FormatTo(EntityName));

        var status = user.Status is EUserStatus.Active
            ? EUserStatus.Inactive
            : EUserStatus.Active;

        var result = await userRepository.ActivateOrDeactivateAsync(userId, status);

        if (result)
            GenerateLogger(UserTrace.ActivateOrDeactivate, userCredential.Id, userId.ToString());

        return result ||
               Notification.CreateNotification("Erro inesperado", EMessage.UnexpectedError.Description());
    }

    public async Task<bool> ChangePasswordAsync(
        UserChangePasswordRequest dtoChangePassword,
        UserCredential userCredential)
    {
        var user = await userRepository.FindByPredicateAsync(u => u.Id == dtoChangePassword.UserId);

        if (user is null)
            return Notification.CreateNotification(
                UserTrace.ChangePassword,
                EMessage.NotFound.GetDescription().FormatTo(EntityName));

        if (!dtoChangePassword.NewPassword.ValidatePassword())
        {
            return Notification.CreateNotification(
                UserTrace.ChangePassword,
                UserTrace.InvalidPassword);
        }

        var repositoryResult = await userRepository.PasswordRecoveryAsync(
            user,
            dtoChangePassword.NewPassword);

        if (!repositoryResult.Succeeded)
            return Notification.CreateNotifications(repositoryResult.SetNotificationByIdentityResult());

        GenerateLogger(UserTrace.ChangePassword, userCredential.Id, user.Id.ToString());

        return repositoryResult.Succeeded;
    }

    //TODO: implementar recuperar senha.
    public Task ResetPasswordAsync(ChangePasswordRequest changePassword)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(
        Guid analystId,
        UserCredential userCredential)
    {
        var user = await userRepository.FindByPredicateAsync(
            u => u.Id == analystId,
            i => i.Include(u => u.UserRoles)!,
            true);

        if (user is null)
            return Notification.CreateNotification(
                UserTrace.Delete,
                EMessage.NotFound.GetDescription().FormatTo(EntityName));

        var role = await roleFacadeQueryService.FindRoleByPredicateAsync(
            r => r.Name == Policy.Owner,
            RoleProjection.RoleNameProjection(),
            true);

        if (user.UserRoles!.Any(r => r.RoleId == role!.Id))
            return Notification.CreateNotification(
                UserTrace.Delete,
                EMessage.NoDelete.GetDescription().FormatTo(EntityName));

        var result = await userRepository.DeleteAsync(user);

        if (!result.Succeeded)
            return Notification.CreateNotifications(
                result.SetNotificationByIdentityResult(UserTrace.Delete));

        GenerateLogger(UserTrace.Delete, userCredential.Id, analystId.ToString());

        return result.Succeeded;
    }

    private async Task<bool> CheckUserEmail(
        string login,
        Guid? userId = null)
    {
        if (userId is null)
            return await userRepository.ExistsByPredicateAsync(u => u.UserName == login);

        return await userRepository.ExistsByPredicateAsync(u => u.UserName == login && u.Id != userId);
    }

    private static EmailSenderRequest CreateEmailData(User user, string password) =>
        new()
        {
            Date = DateTime.Now.ToBrazilianFormat(),
            UserName = user.Name,
            Email = user.UserName!,
            Password = password,
            Description = ""
        };
}