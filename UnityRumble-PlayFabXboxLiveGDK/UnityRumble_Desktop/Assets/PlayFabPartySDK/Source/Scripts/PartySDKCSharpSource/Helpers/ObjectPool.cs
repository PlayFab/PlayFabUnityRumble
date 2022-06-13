using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace PartyCSharpSDK
{
    public class ObjectPool
    {
        internal class Pool
        {
            internal Int32 limit;
            internal ConstructorInfo ctor;
            internal List<Object> objects;

            internal Pool(Int32 limit, ConstructorInfo ctor)
            {
                this.limit = limit;
                this.ctor = ctor;
                objects = new List<Object>();
            }
        }

        static ObjectPool()
        {
            ctorParamList0Element = new Object[0];
            ctorParamList1Element = new Object[1];
            ctorParamList2Element = new Object[2];
        }

        public ObjectPool()
        {
            pools = new Dictionary<Type, Pool>();
        }

        public void AddEntry<T>(Int32 maxLimit, Type[] ctorTypes)
        {
            Type t = typeof(T);
            ConstructorInfo ctor = t.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ctorTypes, null);
            if (ctor != null)
            {
                pools[t] = new Pool(maxLimit, ctor);
            }
            else
            {
                Debug.WriteLine(String.Format("Failed to get constructor for type {0}!\n", t));
                Debugger.Break();
            }
        }

        public T Retrieve<T>()
        {
            return this.Retrieve<T>(ctorParamList0Element);
        }

        public T Retrieve<T>(Object param)
        {
            ctorParamList1Element[0] = param;
            return this.Retrieve<T>(ctorParamList1Element);
        }

        public T Retrieve<T>(Object param0, Object param1)
        {
            ctorParamList2Element[0] = param0;
            ctorParamList2Element[1] = param1;
            return this.Retrieve<T>(ctorParamList2Element);
        }

        public T Retrieve<T>(Object[] ctorParams)
        {
            Type key = typeof(T);
            Object result = null;
            if (pools.ContainsKey(key))
            {
                Pool pool = pools[key];
                ConstructorInfo ctor = pool.ctor;
                Int32 ct = pool.objects.Count;
                if (ct > 0)
                {
                    result = pool.objects[ct - 1];
                    pool.objects.RemoveAt(ct - 1);
                    ctor.Invoke(result, ctorParams);
                }
                else
                {
                    result = ctor.Invoke(ctorParams);
                }
            }
            else
            {
                Debug.WriteLine(String.Format("Need to add type with AddEntry first!\n"));
                Debugger.Break();
            }

            return (T)result;
        }

        public void Return(Object o)
        {
            Type key = o.GetType();
            if (pools.ContainsKey(key) && pools[key].objects.Count < pools[key].limit)
            {
                Pool pool = pools[key];
                if (pool.objects.Count < pool.limit)
                {
                    pools[key].objects.Add(o);
                }
                else
                {
                    Debug.WriteLine(String.Format("Reach pool limit {0} for object {1}!\n", pool.limit, key.ToString()));
                }
            }
        }

        private static Object[] ctorParamList0Element;
        private static Object[] ctorParamList1Element;
        private static Object[] ctorParamList2Element;
        private Dictionary<Type, Pool> pools;
    }
}
