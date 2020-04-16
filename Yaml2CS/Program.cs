using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;

namespace Yaml2CS
{
	public class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 1 | args.Any(i => !File.Exists(i)))
			{
				Console.WriteLine(args.First());
				Console.WriteLine("Invalid File Name.");
				Console.ReadLine();
				return;
			}

			foreach (string file in args)
			{
				IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
				object order = deserializer.Deserialize<object>(new StreamReader(file));
				Console.WriteLine($"Object size: ? TypeName: {order.GetType().Name}");
				foreach (var field in order.GetType().GetFields())
				{
					Console.WriteLine($"Field found :{field.Name}");
				}
			}

			Console.ReadLine();
		}
	}
}