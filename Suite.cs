using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fractales
{
    public class Suite
    {
        public Suite()
        {
        }
        /// <summary>
        /// // permet de determiner la convergence iu non en fonction d'un point et attribut les couleurs en conséquence ( manderbrot ) 
        /// </summary>
        /// <param name="maxit"nombre maximum d'iteration pour definir la convergence.</param>
        /// <returns>Task<Pixel> </returns>
        public async Task<Pixel> isconvergente(imaginary_number z , imaginary_number c)
        {
            int i = 0,maxit = 255;

            while(z.module() < 4 && i < maxit)
            {
                z = z.prodz(z, z);
                z = z.sumz(z, c);
                i++;
            }
            if(i == maxit)
            {
                return new Pixel(0, 0, 0);
            }
            else
            {
                byte b = (byte)(i);
                if(b%2 == 0) { b = (byte)(255 - b); }
                return new Pixel(b, b, b);
            }
        }


    }
}
