using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QNet.CnDistricts
{
	public partial class Handler
	{
		public static async Task Home(HttpContext context)
		{
			if (!IsLastModifiedExpires(context))
			{
				context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
				return;
			}

			const string html = @"
接口说明


/s
获取所有区域数据


/s?pid=
通过 pid 获取子区域数据
例如，获取所有省：/s?pid=0


/s?id=
通过 id 获取区域数据，可以用 , 分隔
例如，获取指定地区数据：/s?id=44,4403,440305
";
			context.Response.ContentType = "text/plain;charset=UTF-8";
			context.Response.Headers.Add("Last-Modified", DateTime.Now.ToUniversalTime().ToString("R"));
			await context.Response.WriteAsync(html);
		}
	}
}
