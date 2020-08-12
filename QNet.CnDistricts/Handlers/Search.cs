using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace QNet.CnDistricts
{
	public partial class Handler
	{
		public static async Task Search(HttpContext context)
		{
			if (!IsLastModifiedExpires(context))
			{
				context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
				return;
			}


			object jsonData = null;
			if (context.Request.Query.Count == 0)
			{
				// 返回所有
				jsonData = Data.GetAll();
			}
			else
			{
				StringValues p;
				if (context.Request.Query.TryGetValue("pid", out p))
				{
					if (!StringValues.IsNullOrEmpty(p))
					{
						long pid;
						if (long.TryParse(p.ToString(), out pid))
						{
							if (pid > -1)
								jsonData = Data.GetByPid(pid);
						}
					}
				}
				else if (context.Request.Query.TryGetValue("id", out p))
				{
					if (!StringValues.IsNullOrEmpty(p))
					{
						var ids = new List<long>();
						foreach (var item in p.ToString().Split(","))
						{
							long id;
							if (long.TryParse(item, out id))
							{
								if (id > 0)
									ids.Add(id);
							}
						}

						if (ids.Any())
						{
							jsonData = Data.GetById(ids);
						}
					}
				}
			}

			context.Response.ContentType = "application/json;";
			context.Response.Headers.Add("Last-Modified", DateTime.Now.ToUniversalTime().ToString("R"));
			await context.Response.BodyWriter.WriteAsync(JsonSerializer.SerializeToUtf8Bytes(jsonData, _JsonSerializerOptions));
		}
	}
}
