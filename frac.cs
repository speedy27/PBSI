using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace fractales
{

    public class frac
    {
        Screen screen = Screen.PrimaryScreen;
        Suite suite;
        private Pixel[,] mafractale;
        private byte[] header;
        private byte[] headerio;
        
        public frac()
        {
            
            //int largeur = screen.Bounds.Width;
            //int hauteur = screen.Bounds.Height;
            int largeur = 2000;
            int hauteur = 1000;

            mafractale = new Pixel[hauteur, largeur];
            byte[] t = BitConverter.GetBytes(3*(hauteur * largeur) + 54);
            this.header = new byte[14] { 66,77,t[0],t[1],t[2],t[3],0,0,0,0,54,0 ,0, 0};
            byte[] t1 = BitConverter.GetBytes(hauteur * largeur*3);
            byte[] t2 = BitConverter.GetBytes(hauteur);
            byte[] t3 = BitConverter.GetBytes(largeur);
            this.headerio = new byte[40] { 40, 0, 0, 0, t3[0], t3[1], t3[2], t3[3], t2[0], t2[1], t2[2], t2[3], 1, 0, 24, 0, 0,0,0,0, t1[0], t1[1], t1[2], t1[3], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
            for(int i = 0; i < hauteur; i++)
            {
                for(int j = 0; j < largeur; j++)
                {
                    mafractale[i, j] = new Pixel(255, 0, 0);
                }
            }
        }
        /// <summary>
        /// // permet l'enregistrement d'une image dans un fichier
        /// </summary>
        /// <param name="header"en tête conventionel de données au début d'un fichier qui contient des informations sur la structure du fichier.</param>
        /// <param name="headerio" suite des données au début d'un fichier qui contient des informations sur la structure du fichier.</param>
     
        public void From_Image_To_File(string file)
        {
            byte[] rep = new byte[54 + 3*(mafractale.GetLength(0) * mafractale.GetLength(1))];
            for (int i = 0; i < 14; i++)
                rep[i] = header[i];

            for (int i = 14; i < 54; i++)
                rep[i] = headerio[i - 14];

            int k = 54;
            for (int i = 0; i < mafractale.GetLength(0); i++)
            {
                for (int j = 0; j < mafractale.GetLength(1); j++)
                {
                    rep[k] = mafractale[i, j].Rouge;
                    rep[k + 1] = mafractale[i, j].Vert;
                    rep[k + 2] = mafractale[i, j].Bleu;
                    k += 3;
                }
            }
            File.WriteAllBytes(file, rep);
        }

        /// <summary>
        /// // crée la fractale
        /// </summary>
        /// <param name="suite"suite mathématique définie au préalable pour la création de la fractale.</param>
        /// <param name="async / await" permet de faire plusieurs tache en même temps pour un gain de temps.</param>
        public async void creationfrac()
        {
            Suite suite = new Suite();
            int hauteur = mafractale.GetLength(0);
            int largeur = mafractale.GetLength(1);
            for (int i = 0; i < hauteur; i++)
            {
                for(int j = 0;j < largeur; j++)
                {
                    double x = j - largeur / 2;
                    double y = hauteur / 2 - i;
                    x = x / 500.0;
                    y = y / 500.0;
                    imaginary_number z = new imaginary_number(x, y);
                    mafractale[i, j] =  await suite.isconvergente(z,z);
                }
            }
        }

        public async void creationfrac2()
        {
            Suite suite = new Suite();
            int hauteur = mafractale.GetLength(0);
            int largeur = mafractale.GetLength(1);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    double x = j - largeur / 2;
                    double y = hauteur / 2 - i;
                    x = x / 500.0;
                    y = y / 500.0;
                    imaginary_number z = new imaginary_number(x, y);
                    mafractale[i, j] = await suite.isconvergente(z,new imaginary_number(-1,0));
                }
            }
        }

        public async void creationfrac3()
        {
            Suite suite = new Suite();
            int hauteur = mafractale.GetLength(0);
            int largeur = mafractale.GetLength(1);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    double x = j - largeur / 2;
                    double y = hauteur / 2 - i;
                    x = x / 500.0;
                    y = y / 500.0;
                    imaginary_number z = new imaginary_number(x, y);
                    mafractale[i, j] = await suite.isconvergente(z, new imaginary_number(-0.2, 0.7));
                }
            }
        }








    }
}
