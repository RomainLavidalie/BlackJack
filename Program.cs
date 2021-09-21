using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Initialization

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

            //Card pack preparation

            Console.WriteLine("Entrez votre nom : ");
            string playerName = Console.ReadLine();
            Console.WriteLine("Combien de decks voulez vous dans le paquet ?");
            int deckAmount = Int32.Parse(Console.ReadLine());
            List<string> paquet = new List<string>();
            List<string> tempPaquet = new List<string>();
            for (int i = 0; i < 8 * deckAmount; i++)
            {
                foreach (KeyValuePair<string, int> kvp in valeurCartes)
                {
                    tempPaquet.Add(kvp.Key);
                    paquet.AddRange(tempPaquet);
                }
            }

            tempPaquet = tempPaquet.OrderBy(x => Guid.NewGuid()).ToList();
            
            Console.WriteLine(string.Join(" ", tempPaquet));

            //Card distribution

            for (int i = 0; i < 2; i++)
            {
                joueurH.Add(paquet.Last());
                paquet.Remove(paquet.Last());
                joueurO.Add(paquet.Last());
                paquet.Remove(paquet.Last());
            }

            //Hand display

            Console.WriteLine(playerName + " : {0} / {1}", joueurH[0], joueurH[1]);
            Console.WriteLine("Ordinateur : ? / {0}", joueurO[1]);

            //Turn

            bool stopJoueur = false;
            bool stopOrdinateur = false;
            bool finPartie = false;

            //Player decision

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
                        paquet.Remove(paquet.Last());
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
                            paquet.Remove(paquet.Last());
                        }
                        else
                        {
                            Console.WriteLine("Ordinateur : Je m'arrête là.");
                            stopOrdinateur = true;
                        }
                    }

                    Console.WriteLine(playerName + " : ");
                    Console.WriteLine(string.Join(" ", joueurH));
                    Console.WriteLine("Ordinateur : ");
                    Console.WriteLine("? " + string.Join(" ", joueurH.Skip(1)));

                    //Endgame

                    int sumJoueur = 0;
                    int sumOrdinateur = 0;

                    foreach (string card in joueurH)
                        sumJoueur += valeurCartes[card];
                    foreach (string card in joueurO)
                        sumOrdinateur += valeurCartes[card]
                            ;

                    if (stopOrdinateur == true && stopJoueur == true)
                        finPartie = true;
                    if (sumJoueur > 21 || sumOrdinateur > 21)
                        finPartie = true;
                }
            }
        }
    }
}

//TODO : fonction de score
//TODO : déterminer le winner
//TODO : soft hands