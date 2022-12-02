// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Advent;

Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(x => x.IsClass && !x.IsAbstract && typeof(AdventDay).IsAssignableFrom(x))
    .Select(x => Activator.CreateInstance(x) as AdventDay)
    .Where(x => x!.Day == DateTime.Now.Day)
    .Single()!
    .DoIt();
