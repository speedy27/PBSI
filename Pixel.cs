using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fractales
{
    /// <summary>
    /// // Définie la table pixel sur 3 byte
    /// </summary>
    /// <returns> Pixel </returns>
    public class Pixel
    {
        private byte rouge;
        private byte vert;
        private byte bleu;

        public Pixel(byte rouge, byte vert, byte bleu)
        {
            this.rouge = rouge;
            this.bleu = bleu;
            this.vert = vert;
        }

        public Pixel(Pixel c)
        {
            this.rouge = c.rouge;
            this.vert = c.vert;
            this.bleu = c.bleu;
        }

        /// <summary>
        /// // retourner une représentation sous forme de chaîne de caractères de l'objet sur lequel elle est appelée.
        /// </summary>
        /// <param name="this.rouge" byte 1.</param>
        /// <param name="this.vert" nombre byte 2</param>
        ///  /// <param name="this.bleu " byte 3 </param>
        /// <returns> string </returns>
        public string toString()
        {
            return "" + this.rouge + " " + this.vert + " " + this.bleu + "";
        }


        public byte Rouge
        {
            get { return this.rouge; }
        }
        public byte Vert
        {
            get { return this.vert; }
        }
        public byte Bleu
        {
            get { return this.bleu; }
        }

    }
}
