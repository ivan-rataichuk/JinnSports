﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using JinnSports.Parser.App.ProxyService.ProxyEntities;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using JinnSports.Parser.App.ProxyService.ProxyEnums;

namespace JinnSports.Parser.App.ProxyService.ProxyRepository
{
    public class ProxyRepository<T> : IProxyRepository<T> where T : IProxyServer
    {
        private XmlSerializer xmlSerializer;
        private static object repLocker = new Object();
        private string path;

        public ProxyRepository()
        {
            this.path = @"..\..\" + ConfigSettings.Xml();
            this.xmlSerializer = new XmlSerializer(typeof(List<ProxyServer>));
            this.Interval = 1;
        }

        public int Interval { get; private set; }

        public void Delete(string ip)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
            }
            proxyCollection.Remove(proxyCollection.FirstOrDefault(x => x.Ip == ip));
            lock (repLocker)
            {
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }
        public void Modify(T proxy)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                int index = proxyCollection.FindIndex(x => x.Ip == proxy.Ip);
                proxyCollection.RemoveAt(index);
                proxyCollection.Insert(index, proxy);
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }
        public void Clear()
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }
        public int Count()
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
            }
            int a = proxyCollection.Count();
            return proxyCollection.Count();
        }
        public void Add(T proxy)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                proxyCollection.Add(proxy);
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }
        public void Add(List<T> proxyList)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                foreach (T proxy in proxyList)
                {
                    proxyCollection.Add(proxy);
                }
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }
        public List<T> GetAll()
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
            }
            return proxyCollection;
        }
        public bool IsAvaliable(T proxy)
        {
            TimeSpan timeDifference = DateTime.Now.TimeOfDay - proxy.LastUsed.TimeOfDay;
            var proxyTimeout = timeDifference.Seconds + (timeDifference.Minutes * 60) + (timeDifference.Hours * 3600) + (timeDifference.Days * 3600 * 24);
            //var proxyTimeout = timeDifference.TotalSeconds().;
            if ((Math.Abs(proxyTimeout) >= this.Interval * 60) && (proxy.IsBusy != true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Contains(string ip)
        {
            List<T> proxyCollection = new List<T>();

            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                if (proxyCollection.Where(x => x.Ip == ip).Count() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
