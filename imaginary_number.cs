using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fractales
{
    public class imaginary_number
    {
        double x;
        double y;
        public imaginary_number(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// // produit de nombre imaginaire
        /// </summary>
        /// <param name="Z1"nombre imganiaire 1.</param>
        /// <param name="Z2" nombre imaginaire 2</param>
        /// <returns> imaginary_number </returns>
        public imaginary_number prodz(imaginary_number z1, imaginary_number z2)
        {
            return new imaginary_number(z1.x * z2.x - z1.y * z2.y, z1.x * z2.y + z1.y * z2.x);
        }

        /// <summary>
        /// // somme de nombre imaginaire
        /// </summary>
        /// <param name="Z1"nombre imganiaire 1.</param>
        /// <param name="Z2" nombre imaginaire 2</param>
        /// <returns> imaginary_number </returns>
        public imaginary_number sumz(imaginary_number z1, imaginary_number z2)
        {
            return new imaginary_number(z1.x + z2.x, z1.y + z2.y);
        }

        /// <summary>
        /// // return le modulen d'un nombre imaginaire
        /// </summary>
        /// <param name="x" partie réel.</param>
        /// <param name="y" partie imaginaire </param>
        /// <returns> Double </returns>
        public double module()
        {
            return Math.Sqrt(this.x * this.x + this.y * this.y);
        }
    }
}
