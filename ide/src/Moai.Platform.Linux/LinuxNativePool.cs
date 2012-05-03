using System;
using System.Collections.Generic;

namespace Moai.Platform.Linux
{
    /// <summary>
    /// A pooling infrastructure to store native objects
    /// so that they are never collected by garbage collection
    /// until explicitly released.
    /// </summary>
    public class LinuxNativePool
    {
        #region Static Components

        private static LinuxNativePool m_Instance = new LinuxNativePool();
        public static LinuxNativePool Instance
        {
            get { return LinuxNativePool.m_Instance; }
        }

        #endregion

        private Dictionary<string, object> m_Pool = new Dictionary<string, object>();

        private LinuxNativePool()
        {
        }

        public object this[string key]
        {
            get
            {
                return this.m_Pool[key];
            }
        }

        public string Retain(object obj)
        {
            string id = Guid.NewGuid().ToString();
            this.m_Pool.Add(id, obj);
            return id;
        }

        public void Release(string id)
        {
            this.m_Pool.Remove(id);
        }
    }
}

