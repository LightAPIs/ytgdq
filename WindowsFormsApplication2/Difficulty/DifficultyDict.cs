using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TyDll;

namespace WindowsFormsApplication2.Difficulty
{
    public class DifficultyDict
    {
        public Dictionary<string, double> Ranks = new Dictionary<string, double>();
        public DifficultyDict()
        {
            for (int i = 0; i < 10; i++)
            {
                string dicStr = TyDll.GetResources.GetText("Resources.DIC." + i.ToString() + ".txt");
                for (int j = 0; j < dicStr.Length; j++)
                {
                    double ra = 0.75;
                    switch (i)
                    {
                        case 0:
                            ra = 1;
                            break;
                        case 1:
                            ra = 1.25;
                            break;
                        case 2:
                            ra = 1.5;
                            break;
                        case 3:
                            ra = 1.75;
                            break;
                        case 4:
                            ra = 2;
                            break;
                        case 5:
                            ra = 2.5;
                            break;
                        case 6:
                            ra = 3;
                            break;
                        case 7:
                            ra = 4;
                            break;
                        case 8:
                            ra = 5;
                            break;
                        case 9:
                            ra = 7;
                            break;
                    }
                    this.Ranks.Add(dicStr[j].ToString(), ra);
                }
            }
        }
    }
}
