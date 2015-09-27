using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace a7ExtensionMethods
{
    /// <summary>
    /// extension methods related to various collection classes
    /// </summary>
    public static class a7CollectionExtensionMethods
    {
        public static T GetItem<K, T>(this Dictionary<K, T> dict, K key)
        {
            if (key != null && dict.ContainsKey(key))
                return dict[key];
            else
                return default(T);
        }

        public static void Remove<T>(this List<T> l, List<T> toRemove)
        {
            if (toRemove != null && l != null)
            {
                foreach (var tr in toRemove)
                    l.Remove(tr);
            }
        }

        public static bool EqualDictionary<TKey, TValue>(
             this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == second) return true;
            if ((first == null) && (second != null)) return false;
            if ((first != null) && (second == null)) return false;
            if (first.Count != second.Count) return false;

            var comparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in first)
            {
                TValue secondValue;
                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!comparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }

        public static bool EqualList<T>(
                 this IList<T> first, IList<T> second)
        {
            if (first == second) return true;
            if ((first == null) && (second != null)) return false;
            if ((first != null) && (second == null)) return false;
            if (first.Count != second.Count) return false;

            var comparer = EqualityComparer<T>.Default;

            foreach (T firstValue in first)
            {
                var secondValue = second.FirstOrDefault((s) => comparer.Equals(s, firstValue));
                if (firstValue == null)
                    return false;
            }
            return true;
        }

        public static List<string> ToStringList(this List<object> objList)
        {
            var retList = new List<string>();
            if (objList != null)
            {
                foreach (var ol in objList)
                    retList.Add(ol.ToStringAllowsNull());
            }
            return retList;
        }

        public static bool EqualElementsOnToString<T>(
         this IList<T> first, IList<T> second)
        {
            if (first == second) return true;
            if ((first == null) && (second != null)) return false;
            if ((first != null) && (second == null)) return false;
            if (first.Count != second.Count) return false;

            for (int i = 0; i < first.Count; i++)
                if (first[i].ToStringAllowsNull() != second[i].ToStringAllowsNull())
                    return false;
            return true;
        }

        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector)
        {
            List<TSource> sortedList = source.OrderBy(keySelector).ToList();
            source.Clear();
            foreach (var sortedItem in sortedList)
                source.Add(sortedItem);
        }
        /// <summary>
        /// converts a list of string to a string seperated by given seperator, optionally converting each string with given 
        /// converter function
        /// </summary>
        /// <param name="l"></param>
        /// <param name="seperation"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static string ToStringSeperated<T>(this List<T> l, string seperation, Func<T, string> converter = null)
        {
            string sRet = "";
            bool isFirst = true;
            foreach (T s in l)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sRet += seperation;
                if (converter == null)
                    sRet += s.ToStringAllowsNull();
                else
                    sRet += converter(s);
            }
            return sRet;
        }



        public static string ToStringSeperated(this List<string> l, string seperation, string prefix, string suffix, Func<string, string> converter = null)
        {
            string sRet = "";
            bool isFirst = true;
            foreach (string s in l)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sRet += seperation;
                if (converter == null)
                    sRet += prefix + s + suffix;
                else
                    sRet += converter(s);
            }
            return sRet;
        }

        public static void Add<T>(this List<T> l, params T[] pars)
        {
            foreach (var p in pars)
                l.Add(p);
        }

        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> d, Dictionary<TKey, TValue> d2)
        {
            if (d2 != null)
                foreach (var kv in d2)
                {
                    d[kv.Key] = kv.Value;
                }
        }


        public static void AddRange<T>(this ICollection<T> c, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                c.Add(item);
            }
        }

        public static void AddRangeAvoidDuplicates<T>(this ICollection<T> c, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                if (!c.Contains(item))
                {
                    c.Add(item);
                }
            }
        }

        public static void AddAvoidDuplicates<T>(this ICollection<T> c, T item)
        {
            if (!c.Contains(item))
            {
                c.Add(item);
            }
        }


        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> d)
        {
            if (d == null)
                return null;
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> kv in d)
            {
                if (kv.Value is ICloneable)
                    ret.Add(kv.Key, (TValue)(kv.Value as ICloneable).Clone());
                else
                    ret.Add(kv.Key, kv.Value);
            }
            return ret;
        }

        /// <summary>
        /// adds to the List that is the value in this dictionary the given value on the given key in dictionary.
        /// If on the given key doesn't exist on this dictionary, a List is created and then the value added.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddToListInValue<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, TValue value, bool avoidDuplicates = false)
        {
            if (!dict.ContainsKey(key))
                dict[key] = new List<TValue>();
            if (!avoidDuplicates || !dict[key].Contains(value))
                dict[key].Add(value);
        }

        public static void AddToListInValue<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, List<TValue> values, bool avoidDuplicates = false)
        {
            if (!dict.ContainsKey(key))
                dict[key] = new List<TValue>();
            foreach (var value in values)
            {
                if (!avoidDuplicates || !dict[key].Contains(value))
                    dict[key].Add(value);
            }
        }

        /// <summary>
        /// adds to the List that is the value in this dictionary the given value on the given key in dictionary.
        /// If on the given key doesn't exist on this dictionary, a List is created and then the value added.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddToListInValue<T>(this Dictionary<string, List<T>> dict, string key, T value, bool avoidDuplicates = false)
        {
            if (!dict.ContainsKey(key))
                dict[key] = new List<T>();
            if (!avoidDuplicates || !dict[key].Contains(value))
                dict[key].Add(value);
        }

        public static void AddToListInValue<T>(this Dictionary<string, ObservableCollection<T>> dict, string key, T value)
        {
            if (!dict.ContainsKey(key))
                dict[key] = new ObservableCollection<T>();
            dict[key].Add(value);
        }

        /// <summary>
        /// Converts a List of TSource type object to a List of TDestination type object with the given
        /// conversion function (doesn't that exist already trough linq?:))
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="getDestinationListItem"></param>
        /// <returns></returns>
        public static List<TDestination> ConvertList<TDestination, TSource>(this List<TSource> sourceList,
            Func<TSource, TDestination> getDestinationListItem)
        {
            List<TDestination> outList = new List<TDestination>();
            foreach (TSource data in sourceList)
            {
                outList.Add(getDestinationListItem(data));
            }
            return outList;
        }

        /// <summary>
        /// Converts a List of TSource obejct to a Dictionary with TSource values and TKey key type.
        /// the given function returns the key value for each object
        /// (doesn't that exist already trough linqu?:))
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="getKeyValue"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TSource> ConvertToDictionary<TKey, TSource>(this List<TSource> sourceList,
            Func<TSource, TKey> getKeyValue)
        {
            Dictionary<TKey, TSource> outDict = new Dictionary<TKey, TSource>();
            foreach (TSource data in sourceList)
            {
                outDict.Add(getKeyValue(data), data);
            }
            return outDict;
        }

        /// <summary>
        /// Converts a List of TSource obejct to a Dictionary with TDestination values and TKey key type.
        /// the given getKey function returns the key value for each object.
        /// the given converter function converts each TSource item of source list to TDestination value of returned dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="getKeyValue"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TDestination> ConvertToDictionary<TKey, TDestination, TSource>(this List<TSource> sourceList,
            Func<TSource, TKey> getKeyValue,
            Converter<TSource, TDestination> converter)
        {
            Dictionary<TKey, TDestination> outDict = new Dictionary<TKey, TDestination>();
            foreach (TSource data in sourceList)
            {
                outDict.Add(getKeyValue(data), converter(data));
            }
            return outDict;
        }

        /// <summary>
        /// checks if all object in objArr array are of types given in paramTypes array (in same order)
        /// </summary>
        /// <param name="objArr"></param>
        /// <param name="paramTypes"></param>
        /// <returns></returns>
        public static bool Validate(this object[] objArr, params Type[] paramTypes)
        {
            if (objArr.Length < paramTypes.Length)
                return false;
            for (int i = 0; i < objArr.Length; i++)
            {
                if (objArr[i].GetType() != paramTypes[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// returns from given object array first occured object of type T.
        /// optgionally if which set, it returns not first but given in param object
        /// (dunno if used:))
        /// strictly = GetType()==typeof(T)
        /// not strictly = obj is T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objArr"></param>
        /// <param name="which"></param>
        /// <returns></returns>
        public static T GetParam<T>(this object[] objArr, int which = 0, bool strictly = false)
        {
            if (strictly)
            {
                int w = 0;
                foreach (object obj in objArr)
                {
                    if (obj.GetType() == typeof(T) && w == which)
                    {
                        return (T)obj;
                    }
                    else if (obj.GetType() == typeof(T))
                        w++;
                }
            }
            else
            {
                int w = 0;
                foreach (object obj in objArr)
                {
                    if (obj is T && w == which)
                    {
                        return (T)obj;
                    }
                    else if (obj is T)
                        w++;
                }
            }
            return default(T);
        }

        public static bool ContainsString(this object[] arr, string value)
        {
            foreach (var o in arr)
            {
                if (o is string && o.ToUpperTrimmedString() == value.ToUpperTrimmedString())
                    return true;
            }
            return false;
        }

        public static string[] CloneStringArray(this string[] arr)
        {
            if (arr == null)
                return new string[0];
            var ret = new string[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                ret[i] = arr[i];
            }
            return ret;
        }

        public static string ToStringWithElements(this string[] arr, string seperator = ";")
        {
            if (arr == null)
                return "";
            var ret = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i > 0)
                    ret += seperator;
                ret += arr[i];
            }
            return ret;
        }

        public static Dictionary<string, object> ToDictionaryStringKey(this object[] arr)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            string key = null;
            foreach (var o in arr)
            {
                if (key == null)
                {
                    key = o.ToStringAllowsNull();
                }
                else
                {
                    ret.Add(key, o);
                    key = null;
                }
            }
            return ret;
        }
    }
}
