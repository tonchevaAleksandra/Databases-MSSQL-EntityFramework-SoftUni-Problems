using System;
using System.Collections.Generic;
using System.ComponentModel;
using SingletonDemo.Interfaces;

namespace SingletonDemo
{
  public class StartUp
    {
        static void Main(string[] args)
        {
            var db = SingletonDataContainer.Instance;
            Console.WriteLine(db.GetPopulation("Washington, D.C."));
            var db2 = SingletonDataContainer.Instance;
            Console.WriteLine(db2.GetPopulation("London"));

            SomeMethod(SingletonDataContainer.Instance);

            //foreach (var item in GetNumber())
            //{
            //    Console.WriteLine(item);
            //}
        }

        private static void SomeMethod(ISingletonContainer singletonContainer)
        {
            
        }

        private static IEnumerable<int> GetNumber()
        {
            var data = new List<int> {1, 2, 3, 4,5,6};
            for(int i = 0; i < data.Count; i++)
            {
                if (data[i] % 2 == 0)
                    yield return data[i];
            }
        }
    }
}
