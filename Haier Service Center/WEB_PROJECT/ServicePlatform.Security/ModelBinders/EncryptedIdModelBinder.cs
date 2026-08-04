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

                // If the value is already a plain number, don't decrypt it.
                if (type == typeof(long))
                {
                    if (long.TryParse(value, out long longValue))
                    {
                        bindingContext.Result = ModelBindingResult.Success(longValue);
                    }
                    else
                    {
                        bindingContext.Result = ModelBindingResult.Success(_encryption.DecryptToLong(value));
                    }
                }
                else if (type == typeof(int))
                {
                    if (int.TryParse(value, out int intValue))
                    {
                        bindingContext.Result = ModelBindingResult.Success(intValue);
                    }
                    else
                    {
                        bindingContext.Result = ModelBindingResult.Success(_encryption.DecryptToInt(value));
                    }
                }
                else if (type == typeof(Guid))
                {
                    if (Guid.TryParse(value, out Guid guidValue))
                    {
                        bindingContext.Result = ModelBindingResult.Success(guidValue);
                    }
                    else
                    {
                        bindingContext.Result = ModelBindingResult.Success(_encryption.DecryptToGuid(value));
                    }
                }
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName,
                    $"Invalid encrypted value. {ex.Message}");

                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }


    }
}

