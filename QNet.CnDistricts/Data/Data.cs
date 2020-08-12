using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace QNet.CnDistricts
{
	public static class Data
	{
		private const int MaxLevel = 4; // 最高有多少层级

		// 查询时间复杂度O(1)
		private static readonly Dictionary<long, District> _DicAll = new Dictionary<long, District>(46473);//总条数

		// 缓存父子集关系，方便查询
		private static readonly Dictionary<long, District[]> _DicPSubset = new Dictionary<long, District[]>();

		private static void LoadFromDb()
		{
			var strConn = "server=192.168.86.101;port=3306;database=division_cn; uid=root;pwd=123456;charset=utf8mb4";
			using (var conn = new MySqlConnection(strConn))
			{
				conn.Open();
				var cmd = new MySqlCommand("SELECT * FROM district", conn);
				var dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					var district = new District();
					district.Id = dr.GetInt64(0);
					district.Pid = dr.GetInt64(1);
					district.Name = dr.GetString(2);

					_DicAll.Add(district.Id, district);
				}
			}
		}

		private static void SetDicPSubset(long pid = 0, int level = 1)
		{
			var subDistricts = _DicAll.Values.Where(x => x.Pid == pid).ToArray();

			if (subDistricts.Any())
			{
				_DicPSubset.Add(pid, subDistricts);

				// 最后一层级没有子集，不要跑递归了，数据多，极其浪费时间~
				if (level < MaxLevel)
				{
					foreach (var item in subDistricts)
					{
						SetDicPSubset(item.Id, level + 1);
					}
				}

			}
		}

		public static void Load()
		{
			LoadFromDb();
			SetDicPSubset();
		}

		public static Dictionary<long, District>.ValueCollection GetAll()
		{
			return _DicAll.Values;
		}

		public static District[] GetByPid(long pid)
		{
			District[] districts;

			_DicPSubset.TryGetValue(pid, out districts);

			return districts.ToArray();
		}

		public static List<District> GetById(List<long> ids)
		{
			var districts = new List<District>();
			foreach (var id in ids)
			{
				District v;
				if (_DicAll.TryGetValue(id, out v))
					districts.Add(v);
			}

			return districts;
		}
	}
}
