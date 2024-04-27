using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace psiproject
{
    public class Huffman
    {
        private Noeud root;
        private List<Noeud> arbre;

        /// <summary>
        /// //Initialisation de huffman a partir d'un string, creation entiere de larbre pour encoder des symboles qui sont des nombres dans notre cas.
        /// </summary>
        /// <param name="chaine">Chaine de charactere avec laquelle on va construire l'arbre de huffman.</param>
        public Huffman(string chaine) 
        {
            arbre = new List<Noeud>();
            string[] ch = chaine.Split(';');
            string[] sauv = null;
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i].Contains(':'))
                {
                    sauv = ch[i].Split(':');
                    arbre.Add(new Noeud(sauv[1], int.Parse(sauv[0])));
                }
                else
                {
                    arbre.Add(new Noeud(ch[i], 1));
                }

            }
            triabulle(arbre);

            while(arbre.Count > 1 ) 
            {
                triabulle(arbre);
                if(arbre.Count >=2)
                {
                    List<Noeud> taken = arbre.Take(2).ToList<Noeud>();
                    Noeud parent = new Noeud(taken[0].Frequence + taken[1].Frequence, taken[0], taken[1]);
                    arbre.Remove(taken[0]);
                    arbre.Remove(taken[1]);
                    arbre.Add(parent);
                }
            }

            triabulle(arbre);

            this.root = arbre[0];




        }

        /// <summary>
        /// Propriété root, racine en lecture.
        /// </summary>
        public Noeud Root
        {
            get { return this.root; }
        }
        /// <summary>
        /// TRi de l'arbre, pour simplifier les choses.
        /// </summary>
        /// <param name="l">Prend une list de noeud, c'est l'arbre.</param>
        public void triabulle(List<Noeud> l)
        {
            Noeud sauv = null;
            for (int i = 0; i < l.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (l[j].Frequence > l[j + 1].Frequence)
                    {
                        sauv = l[j];
                        l[j] = l[j + 1];
                        l[j + 1] = sauv;
                    }
                }
            }
        }

        /// <summary>
        /// Encore tout un string avec plusieurs symbole reconnu dans l'instance de classe huffman grace a son arbre.
        /// </summary>
        /// <param name="source">Correspons a chaque symbole qu'on veux coder.</param>
        /// <returns>un tableau de bits, qui combine tout les symboles.</returns>
        public BitArray Encode(string[] source) //Encode un  symbole, ici un nombre en bits
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.root.trouve(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        /// <summary>
        /// //Conversion list de bits en liste de bytes.
        /// </summary>
        /// <param name="ch">Le tableau de bits.</param>
        /// <returns>le tableau de bytes.</returns>
        public List<byte> Convertbittobyte(List<bool> ch)
        {

            while (ch.Count % 8 != 0)
            {
                ch.Add(false);
            }
            List<byte> rep = new List<byte>(ch.Count / 8);
            byte b = 0;
            for (int i = 0; i < rep.Count; i++)
            {
                for (int j = 0 + i * 8; j < 8 + i * 8; j++)
                {
                    if (ch[j] == true)
                    {
                        b += (byte)Math.Pow(2, ch.Count() - 1 - j);
                    }

                }
                rep[i] = b;
                b = 0;
            }
            return rep;

        }

        /// <summary>
        /// // rendre en strnig une iste de booleen au final ne sert a rien.
        /// </summary>
        /// <param name="l">Liste de booléean qui correspond a l'encodage d'un sylmbole en bool.</param>
        /// <returns>une chaine avec les booléeans</returns>
        public string toString(List<bool> l) {
            string chaine = "";
            foreach (bool b in l)
            {
                if (b == true) { chaine += 1; }
                else { chaine += 0; }
            }
            return chaine;
        }

        
    }
}
