using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    /// <summary>
    ///   A class that stores all the information necessary to define a load combination.
    /// </summary>
    public class LoadCombo
    {
        #region Properties
        /// <summary>
        ///   A unique name for the load combination.
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        ///  A list of tags for the load combination. This is a list of any strings you would like to use to categorize your load combinations. It is useful for separating load combinations into strength, service, or overstrength combinations as often required by building codes. This parameter has no effect on the analysis, but it can be used to restrict analysis to only the load combinations with the tags you specify.
        /// </summary>
        public Dictionary<string, double> Factors { get; protected set; }
        /// <summary>
        ///  A dictionary of load case names (`keys`) followed by their load factors (`items`). For example, the load combination 1.2D+1.6L would be represented as follows: `{'D': 1.2, 'L': 1.6}`. Defaults to {}.:type factors: dict, optional
        /// </summary>
        public List<string> Combo_tags { get; protected set; }

        #endregion
        public LoadCombo(string name, Dictionary<string,double> factors = null ,List<string> combo_tags = null) {
            this.Name = name;
            if( factors == null )
            {
                Factors = new Dictionary<string, double>();
            }
            else
            {
                Factors = factors;
            }
            Combo_tags = combo_tags;
        }
        /// <summary>
        ///  Adds a load case with its associated load factor
        /// </summary>
        public void AddLoadCase(string caseName, double factor)
        {
            Factors.Add(caseName, factor);
        }
        /// <summary>
        ///  Deletes a load case with its associated load factor
        /// </summary>
        public void AddLoadCase(string caseName)
        {
            Factors.Remove(caseName);
        }   
    }
}
