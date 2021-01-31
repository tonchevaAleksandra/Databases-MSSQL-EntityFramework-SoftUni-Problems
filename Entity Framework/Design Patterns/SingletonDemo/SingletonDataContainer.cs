using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SingletonDemo.Interfaces;

namespace SingletonDemo
{
   public class SingletonDataContainer:ISingletonContainer
   {
       private Dictionary<string, int> _capitals = new Dictionary<string, int>();

       public SingletonDataContainer()
       {
           Console.WriteLine("Initializing singleton object");

           var elements = File.ReadLines("capitals.txt").ToArray();

           for (int i = 0; i < elements.Length; i+=2)
           {
               
               this._capitals.Add(elements[i], int.Parse(elements[i+1]));
           }
       }
        public int GetPopulation(string name)
        {
            return _capitals[name];
        }

        private static SingletonDataContainer instance = new SingletonDataContainer();

        public static SingletonDataContainer Instance
        {
            get
            {
                if (instance == null)  //Thread-safe
                {
                    lock (instance)
                    {
                        if (instance == null)
                            instance = new SingletonDataContainer();
                    }
                }

                return instance;
            }
        }
   }
}
