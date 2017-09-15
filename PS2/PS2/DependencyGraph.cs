// Implemented by Aaron Bellis u0981638 CS3500-006 Fall2017

// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {

        // a dictionary which looks up all dependents of s. If s
        // has no dependents the key s will not be in the dictionary.
        private Dictionary<string, HashSet<string>> dependents;

        // a dictionary which looks up all dependees of t. If s
        // has no dependents the key t will not be in the dictionary.
        private Dictionary<string, HashSet<string>> dependees;

        // Holds the size of this dependency graph
        private int size;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            size = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (dependees.ContainsKey(s))
                {
                    return dependees[s].Count;
                }
                else
                {
                    return 0;
                }
            }

        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            return dependents.ContainsKey(s);
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return dependees.ContainsKey(s);
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if(dependents.ContainsKey(s))
            {
                // per instructions, create copy for this implementation
                return dependents[s].ToList();
            }
            else
            {
                return new HashSet<string>();
            }

        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if(dependees.ContainsKey(s))
            {
                // per instructions, create copy for this implementation
                return dependees[s].ToList();
            }
            else
            {
                return new HashSet<string>();
            }
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>
        public void AddDependency(string s, string t)
        {
            // update dependents and dependees and check if either was changed
            // done this way instead of 
            // dependents.AddKeyAndHashValue(s, t) || dependees.AddKeyAndHashValue(t, s)
            // because dependees.AddKeyAndHashValue(t, s) wouldn't be called if first is true
            bool added;
            added = dependents.AddKeyAndHashValue(s, t);
            added = dependees.AddKeyAndHashValue(t, s) || added;

            if(added)
            {
                size++;
            }

        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            // done this way instead of 
            // dependents.AddKeyAndHashValue(s, t) || dependees.AddKeyAndHashValue(t, s)
            // because dependees.RemoveKeyAndHashValue(t, s) wouldn't be called if first is true
            bool removed;
            removed = dependents.RemoveKeyAndHashValue(s, t);
            removed = dependees.RemoveKeyAndHashValue(t, s) || removed;

            if(removed)
            {
                size--;
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // if s has dependents remove them
            if(dependents.ContainsKey(s))
            {
                string[] oldDependents = dependents[s].ToArray();
                foreach(string r in oldDependents)
                {
                    RemoveDependency(s, r);
                }
            }

            // add new dependents
            foreach(string t in newDependents)
            {
                AddDependency(s, t);
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // if s has dependees remove them
            if(dependees.ContainsKey(s))
            {
                string[] oldDependees = dependees[s].ToArray();
                foreach(string r in oldDependees)
                {
                    RemoveDependency(r, s);
                }
            }

            // add new dependees
            foreach(string t in newDependees)
            {
                AddDependency(t, s);
            }
        }

    }

    internal static class ExtensionMethods
    {
        /// <summary>
        /// Takes two strings, a key and a value. If the key exists, adds the value to its coresponding HashSet.
        /// If the value does not exist, creates a new HashSet which contains the value then creates an entry 
        /// in the dictionary keyed by the parameter key, which looks up the new HashSet.
        /// 
        /// Returns true if the dictionary or underlying values were modified, else returns false
        /// </summary>
        /// <param name="key">The string used to lookup individual Hashsets</param>
        /// <param name="value">A string to be added to a keyed HashSet</param>
        /// <returns>Returns true if the Dictionary, or underlying HashSets were changed, else returns false</returns>
        public static bool AddKeyAndHashValue(this Dictionary<string, HashSet<string>> dict, string key, string value)
        {
            if(dict.ContainsKey(key))
            {
                // this returns true if the hashset was modified
                return dict[key].Add(value);
            }
            else
            {
                dict.Add(key, new HashSet<string> { value });
                // since we are creating everything to add, we know it had to be added
                return true;
            }
        }

        /// <summary>
        /// Takes two strings, a key and a value. If the key exists, removes the value from its corresponding 
        /// HashSet. If the corresponding HashSet becomes empty, the key and the corresponding HashSet are 
        /// removed from the dictionary.
        /// 
        /// Returns true if the dictionary or underlying values were modified, else returns false
        /// </summary>
        /// <param name="key">The string used to lookup individual Hashsets</param>
        /// <param name="value">A string to be removed from a keyed HashSet</param>
        /// <returns>Returns true if the Dictionary, or underlying HashSets were changed, else returns false</returns>
        public static bool RemoveKeyAndHashValue(this Dictionary<string, HashSet<string>> dict, string key, string value)
        {
            if(dict.ContainsKey(key))
            {
                // key was found
                // this returns true if the hashset was modified
                if(dict[key].Remove(value))
                {
                    // if the value was removed, see if we need to remove the key
                    if(dict[key].Count < 1)
                    {
                        dict.Remove(key);
                    }

                    // we have modified the Dictionary or underlying HashSet, return true;
                    return true;
                }
                else
                {
                    // Key was found but the value wasn't in the HashSet to be removed
                    return false;
                }
            }
            else
            {

                // neither the key or the value was found, nothing was changed
                return false;
            }
        }
    }

}