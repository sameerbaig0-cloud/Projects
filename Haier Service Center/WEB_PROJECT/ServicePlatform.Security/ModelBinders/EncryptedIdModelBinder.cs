using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServicePlatform.Security.Encryption;
using System;
using System.Threading.Tasks;

namespace ServicePlatform.Security.ModelBinders
{
    public class EncryptedIdModelBinder : IModelBinder
    {
        private readonly IUrlEncryptionService _encryption;

        public EncryptedIdModelBinder(IUrlEncryptionService encryption)
        {
            _encryption = encryption;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var value = bindingContext.ValueProvider
                                      .GetValue(bindingContext.ModelName)
                                      .FirstValue;

            if (string.IsNullOrWhiteSpace(value))
                return Task.CompletedTask;

            try
            {
                var type = Nullable.GetUnderlyingType(bindingContext.ModelType)
                           ?? bindingContext.ModelType;

                if (type == typeof(long))
                {
                    bindingContext.Result =
                        ModelBindingResult.Success(_encryption.DecryptToLong(value));
                }
                else if (type == typeof(int))
                {
                    bindingContext.Result =
                        ModelBindingResult.Success(_encryption.DecryptToInt(value));
                }
                else if (type == typeof(Guid))
                {
                    bindingContext.Result =
                        ModelBindingResult.Success(_encryption.DecryptToGuid(value));
                }
            }
            catch
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }


    }
}

