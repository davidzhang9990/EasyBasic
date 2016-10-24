using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Helper
{
    public static class SerializationExtensions
    {
        /// <summary>
        /// Serializes the specified obj to XML.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A string representing serialized data</returns>
        public static string SerializeToXml(this object obj)
        {
            //Check is object is serializable before trying to serialize
            if (obj.GetType().IsSerializable)
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(stream, obj);
                    var bytes = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(bytes, 0, bytes.Length);

                    return Encoding.UTF8.GetString(bytes);
                }
            }
            throw new NotSupportedException(string.Format("{0} is not serializable.", obj.GetType()));
        }

        /// <summary>
        /// Deserializes the specified serialized XML data.
        /// </summary>
        /// <param name="serializedData">The serialized data.</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(this string serializedData)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new XmlTextReader(new StringReader(serializedData));

            return (T)serializer.Deserialize(reader);
        }

        //    public static string SerializeToJson(this object obj)
        //    {

        //        //return JsonConvert.SerializeObject(obj);
        //        return "";

        //    }

        //    public static T DeserializeFromJson<T>(this string serializeData)
        //    {

        //        //return JsonConvert.DeserializeObject<T>(serializeData);
        //        return null;
        //    }
    }
}
