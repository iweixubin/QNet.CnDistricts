using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace QNet.CnDistricts
{
	public partial class Handler
	{
		private static readonly JsonSerializerOptions _JsonSerializerOptions;
		static Handler()
		{
			_JsonSerializerOptions = new JsonSerializerOptions();
			_JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);
		}

		private static bool IsLastModifiedExpires(HttpContext context, int expires = 60)
		{
			var headerValue = context.Request.Headers["If-Modified-Since"];
			if (!StringValues.IsNullOrEmpty(headerValue))
			{
				DateTime modifiedSince;

				if (DateTime.TryParse(headerValue.ToString(), out modifiedSince))
				{
					return modifiedSince.ToLocalTime().AddSeconds(expires) < DateTime.Now;
				}
			}

			return true;

		}
	}
}
