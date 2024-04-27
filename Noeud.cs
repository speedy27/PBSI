using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psiproject
{
    /// <summary>
    /// //Classe noeuf pour creation d'arbre de huffman
    /// </summary>
    public class Noeud 
    {
        private string p = null;
        private int frequence;
        private Noeud gauche;
        private Noeud droit;
        /// <summary>
        /// Premier Constructeur
        /// </summary>
        /// <param name="frequence">frquence du symbole.</param>
        /// <param name="gauche">Noeud gauche</param>
        /// <param name="droit">Noeud droit.</param>
        public Noeud(int frequence, Noeud gauche, Noeud droit)
        {
            this.frequence = frequence;
            this.gauche = gauche;
            this.droit = droit;
            this.p = null;
        }
        /// <summary>
        /// Second Constructeur.
        /// </summary>
        /// <param name="p">Symbole</param>
        /// <param name="frequence">Frequence du symbole.</param>
        public Noeud(string p, int frequence)
        {
            this.p = p;
            this.frequence = frequence;
        }
        /// <summary>
        /// Trsoisiéme constructeur.
        /// </summary>
        /// <param name="frequence">Fréquence pas de symbole.</param>
        public Noeud(int frequence)
        {
            this.frequence = frequence;
            this.P = null;
        }
        /// <summary>
        /// Constructeur vide utile pour des tests.
        /// </summary>
        public Noeud()
        {
            this.P = null;
        }
        /// <summary>
        /// Propriété en lecture de la fréquence d'un Noeud.
        /// </summary>
        public int Frequence
        {
            get { return frequence; }
        }
        /// <summary>
        /// Propriété en lecture  du Noeud droit d'un Noeud 
        /// </summary>
        public Noeud Droit
        {
            get { return droit; }
        }
        /// <summary>
        /// Propriété en lecture du Noeud gauche  d'un Noeud 
        /// </summary>
        public Noeud Gauche
        {
            get { return gauche; }
        }
        /// <summary>
        /// Propriété en lecture et ecriture du symbole d'un Noeud.
        /// </summary>
        public string P
        {
            get { return this.p; }
            set { this.p = value; }
        }

        

        /// <summary>
        /// M=éthode d'egalité pour un noeud.
        /// </summary>
        /// <param name="n">Le parametre de l'autre noeud dont on vérifie l'égalité.</param>
        /// <returns>return un booleen true si deux noeud sont egaux false dans le cas inverse.</returns>
        public bool Egale(Noeud n)
        {
            return this.frequence == n.frequence && this.p == n.p;
        }

        /// <summary>
        /// //Methodequi permet d'encodeer un symbole facon huffman. notions de noeud reccursif
        /// </summary>
        /// <param name="p"> symbole qu'on cherche.</param>
        /// <param name="rep"> liste de booleen dans lequel on va ecrire le code en binaire correpsondant a l=a décompression de huffman. </param>
        /// <returns></returns>
        public List<bool> trouve( string p, List<bool> rep)
        {
            if (droit == null && gauche == null)
            {
                if (p.Equals(this.p))
                {
                    return rep;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (gauche != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(rep);
                    leftPath.Add(false);

                    left = gauche.trouve(p, leftPath);
                }

                if (droit != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(rep);
                    rightPath.Add(true);
                    right = droit.trouve(p, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }







    }
}
