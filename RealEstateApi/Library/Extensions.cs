using Newtonsoft.Json;

namespace RealEstateApi.Library
{
    public static class Extensions
    {
        public static T MapObject<T>(this object item)
        {
            T? sr = default;
            if (item != null)
            {
                var obj = JsonConvert.SerializeObject(item);
                sr = JsonConvert.DeserializeObject<T>(obj);
            }
            return sr;
        }
    }
}
