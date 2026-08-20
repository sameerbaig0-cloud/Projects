using Microsoft.AspNetCore.Mvc;

namespace ServicePlatform.Security.Attributes
{
    public class EncryptedIdAttribute : ModelBinderAttribute
    {
        public EncryptedIdAttribute()
        {
            BinderType = typeof(ModelBinders.EncryptedIdModelBinder);
        }
    }
}

