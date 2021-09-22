using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack
{
    internal class Program
    {
        public static int Score(List<string> mainJoueur, Dictionary<string, int> valeurCartes)
        {
            int currentScore = 0;

            foreach (string card in mainJoueur)
                currentScore += valeurCartes[card];
            return currentScore;
        }

        public static void Main(string[] args)
        {
            // Initialization

            Dictionary<string, int> valeurCartes = new Dictionary<string, int>();
            valeurCartes.Add("A", 1);
            valeurCartes.Add("2", 2);
            valeurCartes.Add("3", 3);
            valeurCartes.Add("4", 4);
            valeurCartes.Add("5", 5);
            valeurCartes.Add("6", 6);
            valeurCartes.Add("7", 7);
            valeurCartes.Add("8", 8);
            valeurCartes.Add("9", 9);
            valeurCartes.Add("J", 10);
            valeurCartes.Add("Q", 10);
            valeurCartes.Add("K", 10);

            List<string> joueurH = new List<string>();
            List<string> joueurO = new List<string>();

            // Card pack preparation

            Console.WriteLine("Entrez votre nom : ");
            string playerName = Console.ReadLine();
            Console.WriteLine("Combien de decks voulez vous dans le paquet ?");
            int deckAmount = Int32.Parse(Console.ReadLine());
            List<string> paquet = new List<string>();
            List<string> tempPaquet = new List<string>();
            for (int i = 0; i < 4 * deckAmount; i++)
            {
                foreach (KeyValuePair<string, int> kvp in valeurCartes)
                {
                    tempPaquet.Add(kvp.Key);
                    paquet.AddRange(tempPaquet);
                }
            }

            paquet = paquet.OrderBy(x => Guid.NewGuid()).ToList();

            // Card distribution

            for (int i = 0; i < 2; i++)
            {
                joueurH.Add(paquet.Last());
                paquet.RemoveAt(paquet.Count - 1);
                joueurO.Add(paquet.Last());
                paquet.RemoveAt(paquet.Count - 1);
            }

            // Hand display

            Console.WriteLine("( " + Score(joueurH, valeurCartes) + " pts ) " + playerName + " : {0} / {1}", joueurH[0],
                joueurH[1]);
            Console.WriteLine("( " + Score(joueurO, valeurCartes) + " pts ) " + "Ordinateur : ? / {0}", joueurO[1]);

            // Turn

            bool stopJoueur = false;
            bool stopOrdinateur = false;
            bool finPartie = false;

            // Player decision

            while (finPartie == false)
            {
                if (stopJoueur == false)
                {
                    Console.WriteLine("Voulez-vous piocher une nouvelle carte ?");
                    Console.WriteLine("o - Oui");
                    Console.WriteLine("n - Non");
                    string choixJoueur = Console.ReadLine();

                    if (choixJoueur == "o")
                    {
                        Console.WriteLine(playerName + " : Je pioche.");
                        joueurH.Add(paquet.Last());
                        paquet.RemoveAt(paquet.Count - 1);
                    }
                    else if (choixJoueur == "n")
                    {
                        Console.WriteLine(playerName + " : Je m'arrête là.");
                        stopJoueur = true;
                    }
                    else
                    {
                        Console.WriteLine("Réponse invalide.");
                        continue;
                    }

                    if (stopOrdinateur == false)
                    {
                        int sumMainOrdinateur = 0;

                        foreach (string card in joueurO)
                        {
                            sumMainOrdinateur += valeurCartes[card];
                        }

                        if (sumMainOrdinateur < 15)
                        {
                            Console.WriteLine("Ordinateur : Je pioche.");
                            joueurO.Add(paquet.Last());
                            paquet.RemoveAt(paquet.Count - 1);
                        }
                        else
                        {
                            Console.WriteLine("Ordinateur : Je m'arrête là.");
                            stopOrdinateur = true;
                        }
                    }
                    Console.WriteLine("( " + Score(joueurH, valeurCartes) + " pts ) " + playerName + " : ");
                    Console.WriteLine(string.Join(" ", joueurH));
                    Console.WriteLine("( " + Score(joueurO, valeurCartes) + " pts ) " + "Ordinateur : ");
                    Console.WriteLine("? " + string.Join(" ", joueurO.Skip(1)));

                    // Endgame

                    if (stopOrdinateur && stopJoueur)
                    {
                        finPartie = true;
                        if (Score(joueurH, valeurCartes) > Score(joueurO, valeurCartes))
                            Console.WriteLine("Vous avez gagné cette main !");
                        else
                            Console.WriteLine("Vous avez perdu cette main.");
                    }

                    if (Score(joueurH, valeurCartes) > 21)
                    {
                        finPartie = true;
                        Console.WriteLine("Vous avez bust ! Le croupier gagne.");
                    }
                    else if (Score(joueurO, valeurCartes) > 21)
                    {
                        finPartie = true;
                        Console.WriteLine("Le croupier a bust ! Vous gagnez.");
                    }
                }
            }
        }
    }
}

// TODO : soft hands