using System.Reflection;
using Advent;

Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(x => !x.IsAbstract && typeof(AdventDay).IsAssignableFrom(x))
    .Select(x => Activator.CreateInstance(x) as AdventDay)
    .OrderByDescending(x => x!.Day)
    .First()!
    .DoIt();
