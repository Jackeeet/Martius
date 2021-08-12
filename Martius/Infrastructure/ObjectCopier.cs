using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Martius.Infrastructure
{
    public static class ObjectCopier
    {
        // source: https://stackoverflow.com/questions/16696448/how-to-make-a-copy-of-an-object-in-c-sharp
        public static T DeepCopy<T>(T obj)
        {
            using var memoryStream = new MemoryStream();
            var formatter = new BinaryFormatter {Context = new StreamingContext(StreamingContextStates.Clone)};
            formatter.Serialize(memoryStream, obj);
                
            memoryStream.Position = 0;
            return (T) formatter.Deserialize(memoryStream);
        }
    }
}