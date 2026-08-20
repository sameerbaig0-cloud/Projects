using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ServicePlatform.Security.Encryption;

namespace ServicePlatform.Security.TagHelpers
{
    public class SecureAnchorTagHelper : AnchorTagHelper
    {
        private readonly IUrlEncryptionService _encryption;

        public SecureAnchorTagHelper(
            IHtmlGenerator generator,
            IUrlEncryptionService encryption)
            : base(generator)
        {
            _encryption = encryption;
        }

        public override void Process(Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContext context, 
            Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput output)
        {
            if (RouteValues.TryGetValue("id", out var value))
            {
                if (value != null)
                {
                    RouteValues["id"] = _encryption.Encrypt(value.ToString()!);
                }
            }

            base.Process(context, output);
        }
    }
}

