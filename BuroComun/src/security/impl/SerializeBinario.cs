using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroComun.src.model.cargas;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BuroComun.src.security.impl
{
    public class SerializeBinario : ISerialize
    {
        public object Derialize(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        public string Serialize(object v)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, v);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
