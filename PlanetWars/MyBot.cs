using System;
using System.Collections.Generic;

public class MyBot
{
    // The DoTurn function is where your code goes. The PlanetWars object
    // contains the state of the game, including information about all planets
    // and fleets that currently exist. Inside this function, you issue orders
    // using the pw.IssueOrder() function. For example, to send 10 ships from
    // planet 3 to planet 8, you would say pw.IssueOrder(3, 8, 10).
    //
    // There is already a basic strategy in place here. You can use it as a
    // starting point, or you can throw it out entirely and replace it with
    // your own. Check out the tutorials and articles on the contest website at
    // http://www.ai-contest.com/resources.
    static Planet targetSort = null;
    public static int SortTarget(Planet p1, Planet p2)
    {
        if (targetSort != null)
        {
            double dx = targetSort.X() - p1.X();
            double dy = targetSort.Y() - p1.Y();
            int distance1 = (int)Math.Ceiling(Math.Sqrt(dx * dx + dy * dy));
            dx = targetSort.X() - p2.X();
            dy = targetSort.Y() - p2.Y();
            int distance2 = (int)Math.Ceiling(Math.Sqrt(dx * dx + dy * dy));
            return distance1.CompareTo(distance2);
        }
        else
        {
            return 0;
        }
    }
    public static int ComparePlanetValue(Planet x, Planet y)
    {
        double a = x.NumShips() / x.GrowthRate();
        double b = y.NumShips() / y.GrowthRate();
        return a.CompareTo(b);
    }
    public static void DoTurn(PlanetWars pw)
    {
        /*
         * basically I need to find out if I need to do something to maintain my advantage, by either attacking, reinforcing, or waiting
         */
        //double x = 0, y = 0;
        List<Planet> blue = pw.MyPlanets();
        List<Planet> opfor = pw.EnemyPlanets();
        List<Planet> targets = pw.NotMyPlanets();

        int opfor_army = 0;
        int opfor_production = 0;
        int blue_army = 0;
        int blue_production = 0;
        foreach (Planet p in opfor)
        {
            opfor_army += p.NumShips();
            opfor_production += p.GrowthRate();
        }
        foreach (Fleet f in pw.EnemyFleets())
        {
            opfor_army += f.NumShips();
        }
        foreach (Planet p in blue)
        {
            blue_army += p.NumShips();
            blue_production += p.GrowthRate();
        }
        foreach (Fleet f in pw.MyFleets())
        {
            blue_army += f.NumShips();
        }
        if (blue_army > opfor_army && blue_production > opfor_production)
        {
            // I win, attack
            foreach (Planet planet in blue)
            {
                targetSort = planet;
                opfor.Sort(SortTarget);
                foreach (Planet target in opfor)
                {
                    int shipsToSend = (int)Math.Floor(planet.NumShips()*0.5);
                    pw.IssueOrder(planet, target, shipsToSend);
                    planet.NumShips(planet.NumShips() - shipsToSend);
                }
                
            }
        }
        targets.Sort(ComparePlanetValue);
        foreach (Planet planet in blue)
        {
            foreach (Planet target in targets)
            {
                int turns = pw.Distance(planet.PlanetID(), target.PlanetID());
                int shipsToSend = target.NumShips() + (turns * target.GrowthRate()) + 1;
                int shipsSent = 0;
                foreach (Fleet fleet in pw.MyFleets())
                {
                    if (fleet.SourcePlanet() == planet.PlanetID())
                    {
                        shipsSent += fleet.NumShips();
                    }
                }
                if (planet.NumShips() > shipsToSend && shipsSent <= shipsToSend)
                {
                    int order = shipsToSend - shipsSent;
                    pw.IssueOrder(planet, target, order);
                    planet.NumShips(planet.NumShips() - order);
                }
            }


            
            
        }
        for (int i = 0; i < 3; i++)
        {
            Planet target = targets[i];
            targetSort = target;
            if (blue.Count > 1)
            {
            //    blue.Sort(SortTarget);
            }

            //pw.IssueOrder(pw.GetPlanet(blue[0].PlanetID()), target, pw.GetPlanet(blue[0].PlanetID()).NumShips() - 5);
            /*for (int j = 0; j < blue.Count; j++)
            {
                pw.IssueOrder(blue[j], target, 1);
                //blue[j].NumShips(0);
            }*/
            
        }
        /*foreach (Planet planet in blue)
        {
            if (planet.Owner() == 1)
            {
                foreach (Planet target in targets)
                {
                    int shipsExtra = 0;
                    int shipsSent = 0;
                    int shipsNeeded = 0;
                    shipsNeeded = target.NumShips() + 1;
                    int takeover = shipsExtra + shipsNeeded;
                    if (target.Owner() == 2)
                    {
                        int turns = pw.Distance(planet.PlanetID(), target.PlanetID());
                        takeover += turns * target.GrowthRate();
                    }
                    if (shipsSent < takeover)
                    {
                        if (planet.NumShips() <= shipsNeeded && planet.NumShips() > planet.GrowthRate()*5)
                        {
                          //  int shipsToSend = planet.NumShips();
                          //  shipsSent += shipsToSend;
                         //   pw.IssueOrder(planet, target, shipsToSend);
                         //   planet.NumShips(planet.NumShips() - shipsToSend);
                        }
                        else if (planet.NumShips() > takeover)
                        {
                            int shipsToSend = takeover;
                            shipsSent += shipsToSend;
                            pw.IssueOrder(planet, target, shipsToSend);
                            planet.NumShips(planet.NumShips() - shipsToSend);
                        }
                    }
                }
            }

        }*/
        
        /*

        else
        {
            if (closest != null)
            {
                int shipCount = 0;
                foreach (Planet p in blue)
                {
                    shipCount += p.NumShips();
                }
                foreach (Planet p in blue)
                {
                    if (shipCount > closest.NumShips())
                        pw.IssueOrder(p, closest, (int)Math.Floor(p.NumShips() * 0.5));
                }
            }
            if (next_closest != null)
            {
                foreach (Planet p in pw.MyPlanets())
                {
                    if (p.NumShips() > next_closest.NumShips())
                        pw.IssueOrder(p, next_closest, (int)Math.Floor(p.NumShips() * 0.5));
                }
            }
        }*/

    }

    public static void Main()
    {
        string line = "";
        string message = "";
        int c;
        try
        {
            while ((c = Console.Read()) >= 0)
            {
                switch (c)
                {
                    case '\n':
                        if (line.Equals("go"))
                        {
                            PlanetWars pw = new PlanetWars(message);
                            DoTurn(pw);
                            pw.FinishTurn();
                            message = "";
                        }
                        else
                        {
                            message += line + "\n";
                        }
                        line = "";
                        break;
                    default:
                        line += (char)c;
                        break;
                }
            }
        }
        catch (Exception)
        {
            // Owned.
        }
    }
}

