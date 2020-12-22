using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day21
{
    class Food
    {
        public List<string> Ingredients;
        public List<string> Allergens;

        public Food(List<string> ingredients, List<string> allergens)
        {
            Ingredients = ingredients;
            Allergens = allergens;
        }    
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var (ingredients, allergens, food, ingredientCount) = Parse(lines);
            Console.WriteLine(Part1(ingredients, allergens, food, ingredientCount));
            Console.WriteLine(Part2(ingredients, allergens, food));
        }

        static (List<string>, List<string>, List<Food>, Dictionary<string, int>) Parse(string[] lines)
        {
            var ingredients = new List<string>();
            var allergens = new List<string>();
            var food = new List<Food>();
            var ingredientCount = new Dictionary<string, int>();
            for(int i = 0; i < lines.Length; i++)
            {
                var containsInd = lines[i].IndexOf("(contains ");
                var ingr = lines[i].Substring(0, containsInd-1).Split(' ').ToList();
                var all = lines[i].Substring(containsInd+10).Trim(')').Split(", ").ToList();
                foreach(var ing in ingr)
                {
                    if(!ingredients.Contains(ing))
                        ingredients.Add(ing);
                    if(ingredientCount.TryGetValue(ing, out _))
                        ingredientCount[ing]++;
                    else 
                        ingredientCount[ing] = 1;
                }
                allergens.AddRange(all);
                food.Add(new Food(ingr, all));
            }
            allergens = allergens.Distinct().ToList();
            return (ingredients, allergens, food, ingredientCount);
        }

        static bool CantBe(string ingredient, string allergen, List<Food> food)
        {
            return food.Any(f => f.Allergens.Contains(allergen) && !f.Ingredients.Contains(ingredient));
        }

        static int Part1(List<string> ingredients, List<string> allergens, List<Food> food, Dictionary<string, int> ingredientCount)
        {
            int count = 0;
            foreach(var ingredient in ingredients)
            {
                bool cantbe = true;
                foreach(var allergen in allergens)
                {
                    if(!CantBe(ingredient, allergen, food))
                    {
                        cantbe = false;
                        break;
                    }
                }
                if(cantbe)
                    count += ingredientCount[ingredient];
            }
            return count;
        }

        static string Part2(List<string> ingredients, List<string> allergens, List<Food> food)
        {
            List<string> safe = new List<string>();
            foreach(var ingredient in ingredients)
            {
                bool cantbe = true;
                foreach(var allergen in allergens)
                {
                    if(!CantBe(ingredient, allergen, food))
                    {
                        cantbe = false;
                        break;
                    }
                }
                if(cantbe)
                    safe.Add(ingredient);
            }
            foreach(var s in safe)
            {    
                ingredients.Remove(s);
                foreach(var f in food)
                    f.Ingredients.Remove(s);
            }
            var possibilities = new Dictionary<string, List<string>>();
            foreach(var a in allergens)
                possibilities.Add(a, ingredients.ToArray().ToList());
            while(!possibilities.Values.All(x => x.Count == 1))
            {
                foreach(var a in allergens)
                {
                    foreach(var f in food.Where(ff => ff.Allergens.Contains(a)))
                    {
                        List<string> toRemove = new List<string>();
                        foreach(var i in possibilities[a])
                        {
                            if(!f.Ingredients.Contains(i))
                                toRemove.Add(i);
                        }
                        foreach(var r in toRemove)
                            possibilities[a].Remove(r);
                    }
                    if(possibilities[a].Count == 1)
                        foreach(var p in possibilities)
                            if(p.Key != a)
                                p.Value.Remove(possibilities[a].Single());
                }
            }
            return string.Join(",", possibilities.OrderBy(x => x.Key).Select(x => x.Value.Single()));
        }
    }
}
