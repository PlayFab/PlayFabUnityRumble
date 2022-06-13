namespace PlayFab.Multiplayer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public class ObjectPool
    {
        private static object[] ctorParamList0Element;
        private static object[] ctorParamList1Element;
        private static object[] ctorParamList2Element;
        private Dictionary<Type, Pool> pools;

        static ObjectPool()
        {
            ctorParamList0Element = new object[0];
            ctorParamList1Element = new object[1];
            ctorParamList2Element = new object[2];
        }

        public ObjectPool()
        {
            this.pools = new Dictionary<Type, Pool>();
        }

        public void AddEntry<T>(int maxLimit, Type[] ctorTypes)
        {
            Type t = typeof(T);
            ConstructorInfo ctor = t.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ctorTypes, null);
            if (ctor != null)
            {
                this.pools[t] = new Pool(maxLimit, ctor);
            }
            else
            {
                Debug.WriteLine(string.Format("Failed to get constructor for type {0}!\n", t));
                Debugger.Break();
            }
        }

        public T Retrieve<T>()
        {
            return this.Retrieve<T>(ctorParamList0Element);
        }

        public T Retrieve<T>(object param)
        {
            ctorParamList1Element[0] = param;
            return this.Retrieve<T>(ctorParamList1Element);
        }

        public T Retrieve<T>(object param0, object param1)
        {
            ctorParamList2Element[0] = param0;
            ctorParamList2Element[1] = param1;
            return this.Retrieve<T>(ctorParamList2Element);
        }

        public T Retrieve<T>(object[] ctorParams)
        {
            Type key = typeof(T);
            object result = null;
            if (this.pools.ContainsKey(key))
            {
                Pool pool = this.pools[key];
                ConstructorInfo ctor = pool.Ctor;
                int ct = pool.Objects.Count;
                if (ct > 0)
                {
                    result = pool.Objects[ct - 1];
                    pool.Objects.RemoveAt(ct - 1);
                    ctor.Invoke(result, ctorParams);
                }
                else
                {
                    result = ctor.Invoke(ctorParams);
                }
            }
            else
            {
                Debug.WriteLine(string.Format("Need to add type with AddEntry first!\n"));
                Debugger.Break();
            }

            return (T)result;
        }

        public void Return(object o)
        {
            Type key = o.GetType();
            if (this.pools.ContainsKey(key) && this.pools[key].Objects.Count < this.pools[key].Limit)
            {
                Pool pool = this.pools[key];
                if (pool.Objects.Count < pool.Limit)
                {
                    this.pools[key].Objects.Add(o);
                }
                else
                {
                    Debug.WriteLine(string.Format("Reach pool limit {0} for object {1}!\n", pool.Limit, key.ToString()));
                }
            }
        }

        internal class Pool
        {
            internal Pool(int limit, ConstructorInfo ctor)
            {
                this.Limit = limit;
                this.Ctor = ctor;
                this.Objects = new List<object>();
            }

            internal int Limit { get; set; }

            internal ConstructorInfo Ctor { get; set; }

            internal List<object> Objects { get; set; }
        }
    }
}
