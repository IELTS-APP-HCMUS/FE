//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Diagnostics;

//namespace login_full.Converters
//{
//	public class DetailConverter : JsonConverter<Dictionary<string, List<AnswerDetail>>>
//	{
//		public override Dictionary<string, List<AnswerDetail>> ReadJson(JsonReader reader, Type objectType, Dictionary<string, List<AnswerDetail>> existingValue, bool hasExistingValue, JsonSerializer serializer)
//		{
//			var result = new Dictionary<string, List<AnswerDetail>>();
//			var jObject = JObject.Load(reader);

//			Debug.WriteLine("Starting JSON deserialization in DetailConverter.");

//			foreach (var property in jObject.Properties())
//			{
//				var key = property.Name;
//				Debug.WriteLine($"Processing property: {key}");

//				var value = property.Value.ToObject<List<AnswerDetail>>(serializer);
//				if (value != null)
//				{
//					Debug.WriteLine($"Successfully deserialized list for key: {key} with {value.Count} items.");
//					result.Add(key, value);
//				}
//				else
//				{
//					Debug.WriteLine($"Failed to deserialize list for key: {key}. Value is null.");
//				}
//			}

//			Debug.WriteLine("Completed JSON deserialization in DetailConverter.");
//			return result;
//		}


//		public override void WriteJson(JsonWriter writer, Dictionary<string, List<AnswerDetail>> value, JsonSerializer serializer)
//		{
//			throw new NotImplementedException();
//		}
//	}
//}
