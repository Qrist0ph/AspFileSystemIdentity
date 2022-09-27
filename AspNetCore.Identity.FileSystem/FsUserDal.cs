using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AspNetCore.Identity.FileSystem
{
    /// <summary>
    /// A data access layer for the FsUserStore
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FsUserDal<T> where T : IdentityUser
    {

        public class Store<T> where T: IdentityUser
        {
            public Dictionary<string,T> Users { get; } = new Dictionary<string,T>();
            public Dictionary<string,string> Logins { get; } = new Dictionary<string,string>();
        }

        private readonly string _filePath;
        public readonly Store<T> _store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingDir">with trailing slash</param>
        public FsUserDal(string workingDir)
        {
            _filePath = workingDir + @"UserStore.json";
            try
            {
                _store = JsonConvert.DeserializeObject<Store<T>>(File.ReadAllText(_filePath));
            }
            catch (Exception e)
            {
                _store = new Store<T>();
                Persist();
                Console.WriteLine(e);
            }
        }

        public void Persist()
        {
            var store = JsonConvert.SerializeObject(_store);
            File.WriteAllText(_filePath, store);
        }

    }
}
