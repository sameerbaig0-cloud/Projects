using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace ServicePlatform.Security.ModelBinders
{
    public class EncryptedIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var type = Nullable.GetUnderlyingType(context.Metadata.ModelType)
                       ?? context.Metadata.ModelType;

            if (type == typeof(long) ||
                type == typeof(int) ||
                type == typeof(Guid))
            {
                return new BinderTypeModelBinder(typeof(EncryptedIdModelBinder));
            }

            return null;
        }

    }
}