using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class MaxFileSizeListAttribute : ValidationAttribute
{
	private readonly long _maxFileSize;

	public MaxFileSizeListAttribute(long maxFileSize)
	{
		_maxFileSize = maxFileSize;
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is List<IFormFile> files)
		{
			foreach (var file in files)
			{
				if (file.Length > _maxFileSize)
				{
					return new ValidationResult($"The file size must not exceed {_maxFileSize / (1024 * 1024)} MB.");
				}
			}
		}

		return ValidationResult.Success;
	}
}


public class AllowedExtensionsAttribute : ValidationAttribute
{
	private readonly string[] _extensions;
	public AllowedExtensionsAttribute(string[] extensions)
	{
		_extensions = extensions;
	}

	protected override ValidationResult IsValid(
	object value, ValidationContext validationContext)
	{
		var file = value as IFormFile;
		if (file != null)
		{
			var extension = Path.GetExtension(file.FileName);
			if (!_extensions.Contains(extension.ToLower()))
			{
				return new ValidationResult(GetErrorMessage());
			}
		}

		return ValidationResult.Success;
	}

	public string GetErrorMessage()
	{
		return $"This photo extension is not allowed!";
	}
}
