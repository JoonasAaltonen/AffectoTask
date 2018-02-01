using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace AffectoEnnakkoteht
{
    public class BusinessIdSpecification<TEntity> : ISpecification<TEntity>
    {
        public BusinessIdSpecification()
        {
            PopulateRegexes();
        }

        // In all honesty I have no idea what to do with this readonly enumerable that I was given.
        // Was the idea to have the reasons hardcoded into here and use them from there?
        public IEnumerable<string> ReasonsForDissatisfaction { get; } = new List<string>();

        // Instead I'm using this list to print the results for the user
        public List<string> Dissatisfactions = new List<string>();

        // A dictionary of regexes to check the input and an explanation for each of them
        private Dictionary<string, Regex> _regexes = new Dictionary<string, Regex>();

        /// <summary>
        /// Method to populate the _regexes dictionary, add new checks here with the descriptions that will be show to user if the check fires
        /// </summary>
        private void PopulateRegexes()
        {
            
            _regexes.Add("Includes letters", new Regex(@"([A-Z]|[a-z])"));
            _regexes.Add("No dash", new Regex(@"[0-9]{7}[^-]\w+"));
            _regexes.Add("Too few digits before dash character", new Regex(@"(^[0-9]{1,6}-)"));
            _regexes.Add("Too many digits after dash", new Regex(@"(-[0-9]{2,})"));

        }

        public bool IsSatisfiedBy(TEntity entity)
        {
            try
            {
                Dissatisfactions =  CheckInvalidId(entity as string);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            if (Dissatisfactions == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method that checks the user entered ID against regexes, returns a list of reasons why it did not qualify or a null list if input is acceptable
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> CheckInvalidId(string id)
        {
            List<string> reasons = new List<string>();

            // Check length
            if (id.Length > 9)
            {
                reasons.Add("ID too long");
            }
            if (id.Length < 9)
            {
                reasons.Add("ID too short");
            }

            // Check trough the regexes in dictionary
            foreach (var regex in _regexes)
            {
                MatchCollection matchesIncorrect = regex.Value.Matches(id);
                if (matchesIncorrect.Count > 0)
                {
                    foreach (Match match in matchesIncorrect)
                    {
                        // If the same thing has been matched, for example letters, do not add again
                        if (!reasons.Contains(regex.Key))
                        {
                            reasons.Add(regex.Key);
                        }
                    }
                }
            }
            // Check against the correct form
            MatchCollection matchesCorrect = new Regex(@"([0-9]{7}-[0-9]{1})$").Matches(id);
            if (matchesCorrect.Count > 0)
            {
                return null;    // Return null list if correct
            }
            else if (reasons.Count == 0)
            {
                reasons.Add("ID not recognized against any checks");
            }
            return reasons;
        }
    }
}