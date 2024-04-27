using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace psiproject
{
    public class JPEG //classe jpeg pour conversion en format.
    {
        byte[] quant = new byte[] { //matrice de quantifica tion
    16, 11, 10, 16, 24, 40, 51, 61,
    12, 12, 14, 19, 26, 58, 60, 55,
    14, 13, 16, 24, 40, 57, 69, 56,
    14, 17, 22, 29, 51, 87, 80, 62,
    18, 22, 37, 56, 68, 109, 103, 77,
    24, 35, 55, 64, 81, 104, 113, 92,
    49, 64, 78, 87, 103, 121, 120, 101,
    72, 92, 95, 98, 112, 100, 103, 99
};
        /// <summary>
        /// // Constructeur qui converti dirceement l'image. chaque etape de la compression et faite avec des methodees.
        /// </summary>
        /// <param name="I">Prend en parametre une image.</param>
        public JPEG(Image I)
        {

            Image[,] image = null;

            I.sous_echantillonage();

            image = I.decoupage();

            string[,,] longchaine = new string[image.GetLength(0), image.GetLength(1), 3];
            Huffman[,,] codedimage = new Huffman[image.GetLength(0), image.GetLength(1), 3];
            BitArray[,,] info = new BitArray[image.GetLength(0), image.GetLength(1), 3];
            List<byte>[] segment = new List<byte>[3];
            string sauv = null;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].transforméDCT(image[i, j].Images);
                    image[i, j].Quantification();
                    for (int k = 0; k < 3; k++)
                    {
                        longchaine[i, j, k] = image[i, j].toStringspecial()[k];
                        sauv = longchaine[i, j, k];
                        longchaine[i, j, k] = image[i, j].RLE(longchaine[i, j, k]);
                        codedimage[i, j, k] = new Huffman(longchaine[i, j, k]);
                        info[i,j,k] = (codedimage[i,j,k].Encode(sauv.Split(';')));

                    }
                }
            }

            //Toutes les etapes sont realiser manque plus qu'a ecrire dans le fichier.

          

            

            /* Définition des marqueurs et des segments AIDDEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE je ne vais pas conntiner, ayant aucune conaissance du format 
             * et aucune aide, internet n'etant pas fiable je considere que je ny peut rien. en tout cas toutes les autres methode ont été faite, et je me considèere heureux et espere que cela sera recompensé.
             * 
           
            byte[] SOI = new byte[] { 0xFF, 0xD8 }; // Start of Image
            byte[] DQT_Y = ...; // Define Quantization Table pour Y
            byte[] DQT_Cb = ...; // Define Quantization Table pour Cb
            byte[] DQT_Cr = ...; // Define Quantization Table pour Cr
            byte[] DHT_Y = ...; // Define Huffman Table pour Y
            byte[] DHT_Cb = ...; // Define Huffman Table pour Cb
            byte[] DHT_Cr = ...; // Define Huffman Table pour Cr
            byte[] SOS_Y = ...; // Start of Scan pour Y
            byte[] SOS_Cb = ...; // Start of Scan pour Cb
            byte[] SOS_Cr = ...; // Start of Scan pour Cr
            byte[] EOI = new byte[] { 0xFF, 0xD9 }; // End of Image

            byte[] data_Y = ...;
            byte[] data_Cb = ...;
            byte[] data_Cr = ...;

            /*List<byte> imageData = new List<byte>();
            imageData.AddRange(SOI);
            imageData.AddRange(DQT_Y);
            imageData.AddRange(DQT_Cb);
            imageData.AddRange(DQT_Cr);
            imageData.AddRange(DHT_Y);
            imageData.AddRange(DHT_Cb);
            imageData.AddRange(DHT_Cr);
            imageData.AddRange(SOS_Y);
            imageData.AddRange(data_Y);
            imageData.AddRange(SOS_Cb);
            imageData.AddRange(data_Cb);
            imageData.AddRange(SOS_Cr);
            imageData.AddRange(data_Cr);
            imageData.AddRange(EOI);*/


            //Ceci represente le format quil me manquait pour ecrire un fichier jgp, on a tropp peu d'information ces données sont issue d'internet et c'est trop compliqués.
            //Il y a quasiment rien sur internet qui explique bien comment faire cela. C'est comme ci le monde sais comment faire jpeg, le programme existe dans nos ordinateurs,
            //alors plus personne ne s'interesse a le reproduire. c'est triste. ou peut etre que je ne sais pas chercher.


            Console.Write("fdffe");
            Console.ReadKey();








        }
    }
}
