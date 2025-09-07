namespace ArqEmCamadas.Domain.Extensions;

public static class PersistenceExtension
{
    public static bool CheckIfThereIsAnyProperty<T>(this T? entity) where T : class
    {
        if(entity is null) return false;

        var properties = typeof(T).GetProperties();

        foreach(var property in properties)
        {
            if(property.GetValue(entity) is not null)
            {
                return true;
            }
        }

        return false;
    }
    
    public static bool CheckIsNull<T>(this T? entity) where T : class
    {
        if(entity is null) return false;
    
        var properties = typeof(T).GetProperties();
        
        var results = (
            from property in properties 
            where property.GetValue(entity) is null select true).ToList();

        return results.Count >= 4;
    }
}